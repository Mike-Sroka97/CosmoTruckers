using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AUG_Wrath : Augment
{
    public override void Trigger()
    {
        if (!firstGo)
            StopEffect();

        AugmentSO.CurrentStacks++;
        Activate();
        AugmentSO.MyCharacter.AdjustDamage((int)StatusEffect);
    }

    public override void StopEffect()
    {
        AugmentSO.MyCharacter.AdjustDamage(-(int)StatusEffect);
    }
}
