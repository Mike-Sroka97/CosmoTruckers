using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AUG_PorkedUp : Augment
{
    [SerializeField] DebuffStackSO hogWild;

    public override void Activate(DebuffStackSO stack = null)
    {
        if (stack.LastStacks != -1)
        {
            stack.MyCharacter.FlatDamageAdjustment -= stack.LastStacks;
            stack.MyCharacter.FlatHealingAdjustment -= stack.LastStacks;
        }

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
