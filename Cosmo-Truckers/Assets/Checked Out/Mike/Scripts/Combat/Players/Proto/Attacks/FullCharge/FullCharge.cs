using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FullCharge : CombatMove
{
    private void Start()
    {
        GenerateLayout();
    }
    public override void EndMove()
    {
        Debug.Log("move done");
    }
}
