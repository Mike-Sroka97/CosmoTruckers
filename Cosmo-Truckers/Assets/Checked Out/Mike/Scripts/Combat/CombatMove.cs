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
    [SerializeField] protected int Damage;
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
    [Header("Testing Variables")]
    [SerializeField] bool spawnTest = false;
    [SerializeField] bool startMoveTest = false; 

    protected float currentTime = 0;
    protected bool trackTime = false;
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

    public virtual void StartMove() { }
    public virtual List<Character> NoTargetTargeting() { Debug.LogError("You didn't setup the override you devilish cunt"); return null; }

    public void SetSpawns()
    {
        //TODO check if player controller is the same real world person

        players = FindObjectsOfType<Player>();
        SetupPlayerMaterials();

        for (int i = 0; i < players.Length; i++)
            players[i].transform.position = spawnPoints[i].position;
    }

    public virtual void EndMove()
    {
        if (MoveEnded)
            return;

        MoveEnded = true;

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

    protected void ApplyAugment(Character character, int augmentStacks)
    {
        character.AddDebuffStack(DebuffToAdd, augmentStacks);
    }

    protected void DealDamageOrHealing(Character character, int currentDamage)
    {
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
            float newDamage = baseDamage * DamageAdj;
            baseDamage = (int)newDamage;
            int totalDamage = baseDamage * numberOfHits + character.FlatDamageAdjustment * numberOfHits;

            character.GetComponent<Character>().TakeMultiHitDamage(totalDamage / numberOfHits, numberOfHits);
        }

        else if (currentDamage > 0 && isHealing)
        {
            //1 being base damage
            float HealingAdj = 1;

            HealingAdj = CombatManager.Instance.GetCurrentCharacter.Stats.Restoration / 100;
            float newHealing = baseDamage * HealingAdj;
            baseDamage = (int)newHealing;
            int totalHealing = baseDamage * numberOfHits + character.FlatHealingAdjustment * numberOfHits;

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
        return 0; //TODO (lol)
    }

    protected int CalculateMultiplayerAugmentScore(int playerAugScore)
    {
        int augmentStacks = playerAugScore * augmentStacksPerScore;
        augmentStacks += baseAugmentStacks;
        if (augmentStacks > maxAugmentStacks)
            augmentStacks = maxAugmentStacks;
        return augmentStacks;
    }

    protected virtual void PlayerEnemyDifference(Character character)
    {
        //implement as needed
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
        if (MoveEnded)
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
            if (player.MyCharacter.PlayerNumber == 1)
                foreach (SpriteRenderer sprite in player.MyRenderers)
                {
                    player.StartingMaterial = CombatManager.Instance.playerOneMaterial;
                    sprite.material = CombatManager.Instance.playerOneMaterial;
                    sprite.sortingOrder += 0;
                }

            else if (player.MyCharacter.PlayerNumber == 2)
                foreach (SpriteRenderer sprite in player.MyRenderers)
                {
                    player.StartingMaterial = CombatManager.Instance.playerTwoMaterial;
                    sprite.material = CombatManager.Instance.playerTwoMaterial;
                    sprite.sortingOrder += 1;
                }

            else if (player.MyCharacter.PlayerNumber == 3)
                foreach (SpriteRenderer sprite in player.MyRenderers)
                {
                    player.StartingMaterial = CombatManager.Instance.playerThreeMaterial;
                    sprite.material = CombatManager.Instance.playerThreeMaterial;
                    sprite.sortingOrder += 2;
                }

            else
                foreach (SpriteRenderer sprite in player.MyRenderers)
                {
                    player.StartingMaterial = CombatManager.Instance.playerFourMaterial;
                    sprite.material = CombatManager.Instance.playerFourMaterial;
                    sprite.sortingOrder += 3;
                }
        }
    }

    protected void SetupMultiplayer()
    {
        PlayerScores = new Dictionary<PlayerCharacter, int>();
        PlayerAugmentScores = new Dictionary<PlayerCharacter, int>();
        PlayersDead = new Dictionary<PlayerCharacter, bool>();

        for (int i = 0; i < players.Length; i++)
        {
            PlayerScores.Add(players[i].MyCharacter, 0);
            PlayerAugmentScores.Add(players[i].MyCharacter, 0);
            PlayersDead.Add(players[i].MyCharacter, false);
        }
    }
}
