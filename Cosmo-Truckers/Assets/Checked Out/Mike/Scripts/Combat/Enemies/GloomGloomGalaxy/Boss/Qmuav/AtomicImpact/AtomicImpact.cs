using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtomicImpact : CombatMove
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

    public void GenerateNextWave()
    {
        GenerateLayout();
    }
}
