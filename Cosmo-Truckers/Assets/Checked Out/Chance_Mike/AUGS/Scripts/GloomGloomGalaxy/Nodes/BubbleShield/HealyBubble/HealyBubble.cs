using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealyBubble : Augment
{

    public override void Activate(AugmentStackSO stack = null)
    {
        if(firstGo)
        {
            base.Activate(stack);
            AugmentSO.MyCharacter.BubbleShieldBrokenEvent.AddListener(StopEffect);
            AugmentSO.MyCharacter.AdjustBubbleShield(true);
        }
        else
        {
            AugmentSO.MyCharacter.TakeHealing((int)StatusEffect);
        }
    }

    public override void StopEffect()
    {
        if (AugmentSO.MyCharacter.BubbleShielded)
            AugmentSO.MyCharacter.AdjustBubbleShield(false);
    }
}
