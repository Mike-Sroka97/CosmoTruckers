using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GalasterHordeAI : Enemy
{
    public override void TakeDamage(int damage, bool defensePiercing = false)
    {
        base.TakeDamage(damage, defensePiercing);
    }

    public override void StartTurn()
    {
        TurnOrder.Instance.EndTurn();
    }
}
