using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NegativeGravityModifier : Augment
{

    public override void Activate(DebuffStackSO stack = null)
    {
        if(!firstGo)
            AugmentSO.MyCharacter.Stats.Gravity += StatusEffect;

        base.Activate(stack);
        AugmentSO.MyCharacter.Stats.Gravity -= StatusEffect;
    }
    public override void StopEffect()
    {
        AugmentSO.MyCharacter.Stats.Gravity += StatusEffect;
    }
}
