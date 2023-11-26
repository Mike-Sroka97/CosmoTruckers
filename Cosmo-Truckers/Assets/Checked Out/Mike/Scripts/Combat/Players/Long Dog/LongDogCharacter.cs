using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LongDogCharacter : PlayerCharacter
{
    public bool ResetCombatPosition = false;

    const int legOnlyHealth = 30;
    const int bodyAndLegHealth = 60;

    public override void StartTurn()
    {
        if(ResetCombatPosition)
        {
            for(int i = 0; i < EnemyManager.Instance.PlayerCombatSpots.Length; i++)
            {
                if (EnemyManager.Instance.PlayerCombatSpots[i] != null && EnemyManager.Instance.PlayerCombatSpots[i] == this)
                    EnemyManager.Instance.PlayerCombatSpots[i] = null;
            }

            ResetCombatPosition = false;
            EnemyManager.Instance.PlayerCombatSpots[CombatSpot] = this;
        }

        base.StartTurn();
    }

    public override void TakeDamage(int damage, bool defensePiercing = false)
    {
        if (!defensePiercing)
            damage = AdjustAttackDamage(damage);

        damage = AdjustDamageHealingBasedOnBodyParts(damage, true);

        base.TakeDamage(damage, defensePiercing);
    }

    public override void TakeMultiHitDamage(int damage, int numberOfHits, bool defensePiercing = false)
    {
        //if (!defensePiercing)
        //    damage = AdjustAttackDamage(damage);

        //int tempDamage = damage * numberOfHits;
        //AdjustDamageHealingBasedOnBodyParts(tempDamage, true);

        base.TakeMultiHitDamage(damage, numberOfHits, defensePiercing);
    }

    public override void TakeHealing(int healing, bool ignoreVigor = false)
    {
        if (!ignoreVigor)
            healing = AdjustAttackHealing(healing);

        healing = AdjustDamageHealingBasedOnBodyParts(healing, false);
        base.TakeHealing(healing, ignoreVigor);
    }

    public override void TakeMultiHitHealing(int healing, int numberOfHeals, bool ignoreVigor = false)
    {
        //if (!ignoreVigor)
        //    healing = AdjustAttackHealing(healing);

        //int tempHealing = 

        AdjustDamageHealingBasedOnBodyParts(healing, false);
        base.TakeMultiHitHealing(healing, numberOfHeals, ignoreVigor);
    }

    private int AdjustDamageHealingBasedOnBodyParts(int amount, bool damage)
    {
        //TODO add 1 turn 0 vigor AUG if any of these trigger

        if (damage)
        {
            //Damage reducing LDG below 60 sets health to 60
            if(CurrentHealth > bodyAndLegHealth)
            {
                if(CurrentHealth - amount < bodyAndLegHealth)
                {
                    amount = CurrentHealth - bodyAndLegHealth;
                }
            }
            //Damage reducing LDG below 30 sets health to 30
            else if(CurrentHealth > legOnlyHealth)
            {
                if (CurrentHealth - amount < legOnlyHealth)
                {
                    amount = CurrentHealth - legOnlyHealth;
                }
            }
        }
        else
        {
            //Healing raising health above 60 sets health to 60
            if(CurrentHealth < bodyAndLegHealth)
            {
                if (CurrentHealth + amount > bodyAndLegHealth)
                {
                    amount = bodyAndLegHealth - CurrentHealth;
                }
            }
            //Healing raising health above 30 sets health to 30
            else if(CurrentHealth < legOnlyHealth)
            {
                if (CurrentHealth + amount > legOnlyHealth)
                {
                    amount = legOnlyHealth - CurrentHealth;
                }
            }
        }

        return amount;
    }
}
