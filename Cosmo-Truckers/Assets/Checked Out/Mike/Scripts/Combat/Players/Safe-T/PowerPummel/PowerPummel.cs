using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerPummel : CombatMove
{
    private void Start()
    {
        GenerateLayout();
    }

    public override void StartMove()
    {
        FindObjectOfType<PPhittable>().DetermineStartingMovement();
        PPspikeBall[] spikeBalls = FindObjectsOfType<PPspikeBall>();

        foreach (PPspikeBall spikeball in spikeBalls)
            spikeball.DetermineStartingMovement();
    }
}
