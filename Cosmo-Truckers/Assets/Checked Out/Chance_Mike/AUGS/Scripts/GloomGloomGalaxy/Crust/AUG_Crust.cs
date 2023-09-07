using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AUG_Crust : Augment
{
    public override void Activate(DebuffStackSO stack = null)
    {
        base.Activate(stack);
        DebuffSO.MyCharacter.AdjustDefense((int)StatusEffect);
    }

    public override void AdjustStatusEffect(int adjuster)
    {
        StopEffect();
        base.AdjustStatusEffect(adjuster);
        Activate(DebuffSO);
    }

    public override void StopEffect()
    {
        DebuffSO.MyCharacter.AdjustDefense(-(int)StatusEffect);
    }
}
