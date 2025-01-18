using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FullCourse : CombatMove
{
    [SerializeField] Transform[] foodSpawns;
    [SerializeField] float delayBetweenSpawns;
    [SerializeField] GameObject goodFood;
    [SerializeField] GameObject badFood;
    [SerializeField] int spawnBadAfterThisManyFood = 3;

    float currentSpawnTime = 0;
    int lastSpawnIndex = -1;
    int foodsSpawned;

    private void Start()
    {
        GenerateLayout();
    }
    public override void StartMove()
    {
        FullCoursePlatformMovement[] platforms = FindObjectsOfType<FullCoursePlatformMovement>();
        foreach (FullCoursePlatformMovement platform in platforms)
            platform.StartMove(); 

        base.StartMove();
    }

    protected override void Update()
    {
        if(trackTime)
        {
            currentSpawnTime += Time.deltaTime;

            if(currentSpawnTime > delayBetweenSpawns)
                SpawnFood();

            base.Update();
        }
    }

    private void SpawnFood()
    {
        currentSpawnTime = 0;

        int currentSpawnIndex = Random.Range(0, foodSpawns.Length);

        while(currentSpawnIndex == lastSpawnIndex)
            currentSpawnIndex = Random.Range(0, foodSpawns.Length);

        lastSpawnIndex = currentSpawnIndex;

        if (foodsSpawned > spawnBadAfterThisManyFood)
        {
            Instantiate(badFood, foodSpawns[currentSpawnIndex]);
            foodsSpawned = 0;
        }
        else
        {
            Instantiate(goodFood, foodSpawns[currentSpawnIndex]);
            foodsSpawned++;
        }
    }


    public override void EndMove()
    {
        if (FindObjectOfType<InaPractice>())
            return;

        MoveEnded = true;

        foreach (Character character in CombatManager.Instance.GetCharactersSelected)
        {
            //Calculate Healing
            if (Score < 0)
                Score = 0;
            if (Score >= maxScore)
                Score = maxScore;

            int currentHealing = 0;
            currentHealing = Score * Damage;

            currentHealing += baseDamage;

            //Calculate Augment Stacks
            int augmentStacks = 1; //always applies overfed to enemies

            //1 being base damage
            float HealingAdj = 1;

            //Damage on players must be divided by 100 to multiply the final
            HealingAdj = CombatManager.Instance.GetCurrentCharacter.Stats.Restoration / 100;
            float tempHealing = (float)currentHealing * HealingAdj + (float)CombatManager.Instance.GetCurrentCharacter.FlatHealingAdjustment;
            currentHealing = (int)tempHealing;

            //okay here it comes... Im boutta math
            if (character.CurrentHealth + character.AdjustAttackHealing(currentHealing) > character.Health)
            {
                int currentDamage = character.CurrentHealth + character.AdjustAttackHealing(currentHealing) - character.Health;
                currentHealing = character.Health - character.CurrentHealth;
                character.SingleHealThenDamage(currentHealing, currentDamage, true);
            }
            else
            {
                character.TakeHealing(currentHealing);
            }

            //Apply augment
            character.GetComponent<Character>().AddAugmentStack(DebuffToAdd, augmentStacks);
        }
    }

    public override string TrainingDisplayText => $"You scored {Score = (Score > maxScore ? maxScore : Score)}/{maxScore} healing all characters for {Score * Damage + baseDamage}. All characters received a stack of {DebuffToAdd.AugmentName}.";
}
