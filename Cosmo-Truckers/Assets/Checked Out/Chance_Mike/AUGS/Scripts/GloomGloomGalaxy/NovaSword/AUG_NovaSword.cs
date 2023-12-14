using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AUG_NovaSword : Augment
{
    public int ShieldAmount = 35;
    public DebuffStackSO DebuffToAdd;

    public override void Activate(DebuffStackSO stack = null)
    {
        base.Activate(stack);

        AugmentSO.MyCharacter.Shield = ShieldAmount;
    }

    public override void StopEffect()
    {
        //Remove all sheild
        AugmentSO.MyCharacter.Shield = 0;
    }

    public override void Trigger()
    {
        CombatManager.Instance.GetCurrentPlayer.AddDebuffStack(DebuffToAdd);
    }
}
