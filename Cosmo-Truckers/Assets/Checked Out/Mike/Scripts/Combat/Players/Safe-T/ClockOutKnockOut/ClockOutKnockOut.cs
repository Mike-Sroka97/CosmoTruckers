using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClockOutKnockOut : CombatMove
{
    private void Start()
    {
        GenerateLayout();
        StartMoveTest(); 
    }

    public override void StartMove()
    {
        COKOhand[] hands = FindObjectsOfType<COKOhand>();

        foreach(COKOhand hand in hands)
        {
            hand.SetVelocity();
        }
    }

    public override void EndMove()
    {
        base.EndMove();
        FindObjectOfType<SafeTMana>().SetCurrentAnger(1);
    }
}
