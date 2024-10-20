using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecklessConductorAhoy : EventNodeBase
{
    public void FullSpeedAhead()
    {
        foreach(PlayerVessel player in PlayerVesselManager.Instance.PlayerVessels)
        {
            int currentDamage = player.MyCharacter.Health / 2; // half health

            if (currentDamage >= player.MyCharacter.CurrentHealth)
                currentDamage = player.MyCharacter.CurrentHealth - 1;

            player.MyCharacter.TakeDamage(currentDamage, true);
        }

        DungeonController controller = FindObjectOfType<DungeonController>();
        controller.CombatNodes[controller.CurrentCombat].CombatDone = true;
        controller.CurrentCombat++;

        IteratePlayerReference();
        StartCoroutine(SelectionChosen());
    }

    public void NoSpeedAhead()
    {
        IteratePlayerReference();
        StartCoroutine(SelectionChosen());
    }
}
