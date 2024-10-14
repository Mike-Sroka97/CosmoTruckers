using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositiveGravityModifier : Augment
{
    public override void Activate(AugmentStackSO stack = null)
    {
        if (!firstGo)
            AugmentSO.MyCharacter.AdjustGravity(-StatusEffect);

        base.Activate(stack);
        AugmentSO.MyCharacter.AdjustGravity(StatusEffect);
    }
    public override void StopEffect()
    {
        AugmentSO.MyCharacter.AdjustGravity(-StatusEffect);
    }
}
