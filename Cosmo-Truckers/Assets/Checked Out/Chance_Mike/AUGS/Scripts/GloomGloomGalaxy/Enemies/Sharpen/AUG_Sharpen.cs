using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AUG_Sharpen : Augment
{
    public override void Activate(AugmentStackSO stack = null)
    {
        base.Activate(stack);
        AugmentSO.MyCharacter.AdjustDamage((int)StatusEffect);
    }

    public override void AdjustStatusEffect(int adjuster)
    {
        StopEffect();
        base.AdjustStatusEffect(adjuster);
        Activate(AugmentSO);
    }

    public override void StopEffect()
    {
        AugmentSO.MyCharacter.AdjustDamage(-(int)StatusEffect);
    }
}
