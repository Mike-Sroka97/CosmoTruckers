using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RetinopathyAUG : Augment
{
    [SerializeField] int vigorDecrease = 50;

    public override void Activate(DebuffStackSO stack = null)
    {
        if (!firstGo)
        {
            AugmentSO.MyCharacter.AdjustVigor(-vigorDecrease);
            AugmentSO.MyCharacter.AdjustMaxHealth(-(AugmentSO.MyCharacter.Health / 2)); //half
        }

        firstGo = false;

        AugmentSO.MyCharacter.AdjustVigor(-vigorDecrease);
        AugmentSO.MyCharacter.AdjustMaxHealth(AugmentSO.MyCharacter.Health);
        AugmentSO.MyCharacter.TakeHealing(AugmentSO.MyCharacter.Health / 2, true);
    }
    public override void StopEffect()
    {
        AugmentSO.MyCharacter.AdjustVigor(-vigorDecrease);
        AugmentSO.MyCharacter.AdjustMaxHealth(-(AugmentSO.MyCharacter.Health / 2)); //half
    }
}
