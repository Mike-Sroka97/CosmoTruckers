using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClockOutKnockOut : CombatMove
{
    private void Start()
    {
        GenerateLayout();
    }

    public override void StartMove()
    {
        COKOhand[] hands = FindObjectsOfType<COKOhand>();

        foreach(COKOhand hand in hands)
        {
            hand.SetVelocity();
        }

        base.StartMove();
    }

    public override void EndMove()
    {
        base.EndMove();
        FindObjectOfType<SafeTMana>().SetCurrentAnger(1);
    }
}
