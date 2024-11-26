using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikyBubble : Augment
{
    public override void Activate(AugmentStackSO stack = null)
    {
        base.Activate(stack);
        AugmentSO.MyCharacter.BubbleShieldBrokenEvent.AddListener(StopEffect);
        AugmentSO.MyCharacter.AdjustBubbleShield(true);
    }

    public override void StopEffect()
    {
        if (CombatManager.Instance.AttackingCharacter == AugmentSO.MyCharacter)
            return;

        if(AugmentSO.MyCharacter.BubbleShielded)
            AugmentSO.MyCharacter.AdjustBubbleShield(false);

        CombatManager.Instance.AttackingCharacter.TakeDamage((int)StatusEffect, true);
    }
}
