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
        Destroy(gameObject);
    }

    protected override int SelectAttack()
    {
        throw new System.NotImplementedException();
    }
}
