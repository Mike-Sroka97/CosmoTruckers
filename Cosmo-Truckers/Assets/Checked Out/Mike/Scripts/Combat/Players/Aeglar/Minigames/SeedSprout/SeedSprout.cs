using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedSprout : CombatMove
{
    SeedSproutFlower[] flowers;
    int currentFlower = 0;

    private void Start()
    {
        GenerateLayout();
        flowers = GetComponentsInChildren<SeedSproutFlower>();
        StartMoveTest(); 
    }

    public override void StartMove()
    {
        NextFlower();
        SeedSproutMovingPlatform[] platforms = FindObjectsOfType<SeedSproutMovingPlatform>();
        foreach (SeedSproutMovingPlatform platform in platforms)
            platform.StartMove();
    }

    public void NextFlower()
    {
        flowers[currentFlower].TrackTime = true;
        currentFlower++;
    }

    public override void EndMove()
    {

    }
}
