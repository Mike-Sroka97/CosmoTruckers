using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AUG_Strength : Augment
{
    public override void Activate(AugmentStackSO stack = null)
    {
        if (!firstGo)
            StopEffect();

        base.Activate(stack);

        AugmentSO.MyCharacter.FlatDamageAdjustment += (int)StatusEffect;
    }

    public override void StopEffect()
    {
        AugmentSO.MyCharacter.FlatDamageAdjustment -= (int)StatusEffect;
    }
}
