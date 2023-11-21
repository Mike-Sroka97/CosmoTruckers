using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AUG_PorkedUp : Augment
{
    public DebuffStackSO HogWild;

    public override void Activate(DebuffStackSO stack = null)
    {
        if(StatusEffect != 0)
            StopEffect();
        base.Activate(stack);

        DebuffSO.MyCharacter.FlatDamageAdjustment += (int)StatusEffect;
        DebuffSO.MyCharacter.FlatHealingAdjustment += (int)StatusEffect;

        if(Stacks >= 5)
        {
            StopEffect();
            DebuffSO.MyCharacter.AddDebuffStack(HogWild);
            DebuffSO.MyCharacter.RemoveDebuffStack(DebuffSO, Stacks);
        }
    }

    public override void StopEffect()
    {
        DebuffSO.MyCharacter.FlatDamageAdjustment -= (int)StatusEffect;
        DebuffSO.MyCharacter.FlatHealingAdjustment -= (int)StatusEffect;
    }
}
