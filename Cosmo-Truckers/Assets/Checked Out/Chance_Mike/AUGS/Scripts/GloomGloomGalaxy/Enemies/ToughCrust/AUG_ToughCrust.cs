using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AUG_ToughCrust : Augment
{
    public override void Activate(DebuffStackSO stack = null)
    {
        if (AugmentSO != null && AugmentSO.LastStacks != -1 && !firstGo)
            StopEffect();

        base.Activate(stack);
        AugmentSO.MyCharacter.AdjustDefense((int)StatusEffect);
        AugmentSO.LastStacks = AugmentSO.CurrentStacks;
        firstGo = false;
    }

    public override void StopEffect()
    {
        AugmentSO.MyCharacter.AdjustDefense(-(int)StatusEffect);
    }
}
