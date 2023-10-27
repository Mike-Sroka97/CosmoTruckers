using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CumuloLickus : CombatMove
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
        Score = (int)currentTime;
    }
}
