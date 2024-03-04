using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AUG_SpikyShield : Augment
{
    [SerializeField] int shieldAmount = 35;

    public override void Activate(DebuffStackSO stack = null)
    {
        if (!firstGo)
            return;

        base.Activate(stack);
        AugmentSO.MyCharacter.Shield += shieldAmount;
        firstGo = false;
    }

    public override void StopEffect()
    {
        foreach (CosmicCrustAI cc in FindObjectsOfType<CosmicCrustAI>())
            cc.QueueNextMove();

        AugmentSO.MyCharacter.Shield = 0;
    }

    public override void Trigger()
    {
        CombatManager.Instance.GetCurrentCharacter.TakeDamage((int)StatusEffect);

        if (AugmentSO.MyCharacter.Shield <= 0)
            StopEffect();
    }
}
