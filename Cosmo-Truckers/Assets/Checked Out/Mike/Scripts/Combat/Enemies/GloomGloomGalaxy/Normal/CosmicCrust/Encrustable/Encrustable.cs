using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Encrustable : CombatMove
{
    [SerializeField] int maxScore = 3;

    private void Start()
    {
        StartMove();
        GenerateLayout();
    }

    private void Update()
    {
        TrackTime();
        if (Score >= maxScore && !MoveEnded)
            EndMove();
    }
}
