using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CombatMove : MonoBehaviour
{
    [SerializeField] TargetType TypeOfAttack;
    [SerializeField] Transform[] spawnPoints;
    [SerializeField] protected GameObject[] layouts;
    //For games that can deal player damage
    [SerializeField] public int Damage;
    [SerializeField] protected DebuffStackSO DebuffToAdd;
    [SerializeField] protected bool playerEnemyTargetDifference = false;
    public float MinigameDuration;

    public int Score;
    [HideInInspector] public bool PlayerDead = false;
    [HideInInspector] public bool MoveEnded = false;
    protected bool endMoveCalled = false; 
    public int AugmentScore = 0;
    [Space(20)]
    [Header("Minigame Variables")]
    [SerializeField] protected int maxScore;
    [SerializeField] protected int augmentStacksPerScore;
    [SerializeField] protected int maxAugmentStacks;
    [SerializeField] protected int baseDamage;
    [SerializeField] protected int baseAugmentStacks;
    [SerializeField] protected float timeToEndMove = 1f;
    [SerializeField] protected bool isDamaging;
    [SerializeField] protected bool isHealing;
    [SerializeField] protected bool pierces = false;

    [Space(20)]
    [Header("Multiplayer Variables")]
    public Dictionary<PlayerCharacter, int> PlayerScores;
    public Dictionary<PlayerCharacter, int> PlayerAugmentScores;
    public Dictionary<PlayerCharacter, bool> PlayersDead;

    [Space(20)]
    [Header("Boss Variables")]
    [SerializeField] bool bossMove = false;
    public bool FightWon = false;

    [Space(20)]
    [Header("Testing Variables")]
    [SerializeField] bool spawnTest = false;
    [SerializeField] bool startMoveTest = false; 

    protected float currentTime = 0;
    protected bool trackTime = false;
    private bool multiplayer = false;
    protected Player[] players;

    public bool GetIsDamaging() { return isDamaging; }
    public bool GetIsHealing() { return isHealing; }

    private void Awake()
    {
        if (spawnTest)
        {
            Debug.LogError("Spawn Test is active"); 
            SetSpawns(); 
        }

        if (startMoveTest)
        {
            Debug.LogError("Start Move Test is active");
            StartMove();
        }
    }

    protected virtual void Update()
    {
        if (!trackTime)
            return;

        TrackTime();
    }

    public enum TargetType
    {
        NoTarget,
        SelfTarget,
        SingleTarget,
        MultiTarget,
        Cone,
        MultiTargetChoice,
        AOE,
        AllTarget
    }

    public virtual void StartMove() { trackTime = true; }
    public virtual List<Character> NoTargetTargeting() { Debug.LogError("You didn't setup the override you devilish cunt"); return null; }

    public void SetSpawns() //Rename because this is quickly becoming an INIT for the combatMove class
    {
        //TODO check if player controller is the same real world person

        players = FindObjectsOfType<Player>();
        SetupPlayerMaterials();

        for (int i = 0; i < players.Length; i++)
            players[i].transform.position = spawnPoints[i].position;

        if (bossMove)
            SetupBossFight();
    }

    public virtual void EndMove()
    {
        MoveEnded = true;

        if(multiplayer)
            MultiplayerEndMove();
        else
            SinglePlayerEndMove();
    }

    private void SinglePlayerEndMove()
    {
        foreach (Character character in CombatManager.Instance.GetCharactersSelected)
        {
            //Calculate Damage
            int currentDamage = CalculateScore();

            //Calculate Augment Stacks
            int augmentStacks = CalculateAugmentScore();

            //Deals damage or heals target based on booleans set in inspector
            DealDamageOrHealing(character, currentDamage);

            //Apply augment
            ApplyAugment(character, augmentStacks);
        }
    }
    private void MultiplayerEndMove()
    {
        //Calculate the score seperately for each player who was in the minigame
        for (int i = 0; i < CombatManager.Instance.GetCharactersSelected.Count; i++)
        {
            //Calculate this players score
            int currentScore = CalculateMultiplayerScore(PlayerScores[CombatManager.Instance.GetCharactersSelected[i].GetComponent<PlayerCharacter>()]);
            DealDamageOrHealing(CombatManager.Instance.GetCharactersSelected[i], currentScore);

            //Calculate this players augment Score
            int currentAugmentScore = CalculateMultiplayerAugmentScore(PlayerScores[CombatManager.Instance.GetCharactersSelected[i].GetComponent<PlayerCharacter>()]);
            ApplyAugment(CombatManager.Instance.GetCharactersSelected[i], currentAugmentScore);
        }
    }

    protected void ApplyAugment(Character character, int augmentStacks)
    {
        character.AddDebuffStack(DebuffToAdd, augmentStacks);
    }

    public void DealDamageOrHealing(Character character, int currentDamage, int damage = 0)
    {
        //can be used to set if the move is damaging or healing end move
        if(damage != 0)
        {
            if (damage == 1)
            {
                isHealing = false;
                isDamaging = true;
            }
            else
            {
                isHealing = true;
                isDamaging = false;
            }
        }

        if (currentDamage > 0 && isDamaging)
        {
            //1 being base damage
            float DamageAdj = 1;

            //Damage on players must be divided by 100 to multiply the final
            DamageAdj = (float)CombatManager.Instance.GetCurrentCharacter.Stats.Damage / 100;

            character.TakeDamage((int)(currentDamage * DamageAdj + CombatManager.Instance.GetCurrentCharacter.FlatDamageAdjustment), pierces);
        }

        else if (currentDamage > 0 && isHealing)
        {
            //1 being base damage
            float HealingAdj = 1;

            //Damage on players must be divided by 100 to multiply the final
            HealingAdj = (float)CombatManager.Instance.GetCurrentCharacter.Stats.Restoration / 100;

            character.TakeHealing((int)(currentDamage * HealingAdj + CombatManager.Instance.GetCurrentCharacter.FlatHealingAdjustment), pierces);
        }
    }

    protected void DealMultiHitDamageOrHealing(Character character, int currentDamage, int numberOfHits)
    {
        if (currentDamage > 0 && isDamaging)
        {
            //1 being base damage
            float DamageAdj = 1;

            DamageAdj = CombatManager.Instance.GetCurrentCharacter.Stats.Damage / 100;
            float newDamage = currentDamage * DamageAdj;
            baseDamage = (int)newDamage;
            int totalDamage = currentDamage * numberOfHits + character.FlatDamageAdjustment * numberOfHits;

            character.GetComponent<Character>().TakeMultiHitDamage(totalDamage / numberOfHits, numberOfHits);
        }

        else if (currentDamage > 0 && isHealing)
        {
            //1 being base damage
            float HealingAdj = 1;

            HealingAdj = CombatManager.Instance.GetCurrentCharacter.Stats.Restoration / 100;
            float newHealing = currentDamage * HealingAdj;
            baseDamage = (int)newHealing;
            int totalHealing = currentDamage * numberOfHits + character.FlatHealingAdjustment * numberOfHits;

            character.GetComponent<Character>().TakeMultiHitHealing(totalHealing / numberOfHits, numberOfHits);
        }
    }

    protected int CalculateScore()
    {
        if (Score < 0)
            Score = 0;
        if (Score >= maxScore)
            Score = maxScore;

        int currentDamage;
        currentDamage = Score * Damage;
        currentDamage += baseDamage;
        return currentDamage;
    }

    protected int CalculateAugmentScore()
    {
        int augmentStacks = AugmentScore * augmentStacksPerScore;
        augmentStacks += baseAugmentStacks;
        if (augmentStacks > maxAugmentStacks)
            augmentStacks = maxAugmentStacks;
        return augmentStacks;
    }

    protected int CalculateMultiplayerScore(int playerScore)
    {
        if (Score < 0)
            Score = 0;
        if (Score >= maxScore)
            Score = maxScore;

        int currentDamage;
        currentDamage = playerScore * Damage;
        currentDamage += baseDamage;
        return currentDamage;
    }

    protected int CalculateMultiplayerAugmentScore(int playerAugScore)
    {
        int augmentStacks = playerAugScore * augmentStacksPerScore;
        augmentStacks += baseAugmentStacks;
        if (augmentStacks > maxAugmentStacks)
            augmentStacks = maxAugmentStacks;
        return augmentStacks;
    }

    protected void GenerateLayout()
    {
        if(layouts.Length > 0)
        {
            int random = UnityEngine.Random.Range(0, layouts.Length);
            Instantiate(layouts[random], transform);
        }
    }

    protected virtual void TrackTime()
    {
        if (MoveEnded || !trackTime)
            return;

        currentTime += Time.deltaTime;

        if ((currentTime >= MinigameDuration || PlayerDead) && !MoveEnded && !endMoveCalled)
        {
            MoveEnded = true;
        }
    }

    public void CheckScore()
    {
        if (Score >= maxScore)
        {
            float timeRemaining = MinigameDuration - currentTime;

            if (timeRemaining > timeToEndMove)
            {
                if (MoveEnded)
                    return;

                StartCoroutine(DelayedCallEndMove());
            }
        }
    }

    private IEnumerator DelayedCallEndMove()
    {
        endMoveCalled = true; 
        yield return new WaitForSeconds(timeToEndMove);
        MoveEnded = true;
    }

    private void SetupPlayerMaterials()
    {
        foreach(Player player in players)
        {
            foreach (SpriteRenderer sprite in player.MyRenderers)
            {
                player.StartingMaterial = CombatManager.Instance.playerMaterials[player.MyCharacter.PlayerNumber - 1];
                sprite.material = CombatManager.Instance.playerMaterials[player.MyCharacter.PlayerNumber - 1];
                sprite.sortingOrder += player.MyCharacter.PlayerNumber - 1;
            }
        }
    }

    protected void SetupMultiplayer()
    {
        multiplayer = true;

        PlayerScores = new Dictionary<PlayerCharacter, int>();
        PlayerAugmentScores = new Dictionary<PlayerCharacter, int>();
        PlayersDead = new Dictionary<PlayerCharacter, bool>();

        for (int i = 0; i < players.Length; i++)
        {
            PlayerScores.Add(players[i].MyCharacter, 0);
            PlayerScores[players[i].MyCharacter] = Score; 
            PlayerAugmentScores.Add(players[i].MyCharacter, 0);
            PlayerAugmentScores[players[i].MyCharacter] = AugmentScore;
            PlayersDead.Add(players[i].MyCharacter, false);
        }

        foreach (PlayerPickup pickup in GetComponentsInChildren<PlayerPickup>())
            pickup.multiplayer = true;

        foreach (TrackPlayerDeath deathSource in GetComponentsInChildren<TrackPlayerDeath>())
            deathSource.Multiplayer = true;
    }

    private void SetupBossFight()
    {
        foreach (TrackPlayerDeath deathSource in GetComponentsInChildren<TrackPlayerDeath>())
            deathSource.Boss = true;
    }
}
