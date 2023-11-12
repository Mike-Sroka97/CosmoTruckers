using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FunkyPersuasion : CombatMove
{
    private void Start()
    {
        StartMove();
        GenerateLayout();
    }

    public override void EndMove()
    {
        base.EndMove();
        FindObjectOfType<SixFaceMana>().UpdateFace();
    }
}
