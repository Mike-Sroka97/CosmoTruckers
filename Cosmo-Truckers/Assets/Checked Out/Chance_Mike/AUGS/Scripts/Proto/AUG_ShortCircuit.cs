using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AUG_ShortCircuit : Augment
{
    bool activated = false;

    public override void Activate(DebuffStackSO stack = null)
    {
        base.Activate(stack);

        if(!activated || 
            (CombatManager.Instance.GetCurrentCharacter.GetComponent<Enemy>() && !DebuffSO.MyCharacter.GetComponent<Enemy>()) ||
            (CombatManager.Instance.GetCurrentCharacter.GetComponent<PlayerCharacter>() && !DebuffSO.MyCharacter.GetComponent<PlayerCharacter>()))
        {
            activated = true;

            if (DebuffSO.MyCharacter.GetComponent<Enemy>())
                DebuffSO.MyCharacter.TakeDamage((int)StatusEffect);

            if (DebuffSO.MyCharacter.GetComponent<PlayerCharacter>())
                DebuffSO.MyCharacter.AdjustDefense((int)StatusEffect);
        }
        else if(CombatManager.Instance.CurrentAttack.CombatPrefab.GetComponent<CombatMove>().GetIsDamaging())
        {
            foreach (Character character in CombatManager.Instance.CharactersSelected)
                character.AddDebuffStack(DebuffSO, DebuffSO.CurrentStacks);
            DebuffSO.MyCharacter.RemoveDebuffStack(DebuffSO, DebuffSO.MaxStacks);
        }
    }

    public override void StopEffect()
    {
        if(DebuffSO.MyCharacter.GetComponent<PlayerCharacter>())
            DebuffSO.MyCharacter.AdjustDefense(-(int)StatusEffect);
    }
}
