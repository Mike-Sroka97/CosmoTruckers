using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AUG_NovaSword : Augment
{
    [SerializeField] int shieldAmount = 35;

    public override void Activate(DebuffStackSO stack = null)
    {
        if (!firstGo)
            return;

        base.Activate(stack);
        AugmentSO.MyCharacter.AdjustDamage((int)StatusEffect);
        AugmentSO.MyCharacter.Shield += shieldAmount;
        firstGo = false;
    }

    public override void StopEffect()
    {
        AugmentSO.MyCharacter.Shield = 0;
        AugmentSO.MyCharacter.AdjustDamage(-(int)StatusEffect);
    }

    public override void Trigger()
    {
        if (AugmentSO.MyCharacter.Shield <= 0)
            StopEffect();
    }
}
