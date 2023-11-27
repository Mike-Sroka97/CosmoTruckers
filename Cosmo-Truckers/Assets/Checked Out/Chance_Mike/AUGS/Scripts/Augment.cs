using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Augment : MonoBehaviour
{
    [SerializeField] protected float baseStatusEffect;
    [SerializeField] protected float additionalStatusEffect;
    [HideInInspector] public DebuffStackSO DebuffSO;
    public int Stacks;
    protected float StatusEffect;
    protected float MaxStatusEffect;

    //LifeSpan

    /// <summary>
    /// Called when the debuff is started
    /// </summary>
    /// <param name="stack"></param>
    public virtual void Activate(DebuffStackSO stack = null) //Just in case we test without the SO
    {
        if (stack)
            InitializeAugment(stack);

        if (Stacks == 1)
        {
            StatusEffect = baseStatusEffect;
        }
        else if (Stacks > 1)
        {
            StatusEffect = baseStatusEffect;
            for (int i = 0; i < Stacks - 1; i++)
            {
                StatusEffect += additionalStatusEffect;
            }
        }
        else
        {
            StatusEffect = 0;
        }

        AdjustMaxStatusEffect();
    }

    public void InitializeAugment(DebuffStackSO stack)
    {
        DebuffSO = stack;
        DebuffSO.MyCharacter = stack.MyCharacter;
        baseStatusEffect = stack.StackValue.x;
        additionalStatusEffect = stack.StackValue.y;
        Stacks = stack.CurrentStacks;
        MaxStatusEffect = stack.StackValue.x + stack.StackValue.y * (stack.MaxStacks - 1);
    }

    protected void AdjustMaxStatusEffect()
    {
        if (StatusEffect > MaxStatusEffect)
            StatusEffect = MaxStatusEffect;
    }

    /// <summary>
    /// Called if the aug has abilitys that can be triggered after instansiation
    /// </summary>
    public virtual void Trigger() { }

    public virtual void AdjustStatusEffect(int adjuster)
    {
        Stacks += adjuster;
    }

    /// <summary>
    /// Stops the effect of the aug
    /// </summary>
    public abstract void StopEffect();
}
