using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorsingAround : CombatMove
{
    public override void StartMove()
    {
        GetComponentInChildren<HorsingAroundHorse>().enabled = true;
    }
    public override void EndMove()
    {
        if (PlayerDead)
            Score++;

        int damage = CalculateScore();
        DealDamageOrHealing(CombatManager.Instance.GetCharactersSelected[0], damage);
    }
}
