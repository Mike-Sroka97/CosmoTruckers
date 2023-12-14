using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AUG_Crust : Augment
{
    public override void Activate(DebuffStackSO stack = null)
    {
        if (AugmentSO != null && AugmentSO.LastStacks != -1)
            StopEffect();

        base.Activate(stack);
        AugmentSO.MyCharacter.AdjustDefense((int)StatusEffect);
    }

    public override void StopEffect()
    {
        AugmentSO.MyCharacter.AdjustDefense(-(int)StatusEffect);
    }
}
