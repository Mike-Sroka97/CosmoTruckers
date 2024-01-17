using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LongDogCharacter : PlayerCharacter
{
    [SerializeField] DebuffStackSO hollowBones;

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
        if (Dead)
            return;

        List<int> damageList = new List<int>();

        for (int i = 0; i < numberOfHits; i++)
        {
            if (!defensePiercing)
                damage = AdjustAttackDamage(damage);

            int tempDamage = AdjustDamageHealingBasedOnBodyParts(damage, true);
            damageList.Add(tempDamage);

            if (passiveMove && passiveMove.GetPassiveType == EnemyPassiveBase.PassiveType.OnDamage)
                passiveMove.Activate(CurrentHealth);

            if(BubbleShielded)
            {
                AdjustBubbleShield();
            }
            else if (Shield > 0)
            {
                //calculate overrage damage
                int overageDamage = damage - Shield;

                Shield = Shield - damage <= 0 ? 0 : Shield - damage;

                if (overageDamage > 0)
                    CurrentHealth = -overageDamage;
            }
            else
            {
                CurrentHealth = -damage;
            }

            if (CurrentHealth <= 0)
            {
                CurrentHealth = 0;
                Die();
            }
            //See if any AUGS trigger on Damage (Spike shield)
            else
            {
                AugmentsToRemove.Clear();

                foreach (DebuffStackSO aug in AUGS)
                {
                    if (aug.OnDamage)
                    {
                        aug.GetAugment().Trigger();
                    }
                }

                foreach (DebuffStackSO augment in AugmentsToRemove)
                    AUGS.Remove(augment);
            }
        }

        StartCoroutine(MyVessel.GetComponent<LongDogVessel>().LongDogDamageHealingEffect(damageList));
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
        if (Dead)
            return;

        List<int> healingList = new List<int>();

        for (int i = 0; i < numberOfHeals; i++)
        {
            if (!ignoreVigor)
                healing = AdjustAttackHealing(healing);

            int tempHealing = AdjustDamageHealingBasedOnBodyParts(healing, false);
            healingList.Add(tempHealing);
            CurrentHealth = healing;
        }

        StartCoroutine(MyVessel.GetComponent<LongDogVessel>().LongDogDamageHealingEffect(healingList, false));
    }

    private int AdjustDamageHealingBasedOnBodyParts(int amount, bool damage)
    {
        if (BubbleShielded)
            return 0;

        int currentHealth = CurrentHealth;

        if (damage)
        {
            //Damage reducing LDG below 60 sets health to 60
            if(currentHealth > bodyAndLegHealth)
            {
                if(currentHealth - amount < bodyAndLegHealth)
                {
                    AddDebuffStack(hollowBones);
                    amount = currentHealth - bodyAndLegHealth;
                }
            }
            //Damage reducing LDG below 30 sets health to 30
            else if(currentHealth > legOnlyHealth)
            {
                if (currentHealth - amount < legOnlyHealth)
                {
                    AddDebuffStack(hollowBones);
                    amount = currentHealth - legOnlyHealth;
                }
            }
        }
        else
        {
            //Healing raising health above 60 sets health to 60
            if(currentHealth < bodyAndLegHealth)
            {
                if (currentHealth + amount > bodyAndLegHealth)
                {
                    AddDebuffStack(hollowBones);
                    amount = bodyAndLegHealth - currentHealth;
                }
            }
            //Healing raising health above 30 sets health to 30
            else if(currentHealth < legOnlyHealth)
            {
                if (currentHealth + amount > legOnlyHealth)
                {
                    AddDebuffStack(hollowBones);
                    amount = legOnlyHealth - currentHealth;
                }
            }
        }

        return amount;
    }
}
