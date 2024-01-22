using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModerateDefense : Augment
{
    [SerializeField] bool defenseIncrease = true;
    const int moderateBaseStatusEffect = 12;
    const int moderateAdditionalStatusEffect = 3;

    public override void Activate(DebuffStackSO stack = null)
    {
        baseStatusEffect = moderateBaseStatusEffect;
        additionalStatusEffect = moderateAdditionalStatusEffect;

        if (!defenseIncrease)
        {
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
