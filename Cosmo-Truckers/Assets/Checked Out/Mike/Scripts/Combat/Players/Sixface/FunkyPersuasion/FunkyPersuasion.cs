using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FunkyPersuasion : CombatMove
{
    private void Start()
    {
        GenerateLayout();
    }
    public override void StartMove()
    {
        base.StartMove();
    }

    public override void EndMove()
    {
        if (FindObjectOfType<InaPractice>())
            return;

        base.EndMove();

        Character target = CombatManager.Instance.CharactersSelected[0];
        int subductionToAdd = 2; //base augment to add. Reduce if they are already subdued

        foreach(AugmentStackSO augment in target.GetAUGS)
        {
            if(augment.AugmentName == DebuffToAdd.AugmentName)
            {
                subductionToAdd--;
                break;
            }
        }

        target.AddAugmentStack(DebuffToAdd, subductionToAdd);

        FindObjectOfType<SixFaceMana>().UpdateFace();
    }

    public override string TrainingDisplayText => $"You scored {Score = (Score > maxScore ? maxScore : Score)}/{maxScore} dealing {Score * Damage + baseDamage} damage. The target also received {DebuffToAdd.AugmentName}.";
}
