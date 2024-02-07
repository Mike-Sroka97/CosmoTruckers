using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GloomGuarded : CombatMove
{
    void Start()
    {
        GenerateLayout();
    }

    public override void EndMove()
    {
        MoveEnded = true;

        CombatManager.Instance.GetCharactersSelected[0].TakeShielding(CalculateScore());
    }
}
