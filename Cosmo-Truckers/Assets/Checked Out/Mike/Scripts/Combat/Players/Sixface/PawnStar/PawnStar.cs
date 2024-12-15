using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PawnStar : CombatMove
{
    [SerializeField] GameObject projectile;
    [SerializeField] int oneScore;
    [SerializeField] int twoScore;

    SixFaceMana sixFaceMana;

    public override void StartMove()
    {
        projectile.SetActive(true);

        base.StartMove();
    }

    public override List<Character> NoTargetTargeting()
    {
        sixFaceMana = FindObjectOfType<SixFaceMana>();

        if (sixFaceMana.FaceType == SixFaceMana.FaceTypes.Dizzy)
        {
            return null;
        }
        else if (sixFaceMana.FaceType == SixFaceMana.FaceTypes.Money)
        {
            CombatManager.Instance.MyTargeting.CurrentTargetingType = EnumManager.TargetingType.Single_Target;
            CombatManager.Instance.MyTargeting.InitialSetup = false;
        }
        else
        {
            //Actual target condition
            Enemy highestSubductionEnemy = null;
            int highestSubduction = 0;

            foreach (Enemy enemy in EnemyManager.Instance.Enemies)
            {
                if (!enemy.Dead && highestSubductionEnemy == null)
                {
                    highestSubductionEnemy = enemy;
                    foreach (AugmentStackSO augment in enemy.GetAUGS)
                        if (augment.AugmentName == "Subduction")
                            highestSubduction = augment.CurrentStacks;
                }

                else if (!enemy.Dead)
                {
                    foreach(AugmentStackSO augment in enemy.GetAUGS)
                        if(augment.AugmentName == "Subduction" && augment.CurrentStacks > highestSubduction)
                        {
                            highestSubduction = augment.CurrentStacks;
                            highestSubductionEnemy = enemy;
                        }
                }
            }

            List<Character> target = new List<Character>();
            target.Add(highestSubductionEnemy);
            return target;
        }

        return null;
    }

    public override void EndMove()
    {
        if (FindObjectOfType<InaPractice>())
            return;

        sixFaceMana = FindObjectOfType<SixFaceMana>();

        //if Dizzy do the dizzy thing
        sixFaceMana = FindObjectOfType<SixFaceMana>();
        if (sixFaceMana.FaceType == SixFaceMana.FaceTypes.Dizzy)
        {
            List<Character> aliveEnemies = new List<Character>();
            foreach (Enemy enemy in EnemyManager.Instance.Enemies)
            {
                if (!enemy.Dead)
                    aliveEnemies.Add(enemy);
            }

            int random = Random.Range(0, aliveEnemies.Count);
            CombatManager.Instance.GetCharactersSelected.Add(aliveEnemies[random]);
        }

        MoveEnded = true;

        if (Score >= twoScore)
            Score = 2;
        else if (Score >= oneScore)
            Score = 1;

        //Calculate Damage
        if (Score < 0)
            Score = 0;

        AugmentScore = Score;

        int currentDamage = 0;
        //defending/attacking
        currentDamage = Score * Damage;
        currentDamage += baseDamage;

        //Calculate Augment Stacks
        int augmentStacks = CalculateAugmentStacks();

        //Apply augment
        if (sixFaceMana.FaceType == SixFaceMana.FaceTypes.Hype)
            augmentStacks++;
        
        if(sixFaceMana.FaceType != SixFaceMana.FaceTypes.Sad)
            CombatManager.Instance.GetCharactersSelected[0].AddAugmentStack(DebuffToAdd, augmentStacks);

        float stacksOfSubduction = 0;

        foreach(AugmentStackSO augment in CombatManager.Instance.GetCharactersSelected[0].GetAUGS)
            if (augment.AugmentName == "Subduction")
                stacksOfSubduction = augment.CurrentStacks;

        double tempDamage = currentDamage;
        tempDamage *= (1 + (stacksOfSubduction * .1));
        currentDamage = (int)tempDamage;

        //1 being base damage
        float DamageAdj = 1;

        //Damage on players must be divided by 100 to multiply the final
        DamageAdj = CombatManager.Instance.GetCurrentCharacter.Stats.Damage / 100;

        CombatManager.Instance.GetCharactersSelected[0].TakeDamage((int)(currentDamage * DamageAdj + CombatManager.Instance.GetCurrentCharacter.FlatDamageAdjustment), pierces);

        //Update face
        sixFaceMana.UpdateFace();
    }

    private int CalculateAugmentStacks()
    {
        int augmentStacks = AugmentScore * augmentStacksPerScore;
        augmentStacks += baseAugmentStacks;
        if (augmentStacks > maxAugmentStacks)
            return maxAugmentStacks;
        else
            return augmentStacks;
    }

    public override string TrainingDisplayText => $"You scored {Score = (Score > maxScore ? maxScore : Score)}/{maxScore} dealing {Score * Damage + baseDamage} damage. Your target gained {CalculateAugmentStacks()} stack(s) of {DebuffToAdd.AugmentName}.";
}
