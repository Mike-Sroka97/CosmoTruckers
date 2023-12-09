using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AUG_Megawatt : Augment
{
    public override void Activate(DebuffStackSO stack = null)
    {
        base.Activate(stack);

        if(CombatManager.Instance.CurrentAttack.CombatPrefab.GetComponent<CombatMove>().GetIsDamaging())
        {
            DebuffSO.MyCharacter.AdjustDamage((int)StatusEffect);
            DebuffSO.SetFade(DebuffSO.MaxStacks); //makes sure to get rid of aug at the end of the turn
        }
    }

    public override void StopEffect()
    {
        Debug.Log("here");
        DebuffSO.MyCharacter.AdjustDamage(-(int)StatusEffect);
    }
}
