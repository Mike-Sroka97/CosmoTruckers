using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AUG_Shocked : Augment
{
    public override void Activate(DebuffStackSO stack = null)
    {
        if (!firstGo)
            StopEffect();

        base.Activate(stack);
        AugmentSO.MyCharacter.AdjustSpeed(-(int)StatusEffect);
    }

    public override void StopEffect()
    {
        AugmentSO.MyCharacter.AdjustSpeed((int)StatusEffect);
    }
}
