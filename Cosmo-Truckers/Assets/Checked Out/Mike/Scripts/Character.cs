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
    [HideInInspector] public int CurrentHealth;
    public bool Dead;

    protected TurnOrder turnOrder;

    public virtual void TakeDamage(int damage)
    {
        damage = AdjustDamage(damage);

        CurrentHealth -= damage;
        if (CurrentHealth <= 0)
        {
            Die();
        }
    }

    private int AdjustDamage(int damage)
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
    public void AddDebuffStack(DebuffStackSO stack)
    {
        foreach (DebuffStackSO aug in AUGS)
        {
            if (String.Equals(aug.DebuffName, stack.DebuffName))
            {
                if (aug.CurrentStacks < aug.MaxStacks)
                    aug.CurrentStacks++;

                return;
            }
        }

        DebuffStackSO tempAUG = Instantiate(stack);
        tempAUG.MyCharacter = this;

        AUGS.Add(tempAUG);

        if (stack.Type == DebuffStackSO.ActivateType.StartUp)
            stack.DebuffEffect();
    }

    public abstract void AdjustDefense(int defense);

    public void AdjustSpeed(int speed)
    {
        Stats.Speed += speed;

        if (Stats.Speed > 200)
            Stats.Speed = 200;
        else if (Stats.Speed < -40)
            Stats.Speed = -40;
    }

    public abstract void StartTurn();
    public abstract void EndTurn();
}
