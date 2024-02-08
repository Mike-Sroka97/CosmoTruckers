using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NovaSpikes : Augment
{
    public override void Activate(DebuffStackSO stack = null)
    {
        if (!firstGo)
            return;

        base.Activate(stack);
        firstGo = false;
    }

    public override void StopEffect()
    {
        //No-op just stops triggering
    }

    public override void Trigger()
    {
        CombatManager.Instance.GetCurrentPlayer.TakeDamage((int)StatusEffect);
    }
}
