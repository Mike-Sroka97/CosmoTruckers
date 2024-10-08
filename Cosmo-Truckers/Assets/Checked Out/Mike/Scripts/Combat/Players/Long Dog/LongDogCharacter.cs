using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements.Experimental;

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

        StartCoroutine(MyVessel.GetComponent<LongDogVessel>().LongDogDamageHealingEffect(new List<int>(){damage}, EnumManager.CombatOutcome.Damage));
    }

    /// <summary>
    /// This will get the list of damage to pass on to the DamageHealingEffect
    /// </summary>
    /// <param name="healing"></param>
    /// <param name="numberOfHeals"></param>
    /// <param name="ignoreVigor"></param>
    public override void TakeMultiHitDamage(int damage, int numberOfHits, bool defensePiercing = false)
    {
        if (Dead)
            return;

        List<int> damageList = AdjustDamageHealingBasedOnBodyParts(damage, numberOfHits, damage: true, defensePiercing); 

        StartCoroutine(MyVessel.GetComponent<LongDogVessel>().LongDogDamageHealingEffect(damageList, EnumManager.CombatOutcome.MultiDamage));
    }

    /// <summary>
    /// This is what will ACTUALLY deal multi hit damage to Long Dog
    /// </summary>
    /// <param name="damageList"></param>
    /// <param name="defensePiercing"></param>
    public void LongDogTakeMultiHitDamage(List<int> damageList, bool defensePiercing = false)
    {
        if (Dead)
            return;

        for (int i = 0; i < damageList.Count; i++)
        {
            if (passiveMove && passiveMove.GetPassiveType == EnemyPassiveBase.PassiveType.OnDamage)
                passiveMove.Activate(CurrentHealth);

            if (BubbleShielded)
            {
                AdjustBubbleShield();
            }
            else if (Shield > 0)
            {
                //calculate overrage damage
                int overageDamage = damageList[i] - Shield;

                Shield = Shield - damageList[i] <= 0 ? 0 : Shield - damageList[i];

                if (overageDamage > 0)
                    CurrentHealth = -overageDamage;
            }
            else
            {
                CurrentHealth = -damageList[i];
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
                    AdjustAugs(false, augment);
            }
        }

        // After damage is done, subtract Command Executing
        CombatManager.Instance.CommandsExecuting--;
    }

    public override void TakeHealing(int healing, bool ignoreVigor = false)
    {
        if (!ignoreVigor)
            healing = AdjustAttackHealing(healing);

        healing = AdjustDamageHealingBasedOnBodyParts(healing, false);

        StartCoroutine(MyVessel.GetComponent<LongDogVessel>().LongDogDamageHealingEffect(new List<int>() { healing }, EnumManager.CombatOutcome.Healing, false));
    }

    /// <summary>
    /// This will get the list of healing to pass on to the DamageHealingEffect
    /// </summary>
    /// <param name="healing"></param>
    /// <param name="numberOfHeals"></param>
    /// <param name="ignoreVigor"></param>
    public override void TakeMultiHitHealing(int healing, int numberOfHeals, bool ignoreVigor = false)
    {
        if (Dead)
            return;

        List<int> healingList = AdjustDamageHealingBasedOnBodyParts(healing, numberOfHeals, damage: false, ignoreVigor); 

        StartCoroutine(MyVessel.GetComponent<LongDogVessel>().LongDogDamageHealingEffect(healingList, EnumManager.CombatOutcome.MultiHealing, false));
    }

    /// <summary>
    /// This is what will ACTUALLY deal multi hit healing to Long Dog
    /// </summary>
    /// <param name="healingList"></param>
    /// <param name="ignoreVigor"></param>
    public void LongDogTakeMultiHitHealing(List<int> healingList, bool ignoreVigor = false)
    {
        if (Dead)
            return;

        for (int i = 0; i < healingList.Count; i++)
        {
            if (!ignoreVigor)
                healingList[i] = AdjustAttackHealing(healingList[i]);

            int tempHealing = AdjustDamageHealingBasedOnBodyParts(healingList[i], false);
            CurrentHealth = tempHealing;
        }

        // After damage is done, subtract Command Executing
        CombatManager.Instance.CommandsExecuting--;
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

    /// <summary>
    /// Get the list of adjusted damage or healing to use when dispalying damage or healing stars
    /// </summary>
    /// <param name="amount"></param>
    /// <param name="numberOfHits"></param>
    /// <param name="damage"></param>
    /// <param name="piercing"></param>
    /// <returns></returns>
    private List<int> AdjustDamageHealingBasedOnBodyParts(int amount, int numberOfHits, bool damage, bool piercing = false)
    {
        List<int> adjustedValues = new List<int>();
        int currentHealth = CurrentHealth;
        int currentValue = amount; 

        for (int i = 0; i < numberOfHits; i++)
        {
            int temporaryAmount = amount;

            if (damage)
            {
                if (!piercing)
                    temporaryAmount = AdjustAttackDamage(amount);

                //Damage reducing LDG below 60 sets health to 60
                if (currentHealth > bodyAndLegHealth)
                {
                    if (currentHealth - temporaryAmount < bodyAndLegHealth)
                    {
                        currentValue = currentHealth - bodyAndLegHealth;
                    }

                    currentHealth -= currentValue; 
                }
                //Damage reducing LDG below 30 sets health to 30
                else if (currentHealth > legOnlyHealth)
                {
                    if (currentHealth - temporaryAmount < legOnlyHealth)
                    {
                        currentValue = currentHealth - legOnlyHealth;
                    }

                    currentHealth -= currentValue; 
                }

                adjustedValues.Add(currentValue);
            }
            else
            {
                if (!piercing)
                    temporaryAmount = AdjustAttackHealing(amount);

                //Healing raising health above 60 sets health to 60
                if (currentHealth < bodyAndLegHealth)
                {
                    if (currentHealth + temporaryAmount > bodyAndLegHealth)
                    {
                        currentValue = bodyAndLegHealth - currentHealth;
                    }

                    currentHealth += currentValue; 
                }
                //Healing raising health above 30 sets health to 30
                else if (currentHealth < legOnlyHealth)
                {
                    if (currentHealth + temporaryAmount > legOnlyHealth)
                    {
                        currentValue = legOnlyHealth - currentHealth;
                    }

                    currentHealth += currentValue; 
                }

                adjustedValues.Add(currentValue);
            }
        }

        return adjustedValues; 
    }
}
