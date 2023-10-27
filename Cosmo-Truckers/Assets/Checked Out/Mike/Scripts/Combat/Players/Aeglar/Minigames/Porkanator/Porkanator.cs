using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Porkanator : CombatMove
{
    public override void EndMove()
    {

    }

    private void Start()
    {
        StartMove();
        GenerateLayout();
    }
}
