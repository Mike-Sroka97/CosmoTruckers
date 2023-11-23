using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AUG_PorkedUp : Augment
{
    [SerializeField] DebuffStackSO hogWild;

    public override void Activate(DebuffStackSO stack = null)
    {
        if(StatusEffect != 0)
            StopEffect();
        base.Activate(stack);

        DebuffSO.MyCharacter.FlatDamageAdjustment += (int)StatusEffect;
        DebuffSO.MyCharacter.FlatHealingAdjustment += (int)StatusEffect;

        if(Stacks >= 1)
        {
            stack.StopEffect();
            hogWild = Resources.Load("Assets/Checked Out/Chance_Mike/AUGS/ScriptableOBJ/Aeglar/HogWild") as DebuffStackSO;
            DebuffSO.MyCharacter.AddDebuffStack(hogWild);
            DebuffSO.MyCharacter.RemoveDebuffStack(DebuffSO, Stacks);
        }
    }

    public override void StopEffect()
    {
        DebuffSO.MyCharacter.FlatDamageAdjustment -= (int)StatusEffect;
        DebuffSO.MyCharacter.FlatHealingAdjustment -= (int)StatusEffect;
    }
}
