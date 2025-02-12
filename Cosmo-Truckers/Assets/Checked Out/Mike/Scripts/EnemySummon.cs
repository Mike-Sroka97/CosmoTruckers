using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySummon : Enemy
{
    public override void Die()
    {
        EnemyManager.Instance.EnemyCombatSpots[CombatSpot] = null;
        TurnOrder.Instance.RemoveFromSpeedList(Stats);
        base.Die();
        StartCoroutine(WaitToDestroy()); 
    }

    IEnumerator WaitToDestroy()
    {
        while (CombatManager.Instance.CommandsExecuting > 0)
            yield return null;

        Destroy(gameObject);
    }
}
