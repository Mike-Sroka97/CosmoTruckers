using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrustAUG : Augment
{

    public override void Activate(DebuffStackSO stack = null)
    {
        base.Activate(stack);
        stack.MyCharacter.GetComponent<CharacterStats>().Defense += (int)StatusEffect;

        if (stack.MyCharacter.GetComponent<CharacterStats>().Defense > 100)
            stack.MyCharacter.GetComponent<CharacterStats>().Defense = 100;
    }

    public override void AdjustStatusEffect(int adjuster)
    {
        StopEffect();
        base.AdjustStatusEffect(adjuster);
        Activate(DebuffSO);
    }

    public override void StopEffect()
    {
        DebuffSO.MyCharacter.GetComponent<CharacterStats>().Defense -= (int)StatusEffect;

        if (DebuffSO.MyCharacter.GetComponent<CharacterStats>().Defense < -100)
            DebuffSO.MyCharacter.GetComponent<CharacterStats>().Defense = -100;
    }
}
