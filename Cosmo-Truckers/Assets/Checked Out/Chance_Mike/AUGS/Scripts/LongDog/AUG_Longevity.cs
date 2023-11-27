using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AUG_Longevity : Augment
{
    const int healingModifier = 2;

    public override void Activate(DebuffStackSO stack = null)
    {
        if (stack.LastStacks != -1)
        {
            stack.MyCharacter.AdjustSpeed(-stack.LastStacks);
        }

        base.Activate(stack);

        stack.MyCharacter.TakeHealing((int)StatusEffect * healingModifier); //heal for double the number of stacks
        stack.MyCharacter.AdjustSpeed((int)StatusEffect);
    }

    public override void Trigger()
    {
        Activate(DebuffSO);
    }

    public override void StopEffect()
    {
        DebuffSO.MyCharacter.AdjustSpeed(-(int)StatusEffect);
    }
}
