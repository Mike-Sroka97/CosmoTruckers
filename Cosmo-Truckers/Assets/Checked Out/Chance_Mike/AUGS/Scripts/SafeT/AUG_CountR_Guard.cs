using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AUG_CountR_Guard : Augment
{
    int currentShield = 0;
    int lastHealth;

    public override void Activate(DebuffStackSO stack = null)
    {
        base.Activate(stack);
        //Get the current shield level of character at start of combat
        currentShield = stack.MyCharacter.Shield;
        lastHealth = stack.MyCharacter.CurrentHealth;

        AugmentSO.MyCharacter.HealthChangeEvent.AddListener(UpdateLastHealth);
    }

    public override void StopEffect()
    {
        AugmentSO.MyCharacter.Shield = 0;
        AugmentSO.MyCharacter.GetComponent<PlayerCharacter>().MyVessel.ManuallySetShield(0);
    }

    public void UpdateLastHealth()
    {
        lastHealth = AugmentSO.MyCharacter.CurrentHealth;
    }

    public override void Trigger()
    {
        //Only call damage if player has shield and it was delt damage
        if(currentShield > 0 && currentShield > AugmentSO.MyCharacter.Shield && AugmentSO.MyCharacter.CurrentHealth >= lastHealth)
        {
            CombatManager.Instance.GetCurrentEnemy.TakeDamage(currentShield - AugmentSO.MyCharacter.Shield);
        }

        //Remove after activation
        AugmentSO.MyCharacter.HealthChangeEvent.RemoveListener(UpdateLastHealth);
        AugmentSO.MyCharacter.RemoveDebuffStack(AugmentSO);
    }
}
