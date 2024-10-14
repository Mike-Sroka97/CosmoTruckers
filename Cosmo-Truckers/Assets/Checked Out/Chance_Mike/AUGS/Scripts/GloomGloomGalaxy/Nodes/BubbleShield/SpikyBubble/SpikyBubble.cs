using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikyBubble : Augment
{
    public override void Activate(AugmentStackSO stack = null)
    {
        base.Activate(stack);
        AugmentSO.MyCharacter.BubbleShieldBrokenEvent.AddListener(StopEffect);
    }

    public override void StopEffect()
    {
        if(AugmentSO.MyCharacter.BubbleShielded)
            AugmentSO.MyCharacter.AdjustBubbleShield(false);

        CombatManager.Instance.GetCurrentCharacter.TakeDamage((int)StatusEffect, true);
    }
}
