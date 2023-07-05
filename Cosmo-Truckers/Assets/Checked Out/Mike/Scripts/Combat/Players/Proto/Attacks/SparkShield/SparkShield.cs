using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SparkShield : CombatMove
{
    [HideInInspector] public bool PlayerBuff = false;

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
