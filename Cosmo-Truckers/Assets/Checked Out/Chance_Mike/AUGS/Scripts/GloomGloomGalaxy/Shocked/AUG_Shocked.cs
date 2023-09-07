using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AUG_Shocked : Augment
{
    public override void Activate(DebuffStackSO stack = null)
    {
        base.Activate(stack);
        DebuffSO.MyCharacter.AdjustSpeed(-(int)StatusEffect);
    }

    public override void AdjustStatusEffect(int adjuster)
    {
        StopEffect();
        base.AdjustStatusEffect(adjuster);
        Activate(DebuffSO);
    }

    public override void StopEffect()
    {
        DebuffSO.MyCharacter.AdjustSpeed((int)StatusEffect);
    }
}
