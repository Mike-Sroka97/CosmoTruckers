using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhiteNoise : CombatMove
{
    public override void EndMove()
    {
        throw new System.NotImplementedException();
    }

    private void Start()
    {
        StartMove();
        GenerateLayout();
    }
}
