using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhiteNoise : CombatMove
{
    public override void StartMove()
    {
        FollowerNoise[] followerNoise = GetComponentsInChildren<FollowerNoise>();
        RandomNoise[] randomNoise = GetComponentsInChildren<RandomNoise>();
        GetComponentInChildren<RandomNoiseGenerator>().enabled = true;

        foreach (FollowerNoise noise in followerNoise)
            noise.enabled = true;
        foreach (RandomNoise noise in randomNoise)
            noise.enabled = true;

        base.StartMove();
    }

    public override string TrainingDisplayText => $"You scored {GetFinalEnemyScore()}/{maxScore} taking {GetFinalScore() * Damage + baseDamage} damage. You also gained {GetFinalAugmentStackCount()} stacks of {DebuffToAdd.AugmentName}.";
}
