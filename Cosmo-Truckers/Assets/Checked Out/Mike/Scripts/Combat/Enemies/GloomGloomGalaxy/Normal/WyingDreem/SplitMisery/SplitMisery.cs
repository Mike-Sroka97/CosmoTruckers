using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplitMisery : CombatMove
{
    private void Start()
    {
        StartMove();
        GenerateLayout();
    }

    private void Update()
    {
        TrackTime();
    }

    public override void EndMove()
    {
        Debug.Log("Your ending score is: " + Score);
        base.EndMove();
    }
}
