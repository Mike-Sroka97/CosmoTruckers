using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MajorVigor : Augment
{
    [SerializeField] bool vigorIncrease = true;
    const int majorBaseStatusEffect = 50;
    const int majorAdditionalStatusEffect = 10;

    public override void Activate(DebuffStackSO stack = null)
    {
        baseStatusEffect = majorBaseStatusEffect;
        additionalStatusEffect = majorAdditionalStatusEffect;

        if (!vigorIncrease)
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
