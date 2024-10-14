using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FunkyPersuasion : CombatMove
{
    private void Start()
    {
        StartMove();
        GenerateLayout();

        base.StartMove();
    }

    public override void EndMove()
    {
        base.EndMove();

        Character target = CombatManager.Instance.CharactersSelected[0];
        int subductionToAdd = 2; //base augment to add. Reduce if they are already subdued

        foreach(AugmentStackSO augment in target.GetAUGS)
        {
            if(augment.DebuffName == "Subduction")
            {
                subductionToAdd--;
                break;
            }
        }

        target.AddAugmentStack(DebuffToAdd, subductionToAdd);

        FindObjectOfType<SixFaceMana>().UpdateFace();
    }
}
