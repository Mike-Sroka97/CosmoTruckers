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

        vigorAdjustment = -(Stacks * vigorModifier);
        DebuffSO.MyCharacter.AdjustVigor(vigorAdjustment);
    }

    public override void AdjustStatusEffect(int adjuster)
    {
        AdjustVigor();
        base.AdjustStatusEffect(adjuster);
        vigorAdjustment = -(Stacks * vigorModifier);
        DebuffSO.MyCharacter.AdjustVigor(vigorAdjustment);
    }

    private void AdjustVigor()
    {
        DebuffSO.MyCharacter.AdjustVigor(-(int)vigorAdjustment);
    }
}
