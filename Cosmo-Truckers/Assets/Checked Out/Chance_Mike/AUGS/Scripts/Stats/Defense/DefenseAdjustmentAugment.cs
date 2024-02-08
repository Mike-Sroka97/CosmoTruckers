using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenseAdjustmentAugment : Augment
{
    [SerializeField] bool defenseIncrease = true;

    public override void Activate(DebuffStackSO stack = null)
    {
        if (!firstGo)
        {
            AugmentSO.MyCharacter.AdjustDefense(-(int)StatusEffect);
        }

        if (!defenseIncrease && firstGo)
        {
            firstGo = false;
            baseStatusEffect = -baseStatusEffect;
            additionalStatusEffect = -additionalStatusEffect;
        }

        base.Activate(stack);

        AugmentSO.MyCharacter.AdjustDefense((int)StatusEffect);
    }
    public override void StopEffect()
    {
        AugmentSO.MyCharacter.AdjustDefense(-(int)StatusEffect);
    }
}
