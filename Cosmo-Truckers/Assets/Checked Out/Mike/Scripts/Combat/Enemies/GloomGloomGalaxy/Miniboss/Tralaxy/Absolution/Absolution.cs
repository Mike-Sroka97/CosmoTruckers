using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Absolution : CombatMove
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

    protected override void TrackTime()
    {
        if (MoveEnded)
            return;

        base.TrackTime();
        if (Score >= 3)
            EndMove();
    }
}
