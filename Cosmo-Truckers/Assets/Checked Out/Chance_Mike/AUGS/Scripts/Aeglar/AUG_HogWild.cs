using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AUG_HogWild : Augment
{
    public override void Activate(AugmentStackSO stack = null)
    {
        base.Activate(stack);

        AugmentSO.MyCharacter.AdjustDamage((int)StatusEffect);
        AugmentSO.MyCharacter.AdjustRestoration((int)StatusEffect);
    }

    public override void StopEffect()
    {
        AugmentSO.MyCharacter.AdjustDamage(-(int)StatusEffect);
        AugmentSO.MyCharacter.AdjustRestoration(-(int)StatusEffect);
    }
}
