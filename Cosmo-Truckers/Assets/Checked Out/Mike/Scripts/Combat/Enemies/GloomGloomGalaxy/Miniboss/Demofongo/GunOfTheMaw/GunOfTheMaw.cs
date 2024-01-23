using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunOfTheMaw : CombatMove
{
    public override void StartMove()
    {
        GetComponentInChildren<GunOfTheMawHead>().enabled = true;
        GetComponentInChildren<GunOfTheMawGun>().enabled = true;

        base.StartMove();
    }
}
