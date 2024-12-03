using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CryOfFrustration : CombatMove
{
    public override void StartMove()
    {
        GetComponentInChildren<CryOfFrustrationQmuav>().SetHealth(players.Length);
        SetupMultiplayer();
    }

    public override void EndMove()
    {
        if (FindObjectOfType<InaPractice>())
            return;

        if (FightWon)
            FindObjectOfType<QmuavAI>().DieForReal();
    }
}
