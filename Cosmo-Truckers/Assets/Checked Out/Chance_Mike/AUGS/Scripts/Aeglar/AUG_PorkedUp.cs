using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AUG_PorkedUp : Augment
{
    public DebuffStackSO HogWild;

    public override void Activate(DebuffStackSO stack = null)
    {
        base.Activate(stack);

        DebuffSO.MyCharacter.AdjustDamage((int)StatusEffect);

        if(Stacks >= 5)
        {
            StopEffect();
            DebuffSO.MyCharacter.AddDebuffStack(HogWild);
            DebuffSO.MyCharacter.RemoveDebuffStack(DebuffSO, Stacks);
        }
    }

    public override void StopEffect()
    {
        DebuffSO.MyCharacter.AdjustDamage(-(int)StatusEffect);
    }
}
