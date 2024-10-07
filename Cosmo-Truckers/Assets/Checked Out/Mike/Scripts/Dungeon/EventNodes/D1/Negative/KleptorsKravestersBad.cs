using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KleptorsKravestersBad : EventNodeBase
{
    int badKravester;

    protected override void Start()
    {
        base.Start();
        badKravester = Random.Range(0, 4);
    }

    public void EatKravester(int buttonID)
    {
        //Handles good/bad kravesters
        if (buttonID == badKravester)
        {
            PlayerVesselManager.Instance.PlayerVessels[nodeHandler.Player].MyCharacter.Stun(true);
        }
        else
        {
            if (PlayerVesselManager.Instance.PlayerVessels[nodeHandler.Player].MyCharacter.CurrentHealth > 1)
                PlayerVesselManager.Instance.PlayerVessels[nodeHandler.Player].MyCharacter.TakeDamage(1, true);
        }

        MultiplayerSelection(buttonID);

        //don't end minigame while any buttons are active
        foreach (Button button in myButtons)
            if (button.enabled)
                return;

        StartCoroutine(SelectionChosen());
    }
}
