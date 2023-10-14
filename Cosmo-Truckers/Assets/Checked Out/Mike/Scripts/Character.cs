using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    [SerializeField] protected EnemyPassiveBase passiveMove;
    [SerializeField] protected List<DebuffStackSO> AUGS = new List<DebuffStackSO>();
    public List<DebuffStackSO> GetAUGS { get => AUGS; }
    public CharacterStats Stats;
    public int Health;
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
            else
                currentHealth += value;
        }
    }
    private int currentHealth;
    [HideInInspector] public List<int> CombatSpot;
    public int Shield;

    public bool Dead;

    protected TurnOrder turnOrder;
    protected SpriteRenderer myRenderer;

    [SerializeField] int spaceTaken = 1;
    public int GetSpaceTaken { get => spaceTaken; }

    public virtual void TakeDamage(int damage, bool defensePiercing = false)
    {
        if(!defensePiercing)
            damage = AdjustAttackDamage(damage);

        if (passiveMove && passiveMove.GetPassiveType == EnemyPassiveBase.PassiveType.OnDamage)
            passiveMove.Activate(CurrentHealth);

        if (Shield > 0) Shield = Shield - damage <= 0 ? 0 : Shield - damage;
        else CurrentHealth -= damage;

        if (CurrentHealth <= 0)
        {
            Die();
        }
        //See if any AUGS trigger on Damage (Spike shield)
        else
        {
            foreach(DebuffStackSO aug in AUGS)
            {
                if(aug.OnDamage)
                {
                    aug.GetAugment().Trigger();
                }
            }
        }
    }

    private int AdjustAttackDamage(int damage)
    {
        int newDamage = damage;
        
        //No adjustment if defense is zero
        if(Stats.Defense > 0)
        {
            float floatDamage = damage - (damage * Stats.Defense / 100);
            newDamage = (int)Math.Floor(floatDamage);
        }
        else if(Stats.Defense < 0)
        {
            float floatDamage = damage + (damage * Mathf.Abs(Stats.Defense) / 100);
            newDamage = (int)Math.Ceiling(floatDamage);
        }

        return newDamage;
    }

    public virtual void Die()
    {
        Dead = true;
        GetComponent<CharacterStats>().enabled = false;
        myRenderer.enabled = false;
        turnOrder.RemoveFromSpeedList(GetComponent<CharacterStats>());
        turnOrder.DetermineTurnOrder();
    }

    public virtual void Resurrect(int newHealth)
    {
        CurrentHealth = newHealth;
        GetComponent<CharacterStats>().enabled = true;
        turnOrder.AddToSpeedList(GetComponent<CharacterStats>());
        turnOrder.DetermineTurnOrder();
    }
    public void AddDebuffStack(DebuffStackSO stack, int stacksToAdd = 0, bool test = false)
    {
        foreach (DebuffStackSO aug in AUGS)
        {
            if (String.Equals(aug.DebuffName, stack.DebuffName))
            {
                if (aug.CurrentStacks < aug.MaxStacks)
                    aug.CurrentStacks += stacksToAdd;

                return;
            }
        }

        if (stack == null)
            return;

        DebuffStackSO tempAUG = Instantiate(stack);
        tempAUG.CurrentStacks = stacksToAdd;
        tempAUG.MyCharacter = this;

        AUGS.Add(tempAUG);

        if (stack.StartUp || test)
            tempAUG.DebuffEffect();
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
        else if(Stats.Vigor < 0)
            Stats.Vigor = 0;
    }

    public void AdjustDamage(int damage)
    {
        Stats.Damage = damage;

        //max x3 damage min 60% damage
        if (Stats.Damage > 300)
            Stats.Damage = 300;
        else if (Stats.Damage < 60)
            Stats.Damage = 60;
    }

    public abstract void StartTurn();
    public abstract void EndTurn();


    [SerializeField] DebuffStackSO test;

    [ContextMenu("Test AUG")]
    public void TestAUG()
    {
        //AddDebuffStack(test, true);
    }

    protected void FadeAugments()
    {
        foreach (DebuffStackSO augment in AUGS)
            if (augment.FadingAugment)
                augment.Fade();
    }
}
