using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageAdjustmentAugment : Augment
{
    [SerializeField] bool damageIncrease = true;

    public override void Activate(AugmentStackSO stack = null)
    {
        if (!firstGo)
        {
            AugmentSO.MyCharacter.AdjustDamage(-(int)StatusEffect);
        }

        if (!damageIncrease && firstGo)
        {
            baseStatusEffect = -baseStatusEffect;
            additionalStatusEffect = -additionalStatusEffect;
        }

        base.Activate(stack);

        AugmentSO.MyCharacter.AdjustDamage((int)StatusEffect);
    }
    public override void StopEffect()
    {
        AugmentSO.MyCharacter.AdjustDamage(-(int)StatusEffect);
    }
}
