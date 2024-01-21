using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FauxDemoPawAI : EnemySummon
{
    public override void EndTurn()
    {
        base.EndTurn();
        TakeDamage(1, true);
    }
}
