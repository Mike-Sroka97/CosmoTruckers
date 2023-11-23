using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AUG_HogWild : Augment
{
    public override void Activate(DebuffStackSO stack = null)
    {
        base.Activate(stack);

        DebuffSO.MyCharacter.AdjustDamage((int)StatusEffect);
        DebuffSO.MyCharacter.AdjustRestoration((int)StatusEffect);
    }

    public override void StopEffect()
    {
        DebuffSO.MyCharacter.AdjustDamage(-(int)StatusEffect);
        DebuffSO.MyCharacter.AdjustRestoration(-(int)StatusEffect);
    }
}
