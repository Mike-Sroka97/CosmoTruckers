using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LunarLight : Augment
{
    [SerializeField] int maxHealthIncrease = 18;
    [SerializeField] int damageDecrease = 12;

    public override void Activate(AugmentStackSO stack = null)
    {
        if (!firstGo)
        {
            AugmentSO.MyCharacter.AdjustDamage(-damageDecrease);
            AugmentSO.MyCharacter.AdjustMaxHealth(-maxHealthIncrease);
        }

        firstGo = false;

        AugmentSO.MyCharacter.AdjustDamage(-damageDecrease);
        AugmentSO.MyCharacter.AdjustMaxHealth(maxHealthIncrease);
        AugmentSO.MyCharacter.TakeHealing(maxHealthIncrease, true);
    }
    public override void StopEffect()
    {
        AugmentSO.MyCharacter.AdjustDamage(-damageDecrease);
        AugmentSO.MyCharacter.AdjustMaxHealth(-maxHealthIncrease);
    }
}
