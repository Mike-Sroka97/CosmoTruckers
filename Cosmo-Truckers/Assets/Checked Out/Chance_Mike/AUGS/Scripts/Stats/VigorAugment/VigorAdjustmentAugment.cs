using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VigorAdjustmentAugment : Augment
{
    [SerializeField] bool vigorIncrease = true;

    public override void Activate(DebuffStackSO stack = null)
    {
        if (!firstGo)
        {
            AugmentSO.MyCharacter.AdjustVigor(-(int)StatusEffect);
        }

        if (!vigorIncrease && firstGo)
        {
            baseStatusEffect = -baseStatusEffect;
            additionalStatusEffect = -additionalStatusEffect;
        }

        base.Activate(stack);

        AugmentSO.MyCharacter.AdjustVigor((int)StatusEffect);
    }
    public override void StopEffect()
    {
        AugmentSO.MyCharacter.AdjustVigor(-(int)StatusEffect);
    }
}
