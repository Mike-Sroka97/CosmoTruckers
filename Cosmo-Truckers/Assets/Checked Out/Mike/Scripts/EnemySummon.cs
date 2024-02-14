using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySummon : Enemy
{
    public override void Die()
    {
        TurnOrder.Instance.RemoveFromSpeedList(Stats);
        base.Die();
    }
}
