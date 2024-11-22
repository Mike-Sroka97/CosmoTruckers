using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MysteriousCane : Augment
{
    public override void Activate(AugmentStackSO stack = null)
    {
        if (!firstGo)
        {
            AugmentSO.MyCharacter.AdjustDamage(-(int)StatusEffect);
            AugmentSO.MyCharacter.AdjustSpeed((int)StatusEffect);
        }

        base.Activate(stack);

        AugmentSO.MyCharacter.AdjustDamage((int)StatusEffect);
        AugmentSO.MyCharacter.AdjustSpeed(-(int)StatusEffect);
    }
    public override void StopEffect()
    {
        AugmentSO.MyCharacter.AdjustDamage(-(int)StatusEffect);
        AugmentSO.MyCharacter.AdjustSpeed((int)StatusEffect);
    }
}
