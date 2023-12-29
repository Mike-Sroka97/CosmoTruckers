using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AUG_Subduction : Augment
{
    bool firstRun = true;
    public override void Activate(DebuffStackSO stack = null)
    {
        if (AugmentSO != null && AugmentSO.LastStacks != -1 && !firstRun)
            StopEffect();

        base.Activate(stack);
        AugmentSO.MyCharacter.AdjustDamage(-(int)StatusEffect);
        AugmentSO.LastStacks = AugmentSO.CurrentStacks;
        firstRun = false;
    }

    public override void StopEffect()
    {
        AugmentSO.MyCharacter.AdjustDamage((int)StatusEffect);
    }
}
