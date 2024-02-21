using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealyBubble : Augment
{
    bool startup = true;

    public override void Activate(DebuffStackSO stack = null)
    {
        if(startup)
        {
            base.Activate(stack);
            AugmentSO.MyCharacter.BubbleShieldBrokenEvent.AddListener(StopEffect);
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
