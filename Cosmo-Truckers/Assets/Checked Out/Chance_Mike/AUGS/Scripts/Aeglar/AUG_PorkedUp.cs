using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AUG_PorkedUp : Augment
{
    public override void Activate(DebuffStackSO stack = null)
    {
        if (!firstGo)
        {
            AugmentSO.MyCharacter.FlatDamageAdjustment -= AugmentSO.LastStacks;
            AugmentSO.MyCharacter.FlatHealingAdjustment -= AugmentSO.LastStacks;
        }

        base.Activate(stack);

        AugmentSO.MyCharacter.FlatDamageAdjustment += (int)StatusEffect;
        AugmentSO.MyCharacter.FlatHealingAdjustment += (int)StatusEffect;

        AugmentSO.LastStacks = AugmentSO.CurrentStacks;
    }

    public override void StopEffect()
    {
        AugmentSO.MyCharacter.FlatDamageAdjustment -= (int)StatusEffect;
        AugmentSO.MyCharacter.FlatHealingAdjustment -= (int)StatusEffect;
    }
}
