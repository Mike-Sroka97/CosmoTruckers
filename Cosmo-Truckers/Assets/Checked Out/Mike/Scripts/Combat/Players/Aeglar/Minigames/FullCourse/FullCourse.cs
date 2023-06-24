using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FullCourse : CombatMove
{
    private void Start()
    {
        GenerateLayout();
    }

    public override void EndMove()
    {
        //mess with this when we start adding functionality to the moves
    }
}
