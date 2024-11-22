using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LostDulaxy : EventNodeBase
{
    [SerializeField] GameObject dulaxy;
    [SerializeField] int healthThreshold;
    [SerializeField] string popupText = "A Dulaxy is a (15) health player summon that restores health to the party on its turn.";

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

    public override void HandleButtonSelect(int buttonId)
    {
        if (buttonId == 0)
        {
            PopupOne.gameObject.SetActive(true);

            PopupOne.PopupText.text = popupText;
        }
    }
}
