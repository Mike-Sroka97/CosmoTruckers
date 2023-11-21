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

            //TODO CHANCE make this check for the highest subduction alive enemy
            foreach (Enemy enemy in EnemyManager.Instance.Enemies)
            {
                if (!enemy.Dead && highestSubductionEnemy == null)
                    highestSubductionEnemy = enemy;
                //else if (!enemy.Dead && /*FIND NUMBER OF STACKS OF SUBDUCTION FOR highestSubductionEnemy AND enemy then compare*/)
                //    highestSubductionEnemy = enemy;
            }
            //END TODO

            List<Character> target = new List<Character>();
            target.Add(highestSubductionEnemy);
            return target;
        }

        return null;
    }

    public override void EndMove()
    {
        MoveEnded = true;

        if (Score >= twoScore)
            Score = 2;
        else if (Score >= oneScore)
            Score = 1;

        //Calculate Damage
        if (Score < 0)
            Score = 0;
        if (Score >= maxScore)
            Score = maxScore;

        int currentDamage = 0;
        //defending/attacking
        if (!defending)
            currentDamage = Score * Damage;
        else
            currentDamage = maxScore * Damage - Score * Damage;

        currentDamage += baseDamage;

        //TODO CHANCE ADD line
        //currentDamage *= 1 + (CombatManager.Instance.GetCharactersSelected[0].stacksOfSubduction * .1);

        //TODO CHANCE add array of augments to dish out in base combat
        //Calculate Augment Stacks
        int augmentStacks = AugmentScore * augmentStacksPerScore;
        augmentStacks += baseAugmentStacks;
        if (augmentStacks > maxAugmentStacks)
            augmentStacks = maxAugmentStacks;

        //1 being base damage
        float DamageAdj = 1;

        //TODO CHANCE DAMAGE BUFF AUG (ALSO POTENCY AUG)
        //Damage on players must be divided by 100 to multiply the final
        DamageAdj = CombatManager.Instance.GetCurrentCharacter.Stats.Damage / 100;

        CombatManager.Instance.GetCharactersSelected[0].TakeDamage((int)(currentDamage * DamageAdj + CombatManager.Instance.GetCurrentCharacter.FlatDamageAdjustment), pierces);

        //Apply augment
        CombatManager.Instance.GetCharactersSelected[0].AddDebuffStack(DebuffToAdd, augmentStacks);

        //Update face
        FindObjectOfType<SixFaceMana>().UpdateFace();
    }
}
