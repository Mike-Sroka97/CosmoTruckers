using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Augment : MonoBehaviour
{
    [SerializeField] protected float baseStatusEffect;
    [SerializeField] protected float additionalStatusEffect;
    [HideInInspector] public AugmentStackSO AugmentSO;
    protected float StatusEffect;
    protected float MaxStatusEffect;
    protected bool firstGo = true;
    private bool initialized = false;

    //LifeSpan

    /// <summary>
    /// Called when the debuff is started
    /// </summary>
    /// <param name="stack"></param>
    public virtual void Activate(AugmentStackSO stack = null) //Just in case we test without the SO
    {
        if (stack)
            InitializeAugment(stack);

        if (AugmentSO.CurrentStacks == 1)
        {
            StatusEffect = baseStatusEffect;
        }
        else if (AugmentSO.CurrentStacks > 1)
        {
            StatusEffect = baseStatusEffect;
            for (int i = 0; i < AugmentSO.CurrentStacks - 1; i++)
            {
                StatusEffect += additionalStatusEffect;
            }
        }
        else
        {
            StatusEffect = 0;
        }

        firstGo = false;

        AdjustMaxStatusEffect();
    }

    public void InitializeAugment(AugmentStackSO stack)
    {
        if (initialized)
            return;

        AugmentSO = stack;
        AugmentSO.MyCharacter = stack.MyCharacter;
        baseStatusEffect = stack.StackValue.x;
        additionalStatusEffect = stack.StackValue.y;
        MaxStatusEffect = stack.StackValue.x + stack.StackValue.y * (stack.MaxStacks - 1);
        if (AugmentSO.LastStacks == -1)
            AugmentSO.LastStacks = AugmentSO.CurrentStacks;
        initialized = true;
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
        AugmentSO.CurrentStacks += adjuster;
    }

    /// <summary>
    /// Stops the effect of the aug
    /// </summary>
    public abstract void StopEffect();
}
