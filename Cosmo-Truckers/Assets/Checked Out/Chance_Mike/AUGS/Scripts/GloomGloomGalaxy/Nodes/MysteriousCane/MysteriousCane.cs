using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MysteriousCane : Augment
{
    public override void Activate(DebuffStackSO stack = null)
    {
        if (!firstGo)
        {
            AugmentSO.MyCharacter.AdjustAttackDamage(-(int)StatusEffect);
            AugmentSO.MyCharacter.AdjustSpeed(-(int)StatusEffect);
        }

        base.Activate(stack);

        AugmentSO.MyCharacter.AdjustAttackDamage((int)StatusEffect);
        AugmentSO.MyCharacter.AdjustSpeed((int)StatusEffect);
    }
    public override void StopEffect()
    {
        AugmentSO.MyCharacter.AdjustAttackDamage(-(int)StatusEffect);
        AugmentSO.MyCharacter.AdjustSpeed(-(int)StatusEffect);
    }
}
