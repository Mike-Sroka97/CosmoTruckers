using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResinatedAUG : Augment
{
    public Enemy enemyToCheck;
    public PlayerCharacter playerToCheck;

    BaseAttackSO toRemove;

    public override void Activate(DebuffStackSO stack = null)
    {
        toRemove = playerToCheck.GetAllAttacks[Random.Range(0, playerToCheck.GetAllAttacks.Count)];

        toRemove.CanUse = false;

        InvokeRepeating("CheckEnemy", .5f, .5f);
    }

    void CheckEnemy()
    {
        if (enemyToCheck.Health <= 0)
            StopEffect();

    }

    public override void StopEffect()
    {
        CancelInvoke();

        toRemove.CanUse = true;
    }
}
