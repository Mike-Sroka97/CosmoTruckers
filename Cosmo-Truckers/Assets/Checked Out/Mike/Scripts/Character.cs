using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class Character : MonoBehaviour
{
    [SerializeField] protected EnemyPassiveBase passiveMove;
    [SerializeField] protected DebuffStackSO[] passiveAugments;
    [SerializeField] protected List<DebuffStackSO> AUGS = new List<DebuffStackSO>();
    public List<DebuffStackSO> AugmentsToRemove = new List<DebuffStackSO>();
    [SerializeField] protected int maxShield = 60;
    private int shield = 0;
    public List<DebuffStackSO> GetAUGS { get => AUGS; }
    public CharacterStats Stats;
    public int Health;
    public SpriteRenderer[] TargetingSprites;
    [SerializeField] SpriteRenderer[] ShieldSprites;
    [SerializeField] Material shieldedMaterial;
    [SerializeField] Material bubbleShieldMaterial;
    public int CombatSpot;
    public int FlatDamageAdjustment = 0;
    public int FlatHealingAdjustment = 0;
    public UnityEvent HealthChangeEvent = new UnityEvent();
    public UnityEvent ShieldChangeEvent = new UnityEvent();
    public int CurrentHealth
    {
        get
        {
            return currentHealth;
        }
        set
        {
            HealthChangeEvent.Invoke();

            if (currentHealth + value > Health)
                currentHealth = Health;
            else if (currentHealth + value < 0)
                currentHealth = 0;
            else
                currentHealth += value;
        }
    }

    private int currentHealth;
    public int Shield
    {
        get
        {
            return shield;
        }
        set
        {
            ShieldChangeEvent.Invoke();

            if (shield + value > maxShield)
            {
                shield = maxShield;
                AdjustShieldMaterial(true);
            }
            else if (shield + value < 0)
            {
                shield = 0;
                AdjustShieldMaterial(false);
            }
            else
            {
                shield += value;
                AdjustShieldMaterial(true);
            }
        }
    }

    public bool BubbleShielded = false;

    public bool Dead;

    protected TurnOrder turnOrder;
    protected SpriteRenderer myRenderer;

    [SerializeField] int spaceTaken = 1;
    public int GetSpaceTaken { get => spaceTaken; }

    public virtual void TakeDamage(int damage, bool defensePiercing = false)
    {
        if (Dead)
            return;

        if (!defensePiercing)
            damage = AdjustAttackDamage(damage);

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
        else CurrentHealth = -damage;

        if (CurrentHealth <= 0)
        {
            currentHealth = 0;
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
            {
                AUGS.Remove(augment);
                Destroy(augment);
            }

        }
    }

    public virtual void TakeMultiHitDamage(int damage, int numberOfHits, bool defensePiercing = false)
    {
        if (Dead)
            return;

        for (int i = 0; i < numberOfHits; i++)
        {
            if (!defensePiercing)
                damage = AdjustAttackDamage(damage);

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
            else CurrentHealth = -damage;

            if (CurrentHealth <= 0)
            {
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
    }

    public virtual void TakeHealing(int healing, bool ignoreVigor = false)
    {
        if (Dead)
            return;

        if(!ignoreVigor)
            healing = AdjustAttackHealing(healing);

        CurrentHealth = healing;
    }

    public virtual void TakeMultiHitHealing(int healing, int numberOfHeals, bool ignoreVigor = false)
    {
        if (Dead)
            return;

        for (int i = 0; i < numberOfHeals; i++)
        {
            if (!ignoreVigor)
                healing = AdjustAttackHealing(healing);

            CurrentHealth = healing;
        }
    }

    public virtual void TakeShielding(int shieldAmount)
    {
        if (Dead)
            return;

        if (Shield + shieldAmount > maxShield)
            Shield = maxShield;
        else
            Shield += shieldAmount;
    }

    public void AdjustBubbleShield(bool active = false)
    {
        BubbleShielded = active;
        AdjustShieldMaterial(Shield > 0);
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

    private void AdjustShieldMaterial(bool shielded)
    {
        if(BubbleShielded)
        {
            foreach (SpriteRenderer renderer in ShieldSprites)
            {
                renderer.color = new Color(renderer.color.r, renderer.color.g, renderer.color.b, 1);
                renderer.material = bubbleShieldMaterial;
            }
        }
        else
        {
            if (shielded)
                foreach (SpriteRenderer renderer in ShieldSprites)
                {
                    renderer.color = new Color(renderer.color.r, renderer.color.g, renderer.color.b, 1);
                    renderer.material = shieldedMaterial;
                }
            else
                foreach (SpriteRenderer renderer in ShieldSprites)
                {
                    renderer.color = new Color(renderer.color.r, renderer.color.g, renderer.color.b, 0);
                    renderer.material = shieldedMaterial;
                }
        }
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

        //Remove AUGs
        AugmentsToRemove.Clear();

        foreach (DebuffStackSO aug in AUGS)
            if (aug.RemoveOnDeath)
                AugmentsToRemove.Add(aug);
        foreach (DebuffStackSO aug in AugmentsToRemove)
            AUGS.Remove(aug);
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
        if(!tempAUG.InCombat)
            tempAUG.SetTemp();
        tempAUG.MyCharacter = this;

        AUGS.Add(tempAUG);

        if (stack.StartUp || stack.StatChange || test)
            tempAUG.DebuffEffect();
    }

    public void RemoveDebuffStack(DebuffStackSO stack, int stackToRemove = 100)
    {
        if (stack == null || !stack.Removable)
            return;

        foreach (DebuffStackSO aug in AUGS)
        {
            if (String.Equals(aug.DebuffName, stack.DebuffName))
            {
                aug.CurrentStacks -= stackToRemove;
                aug.DestroyAugment();
                    //StartCoroutine(DelayedRemoval(aug)); TODO fuck it we ball. If this breaks the game we will fix it another way

                return;
            }
        }
    }

    /// <summary>
    /// type 0 = remove debuffs
    /// type 1 = remove buffs
    /// type 2 = remove any
    /// </summary>
    /// <param name="amount"></param>
    /// <param name="type"></param>
    public void RemoveAmountOfAugments(int amount, int type)
    {
        int currentAmount = amount;

        switch (type)
        {
            //remove debuffs
            case 0:
                foreach(DebuffStackSO debuff in AUGS)
                    if(debuff.IsDebuff)
                    {
                        if (currentAmount >= debuff.CurrentStacks)
                        {
                            RemoveDebuffStack(debuff, currentAmount);
                            debuff.DestroyAugment();
                            currentAmount -= debuff.CurrentStacks;
                        }
                        else if(currentAmount > 0)
                        {
                            RemoveDebuffStack(debuff, currentAmount);
                            currentAmount = 0;
                        }                        
                        else
                        {
                            break; //no more stacks to remove
                        }
                    }
                break;
            //remove buffs
            case 1:
                foreach (DebuffStackSO buff in AUGS)
                    if (buff.IsBuff)
                    {
                        if (currentAmount >= buff.CurrentStacks)
                        {
                            RemoveDebuffStack(buff, currentAmount);
                            buff.DestroyAugment();
                            currentAmount -= buff.CurrentStacks;
                        }
                        else if (currentAmount > 0)
                        {
                            RemoveDebuffStack(buff, currentAmount);
                            currentAmount = 0;
                        }
                        else
                        {
                            break; //no more stacks to remove
                        }
                    }
                break;
            //remove augments
            case 2:
                foreach (DebuffStackSO augment in AUGS)
                    if (currentAmount >= augment.CurrentStacks)
                    {
                        RemoveDebuffStack(augment, currentAmount);
                        augment.DestroyAugment();
                        currentAmount -= augment.CurrentStacks;
                    }
                    else if (currentAmount > 0)
                    {
                        RemoveDebuffStack(augment, currentAmount);
                        currentAmount = 0;
                    }
                    else
                    {
                        break; //no more stacks to remove
                    }
                break;
            default:
                break;
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
