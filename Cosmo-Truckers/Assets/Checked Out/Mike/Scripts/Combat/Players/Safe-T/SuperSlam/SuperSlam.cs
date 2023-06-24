using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuperSlam : CombatMove
{
    public override void EndMove()
    {
        throw new System.NotImplementedException();
    }

    private void Start()
    {
        FindObjectOfType<SafeTINA>().SetMoveSpeed(0);
        StartMove();
        GenerateLayout();
    }
}
