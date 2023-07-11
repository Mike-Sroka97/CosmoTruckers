using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TripleTether : CombatMove
{
    private void Start()
    {
        GenerateLayout();
        StartMove();
    }

    public override void EndMove()
    {
        throw new System.NotImplementedException();
    }
}
    
