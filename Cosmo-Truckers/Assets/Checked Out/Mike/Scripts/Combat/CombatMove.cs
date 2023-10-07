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
    [SerializeField] private bool playerEnemyTargetDifference = false;
    public float MinigameDuration;

    public int Score;
    [HideInInspector] public bool PlayerDead = false;
    [HideInInspector] public bool MoveEnded = false;
    public int Hits = 0;
    [Space(20)]
    [Header("Minigame Variables")]
    [SerializeField] int maxScore;
    [SerializeField] int augmentStacksPerHit;
    [SerializeField] int maxAugmentStacks;
    [SerializeField] int baseDamage;
    [SerializeField] int baseAugmentStacks;

    protected float currentTime = 0;

    private void Start()
    {
        StartMove();
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

    protected void StartMove()
    {

    }

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
                    int currentDamage = maxScore * Damage - Score * Damage;
                    currentDamage += baseDamage;

                    //Calculate Augment Stacks
                    int augmentStacks = Hits * augmentStacksPerHit;
                    augmentStacks += baseAugmentStacks;
                    if (augmentStacks > maxAugmentStacks)
                        augmentStacks = maxAugmentStacks;

                    character.GetComponent<Character>().TakeDamage(currentDamage);

                    //Apply augment
                    character.GetComponent<Character>().AddDebuffStack(DebuffToAdd, augmentStacks);
                }
            }
        }
        else //Running tests
        {
            Debug.Log($"{Damage * Hits} done to player");
            if (DebuffToAdd != null) Debug.Log($"{Hits} stacks of {DebuffToAdd.DebuffName} added");
        }
    }

    private void PlayerEnemyDifference(Character character)
    {
        if(character.GetComponent<Enemy>())
        {

        }
        else if(character.GetComponent<PlayerCharacter>())
        {

        }
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

        if ((currentTime >= MinigameDuration || PlayerDead) && !MoveEnded)
        {
            EndMove();
        }
    }
}
