using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AUG_HogWild : Augment
{
    public override void Activate(DebuffStackSO stack = null)
    {
        base.Activate(stack);

        DebuffSO.MyCharacter.FlatDamageAdjustment += (int)StatusEffect;
        DebuffSO.MyCharacter.FlatHealingAdjustment += (int)StatusEffect;
    }

    public override void StopEffect()
    {
        DebuffSO.MyCharacter.FlatDamageAdjustment -= (int)StatusEffect;
        DebuffSO.MyCharacter.FlatHealingAdjustment -= (int)StatusEffect;
    }
}
