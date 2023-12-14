using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AUG_HollowBones : Augment
{
    const int superNegaativeStatusEffect = -200;

    private void Start()
    {
        baseStatusEffect = superNegaativeStatusEffect;
    }

    public override void Activate(DebuffStackSO stack = null)
    {
        base.Activate(stack);

        AugmentSO.MyCharacter.AdjustVigor((int)StatusEffect);
    }

    public override void StopEffect()
    {
        AugmentSO.MyCharacter.AdjustVigor(-(int)StatusEffect);
    }
}
