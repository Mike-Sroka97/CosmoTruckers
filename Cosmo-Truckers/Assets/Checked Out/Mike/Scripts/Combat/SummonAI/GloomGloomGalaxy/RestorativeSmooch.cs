using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestorativeSmooch : CombatMove
{
    public override void EndMove()
    {
        if (FindObjectOfType<InaPractice>())
            return;

        baseDamage = FindObjectOfType<DulaxyAI>().HealAmount;
        base.EndMove();
    }
}
