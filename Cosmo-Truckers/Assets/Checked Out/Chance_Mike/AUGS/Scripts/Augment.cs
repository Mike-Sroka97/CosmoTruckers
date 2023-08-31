using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Augment : MonoBehaviour
{
    [SerializeField] float baseStatusEffect;
    [SerializeField] float additionalStatusEffect;
    public DebuffStackSO DebuffSO { get; private set; }
    protected int Stacks;
    protected float StatusEffect;

    //LifeSpan

    public virtual void Activate(DebuffStackSO stack = null) //Just in case we test without the SO
    {
        if(stack)
        {
            stack= DebuffSO;
            baseStatusEffect = stack.StackValue.x;
            additionalStatusEffect = stack.StackValue.y;
            Stacks = stack.CurrentStacks;
        }

        if(Stacks == 1)
        {
            StatusEffect = baseStatusEffect;
        }
        else if(Stacks > 1)
        {
            StatusEffect = baseStatusEffect;
            for(int i = 0; i < Stacks - 1; i++)
            {
                StatusEffect += additionalStatusEffect;
            }
        }
        else
        {
            StatusEffect = 0;
        }
    }

    public virtual void AdjustStatusEffect(int adjuster)
    {
        Stacks += adjuster;
    }

    public abstract void StopEffect();
}
