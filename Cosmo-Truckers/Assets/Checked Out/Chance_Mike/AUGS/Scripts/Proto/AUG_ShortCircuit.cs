using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AUG_ShortCircuit : Augment
{
    bool activated = false;

    public override void Activate(AugmentStackSO stack = null)
    {
        base.Activate(stack);

        if(!activated || 
            (CombatManager.Instance.GetCurrentCharacter.GetComponent<Enemy>() && !AugmentSO.MyCharacter.GetComponent<Enemy>()) ||
            (CombatManager.Instance.GetCurrentCharacter.GetComponent<PlayerCharacter>() && !AugmentSO.MyCharacter.GetComponent<PlayerCharacter>()))
        {
            activated = true;

            if (AugmentSO.MyCharacter.GetComponent<Enemy>())
                AugmentSO.MyCharacter.TakeDamage((int)StatusEffect);

            if (AugmentSO.MyCharacter.GetComponent<PlayerCharacter>())
                AugmentSO.MyCharacter.AdjustDefense((int)StatusEffect);
        }
        else if(CombatManager.Instance.CurrentAttack.CombatPrefab.GetComponent<CombatMove>().GetIsDamaging())
        {
            foreach (Character character in CombatManager.Instance.CharactersSelected)
                if(character != AugmentSO.MyCharacter)
                    character.AddAugmentStack(AugmentSO, AugmentSO.CurrentStacks);
            AugmentSO.SetFade(60);
        }
    }

    public override void StopEffect()
    {
        if(AugmentSO.MyCharacter.GetComponent<PlayerCharacter>())
            AugmentSO.MyCharacter.AdjustDefense(-(int)StatusEffect);
    }
}
