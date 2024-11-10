using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToxicBubble : Augment
{
    public override void Activate(AugmentStackSO stack = null)
    {
        base.Activate(stack);
        AugmentSO.MyCharacter.AdjustVigor(-(int)StatusEffect);
        AugmentSO.MyCharacter.AdjustBubbleShield(true);
        AugmentSO.MyCharacter.BubbleShieldBrokenEvent.AddListener(StopEffect);
    }

    public override void StopEffect()
    {
        if (AugmentSO.MyCharacter.BubbleShielded)
            AugmentSO.MyCharacter.AdjustBubbleShield(false);

        AugmentSO.MyCharacter.AdjustVigor((int)StatusEffect);
    }
}
