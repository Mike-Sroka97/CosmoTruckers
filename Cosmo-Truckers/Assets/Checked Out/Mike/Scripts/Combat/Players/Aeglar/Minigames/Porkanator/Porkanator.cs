using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Porkanator : CombatMove
{
    [SerializeField] AugmentStackSO hogwild;
    [SerializeField] int hogwildStacks = 1;

    private void Start()
    {
        GenerateLayout();
    }

    public override void EndMove()
    {
        base.EndMove();

        foreach(Character character in CombatManager.Instance.GetCharactersSelected)
        {
            for (int i = 0; i < character.GetAUGS.Count; i++)
            {
                if (character.GetAUGS[i].DebuffName == "Pork'd Up" && character.GetAUGS[i].CurrentStacks >= hogwildStacks)
                {
                    character.RemoveDebuffStack(character.GetAUGS[i], character.GetAUGS[i].CurrentStacks);
                    character.AddAugmentStack(hogwild);
                }
            }
        }
    }
}
