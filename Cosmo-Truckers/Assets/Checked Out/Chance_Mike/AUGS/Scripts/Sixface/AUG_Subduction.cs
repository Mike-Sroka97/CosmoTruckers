using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AUG_Subduction : Augment
{
    public override void Activate(DebuffStackSO stack = null)
    {
        base.Activate(stack);
        DebuffSO.MyCharacter.AdjustDamage(-(int)StatusEffect);
    }

    public override void AdjustStatusEffect(int adjuster)
    {
        StopEffect();
        base.AdjustStatusEffect(adjuster);
        Activate(DebuffSO);
    }

    public override void StopEffect()
    {
        DebuffSO.MyCharacter.AdjustDamage((int)StatusEffect);
    }
}
