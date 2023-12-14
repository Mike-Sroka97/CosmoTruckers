using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AUG_SpikyShield : Augment
{
    public int ShieldAmount = 50;
    public int ThornDamage = 10;

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
        CombatManager.Instance.GetCurrentPlayer.TakeDamage(ThornDamage);
    }
}
