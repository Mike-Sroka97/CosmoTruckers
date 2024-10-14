using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlatDamageAdjustAugment : Augment
{
    [SerializeField] bool flatDamageIncrease = true;

    public override void Activate(AugmentStackSO stack = null)
    {
        if (!firstGo)
        {
            AugmentSO.MyCharacter.FlatDamageAdjustment -= (int)StatusEffect;
        }

        if (!flatDamageIncrease && firstGo)
        {
            baseStatusEffect = -baseStatusEffect;
            additionalStatusEffect = -additionalStatusEffect;
        }

        base.Activate(stack);

        AugmentSO.MyCharacter.FlatDamageAdjustment += (int)StatusEffect;
    }
    public override void StopEffect()
    {
        AugmentSO.MyCharacter.FlatDamageAdjustment -= (int)StatusEffect;
    }
}
