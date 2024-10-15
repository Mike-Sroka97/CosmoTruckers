using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LostDulaxy : EventNodeBase
{
    [SerializeField] GameObject dulaxy;
    [SerializeField] int healthThreshold;

    public void DulaxyAttraction()
    {
        for(int i = 0; i < 4; i++)
        {
            if (!(EnemyManager.Instance.PlayerSummons.Count >= EnemyManager.Instance.Players.Count)
                && !EnemyManager.Instance.CheckForSummon(PlayerVesselManager.Instance.PlayerVessels[i].MyCharacter)
                && PlayerVesselManager.Instance.PlayerVessels[i].MyCharacter.CurrentHealth <= healthThreshold)
                EnemyManager.Instance.UpdatePlayerSummons(dulaxy, PlayerVesselManager.Instance.PlayerVessels[i].MyCharacter, PlayerVesselManager.Instance.PlayerVessels[i].MyCharacter.CombatSpot + 4);
        }

        IteratePlayerReference();
        StartCoroutine(SelectionChosen());
    }
}
