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
    }

    public override void StartMove()
    {
        NextFlower();
        SeedSproutMovingPlatform[] platforms = FindObjectsOfType<SeedSproutMovingPlatform>();
        foreach (SeedSproutMovingPlatform platform in platforms)
            platform.StartMove();

        base.StartMove();
    }

    public void NextFlower()
    {
        flowers[currentFlower].TrackTime = true;
        currentFlower++;
    }

    public override string TrainingDisplayText => $"You scored {Score = (Score > maxScore ? maxScore : Score)}/{maxScore}. The target received {Score + baseAugmentStacks} stacks of {DebuffToAdd.AugmentName}.";
}
