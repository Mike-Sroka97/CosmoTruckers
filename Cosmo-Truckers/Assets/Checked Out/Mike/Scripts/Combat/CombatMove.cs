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
    [SerializeField] protected bool defending;
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
    [SerializeField] private bool pierces = false;

    [Space(20)]
    [Header("Testing Variables")]
    [SerializeField] bool spawnTest = false;
    [SerializeField] bool startMoveTest = false; 

    protected float currentTime = 0;

    private void Awake()
    {
        if (spawnTest)
        {
            Debug.LogError("Spawn Test is active"); 
            SetSpawns(); 
        }

        if (startMoveTest)
        {
            Debug.LogError("Spawn Test is active");
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
        if (spawnPoints.Length == 0) return; //TEMP

        Player[] players = FindObjectsOfType<Player>();

        if (players.Length <= 1)
        {
            players[0].transform.position = spawnPoints[0].position;
        }
        else
        {
            //set each alive player to a different spawn
        }
    }

    public virtual void EndMove()
    {
        Debug.Log(Score);
        MoveEnded = true;

        if (CombatManager.Instance != null) //In the combat screen
        {
            foreach (Character character in CombatManager.Instance.GetCharactersSelected)
            {
                //handles if the outcome effects enemies and players differently
                if(playerEnemyTargetDifference)
                {
                    PlayerEnemyDifference(character);
                }
                else
                {
                    //Calculate Damage
                    if (Score < 0)
                        Score = 0;
                    if (Score >= maxScore)
                        Score = maxScore;

                    int currentDamage;
                    //defending/attacking
                    if (!defending)
                        currentDamage = Score * Damage;
                    else
                        currentDamage = maxScore * Damage - Score * Damage;

                    currentDamage += baseDamage;

                    //TODO CHANCE add array of augments to dish out in base combat
                    //Calculate Augment Stacks
                    int augmentStacks = AugmentScore * augmentStacksPerScore;
                    augmentStacks += baseAugmentStacks;
                    if (augmentStacks > maxAugmentStacks)
                        augmentStacks = maxAugmentStacks;

                    if (currentDamage > 0 && isDamaging)
                    {
                        //1 being base damage
                        float DamageAdj = 1;

                        //TODO CHANCE DAMAGE BUFF AUG (ALSO POTENCY AUG)
                        //Damage on players must be divided by 100 to multiply the final
                        if(CombatManager.Instance.GetCurrentPlayer != null)
                        {
                            DamageAdj = CombatManager.Instance.GetCurrentPlayer.Stats.Damage / 100;
                        }
                        else if(CombatManager.Instance.GetCurrentEnemy != null)
                        {
                            DamageAdj = CombatManager.Instance.GetCurrentEnemy.Stats.Damage / 100;
                        }

                        character.TakeDamage((int)(currentDamage * DamageAdj), pierces);
                    }

                    else if (currentDamage > 0 && isHealing)
                    {
                        //1 being base damage
                        float DamageAdj = 1;

                        //TODO CHANCE DAMAGE BUFF AUG (ALSO POTENCY AUG)
                        //Damage on players must be divided by 100 to multiply the final
                        if (CombatManager.Instance.GetCurrentPlayer != null)
                        {
                            DamageAdj = CombatManager.Instance.GetCurrentPlayer.Stats.Damage / 100;
                        }
                        else if (CombatManager.Instance.GetCurrentEnemy != null)
                        {
                            DamageAdj = CombatManager.Instance.GetCurrentEnemy.Stats.Damage / 100;
                        }

                        character.TakeHealing((int)(currentDamage * DamageAdj), pierces);
                    }

                    //Apply augment
                    character.AddDebuffStack(DebuffToAdd, augmentStacks);
                }
            }
        }
        else //Running tests
        {
            Debug.Log($"{Damage * AugmentScore} done to player");
            if (DebuffToAdd != null) Debug.Log($"{AugmentScore} stacks of {DebuffToAdd.DebuffName} added");
        }
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
}
