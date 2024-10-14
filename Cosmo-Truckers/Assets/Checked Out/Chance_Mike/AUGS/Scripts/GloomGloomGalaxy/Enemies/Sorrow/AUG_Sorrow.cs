using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AUG_Sorrow : Augment
{
    public override void Activate(AugmentStackSO stack = null)
    {
        base.Activate(stack);

        AugmentSO.MyCharacter.TakeMultiHitDamage(1, AugmentSO.CurrentStacks, true);
    }

    public override void StopEffect()
    {
        //LOL
    }
}
