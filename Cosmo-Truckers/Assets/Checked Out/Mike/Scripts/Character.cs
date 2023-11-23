using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    [SerializeField] protected EnemyPassiveBase passiveMove;
    [SerializeField] protected List<DebuffStackSO> AUGS = new List<DebuffStackSO>();
    public List<DebuffStackSO> AugmentsToRemove = new List<DebuffStackSO>();
    [SerializeField] protected int maxShield = 60;
    public List<DebuffStackSO> GetAUGS { get => AUGS; }
    public CharacterStats Stats;
    public int Health;
    public SpriteRenderer[] TargetingSprites;
    public int CombatSpot;
    public int FlatDamageAdjustment = 0;
    public int FlatHealingAdjustment = 0;
    [HideInInspector] public int CurrentHealth
    {
        get
        {
            return currentHealth;
        }
        set
        {
            if (currentHealth + value > Health)
                currentHealth = Health;
            else if (currentHealth + value < 0)
                currentHealth = 0;
            else
                currentHealth += value;
        }
    }

    private int currentHealth;
    public int Shield;

    public bool Dead;

    protected TurnOrder turnOrder;
    protected SpriteRenderer myRenderer;

    [SerializeField] int spaceTaken = 1;
    public int GetSpaceTaken { get => spaceTaken; }

    public virtual void TakeDamage(int damage, bool defensePiercing = false)
    {
        if (!defensePiercing)
            damage = AdjustAttackDamage(damage);

        if (passiveMove && passiveMove.GetPassiveType == EnemyPassiveBase.PassiveType.OnDamage)
            passiveMove.Activate(CurrentHealth);

        if (Shield > 0)
        {
            //calculate overrage damage
            int overageDamage = damage - Shield;

            Shield = Shield - damage <= 0 ? 0 : Shield - damage;

            if (overageDamage > 0)
                currentHealth -= overageDamage;
        }
        else currentHealth -= damage;

        if (CurrentHealth <= 0)
        {
            currentHealth = 0;
            Die();
        }
        //See if any AUGS trigger on Damage (Spike shield)
        else
        {
            foreach (DebuffStackSO aug in AUGS)
            {
                if (aug.OnDamage)
                {
                    aug.GetAugment().Trigger();
                }
            }
        }
    }

    public virtual void TakeMultiHitDamage(int damage, int numberOfHits, bool defensePiercing = false)
    {
        for (int i = 0; i < numberOfHits; i++)
        {
            if (!defensePiercing)
                damage = AdjustAttackDamage(damage);

            if (passiveMove && passiveMove.GetPassiveType == EnemyPassiveBase.PassiveType.OnDamage)
                passiveMove.Activate(CurrentHealth);

            if (Shield > 0)
            {
                //calculate overrage damage
                int overageDamage = damage - Shield;

                Shield = Shield - damage <= 0 ? 0 : Shield - damage;

                if (overageDamage > 0)
                    currentHealth -= overageDamage;
            }
            else currentHealth -= damage;

            if (CurrentHealth <= 0)
            {
                Die();
            }
            //See if any AUGS trigger on Damage (Spike shield)
            else
            {
                foreach (DebuffStackSO aug in AUGS)
                {
                    if (aug.OnDamage)
                    {
                        aug.GetAugment().Trigger();
                    }
                }
            }
        }
    }

    public virtual void TakeHealing(int healing, bool ignoreVigor = false)
    {
        if(!ignoreVigor)
            healing = AdjustAttackHealing(healing);

        CurrentHealth += healing;
    }

    public virtual void TakeMultiHitHealing(int healing, int numberOfHeals, bool ignoreVigor = false)
    {
        for (int i = 0; i < numberOfHeals; i++)
        {
            if (!ignoreVigor)
                healing = AdjustAttackHealing(healing);
        }
    }

    public virtual void TakeShielding(int shieldAmount)
    {
        if (Shield + shieldAmount > maxShield)
            Shield = maxShield;
        else
            Shield += shieldAmount;
    }

    protected int AdjustAttackDamage(int damage)
    {
        int newDamage = damage;

        //No adjustment if defense is zero
        if (Stats.Defense > 0)
        {
            float floatDamage = damage - (damage * Stats.Defense / 100);
            newDamage = (int)Math.Floor(floatDamage);
        }
        else if (Stats.Defense < 0)
        {
            float floatDamage = damage + (damage * Mathf.Abs(Stats.Defense) / 100);
            newDamage = (int)Math.Ceiling(floatDamage);
        }

        return newDamage;
    }

    public int AdjustAttackHealing(int healing)
    {
        int newHealing = healing;

        //No adjustment if vigor is zero
        float floatHealing = healing * Stats.Vigor / 100;
        newHealing = (int)Math.Floor(floatHealing);

        return newHealing;
    }

    public virtual void Die()
    {
        //play death animation
        Dead = true;
        GetComponent<CharacterStats>().enabled = false;
        foreach (SpriteRenderer renderer in TargetingSprites)
            renderer.enabled = false;
        turnOrder.RemoveFromSpeedList(GetComponent<CharacterStats>());
        turnOrder.DetermineTurnOrder();
    }

    public virtual void Resurrect(int newHealth, bool ignoreVigor = false)
    {
        if (!ignoreVigor)
            newHealth = AdjustAttackHealing(newHealth);

        if (newHealth >= Health)
            newHealth = Health;

        CurrentHealth = newHealth;
        Dead = false;
        GetComponent<CharacterStats>().enabled = true;
        foreach (SpriteRenderer renderer in TargetingSprites)
            renderer.enabled = true;
        turnOrder.AddToSpeedList(GetComponent<CharacterStats>());
        turnOrder.DetermineTurnOrder();
    }
    public void AddDebuffStack(DebuffStackSO stack, int stacksToAdd = 1, bool test = false)
    {
        if (stacksToAdd == 0) return;

        foreach (DebuffStackSO aug in AUGS)
        {
            if (string.Equals(aug.DebuffName, stack.DebuffName))
            {
                if (aug.CurrentStacks < aug.MaxStacks)
                    aug.CurrentStacks += stacksToAdd;
                if (stack.StartUp || stack.StatChange || test)
                    aug.DebuffEffect();
                return;
            }
        }

        if (stack == null)
            return;

        DebuffStackSO tempAUG = Instantiate(stack);
        tempAUG.CurrentStacks = stacksToAdd;
        tempAUG.MyCharacter = this;

        AUGS.Add(tempAUG);

        if (stack.StartUp || stack.StatChange || test)
            tempAUG.DebuffEffect();
    }

    public void RemoveDebuffStack(DebuffStackSO stack, int stackToRemove = 100)
    {
        if (stack == null)
            return;

        foreach (DebuffStackSO aug in AUGS)
        {
            if (String.Equals(aug.DebuffName, stack.DebuffName))
            {
                aug.CurrentStacks -= stackToRemove;

                if (aug.CurrentStacks <= 0)
                    aug.StopEffect();
                    StartCoroutine(DelayedRemoval(aug));

                return;
            }
        }
    }
    //TODO
    //Removing on damage augs will cause errors, for now this is the solution
    //Will need to add a check every turn to see if AUG has 0 stacks and then remove them then
    IEnumerator DelayedRemoval(DebuffStackSO aug)
    {
        yield return new WaitForSeconds(.5f);
        AUGS.Remove(aug);
    }

    public abstract void AdjustDefense(int defense);

    public void AdjustSpeed(int speed)
    {
        Stats.Speed += speed;

        //max double speed and min 40% reduction
        if (Stats.Speed > 200)
            Stats.Speed = 200;
        else if (Stats.Speed < -40)
            Stats.Speed = -40;
    }

    public void AdjustVigor(int vigor)
    {
        Stats.Vigor += vigor;

        //max double vigor and min 0% healing
        if (Stats.Vigor > 200)
            Stats.Vigor = 200;
        else if (Stats.Vigor < 0)
            Stats.Vigor = 0;
    }

    public void AdjustDamage(int damage)
    {
        Stats.Damage += damage;

        //max x3 damage min 60% damage
        if (Stats.Damage > 300)
            Stats.Damage = 300;
        else if (Stats.Damage < 60)
            Stats.Damage = 60;
    }

    public void AdjustRestoration(int restoration)
    {
        Stats.Restoration += restoration;

        //max double healing and min 40%
        if (Stats.Restoration > 200)
            Stats.Damage = 200;
        else if (Stats.Restoration < 40)
            Stats.Restoration = 40;
    }

    public abstract void StartTurn();
    public abstract void EndTurn();

    [Space(10)]
    [Header("Test AUG")]
    [SerializeField] DebuffStackSO test;

    [ContextMenu("Test AUG")]
    public void TestAUG()
    {
        AddDebuffStack(test, 1, true);
    }

    public void FadeAugments()
    {
        AugmentsToRemove.Clear();

        foreach (DebuffStackSO augment in AUGS)
            augment.Fade();
        foreach (DebuffStackSO augment in AugmentsToRemove)
            AUGS.Remove(augment);
    }
}
