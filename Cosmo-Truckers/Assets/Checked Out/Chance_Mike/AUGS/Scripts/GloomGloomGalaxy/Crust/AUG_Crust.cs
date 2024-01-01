using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AUG_Crust : Augment
{
    bool firstRun = true;
    public override void Activate(DebuffStackSO stack = null)
    {
        if (AugmentSO != null && AugmentSO.LastStacks != -1 && !firstRun)
            StopEffect();

        base.Activate(stack);
        AugmentSO.MyCharacter.AdjustDefense((int)StatusEffect);
        AugmentSO.LastStacks = AugmentSO.CurrentStacks;
        firstRun = false;
    }

    public override void StopEffect()
    {
        AugmentSO.MyCharacter.AdjustDefense(-(int)StatusEffect);
    }
}
