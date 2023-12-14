using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AUG_Wet : VisualAugment
{
    [SerializeField] int vigorModifier = 5;

    int vigorAdjustment;

    public override void Activate(DebuffStackSO stack = null)
    {
        base.Activate(stack);

        vigorAdjustment = -(AugmentSO.CurrentStacks * vigorModifier);
        AugmentSO.MyCharacter.AdjustVigor(vigorAdjustment);
    }

    public override void AdjustStatusEffect(int adjuster)
    {
        AdjustVigor();
        base.AdjustStatusEffect(adjuster);
        vigorAdjustment = -(AugmentSO.CurrentStacks * vigorModifier);
        AugmentSO.MyCharacter.AdjustVigor(vigorAdjustment);
    }

    private void AdjustVigor()
    {
        AugmentSO.MyCharacter.AdjustVigor(-(int)vigorAdjustment);
    }
}
