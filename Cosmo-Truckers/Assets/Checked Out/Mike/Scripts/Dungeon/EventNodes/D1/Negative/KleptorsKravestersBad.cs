using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KleptorsKravestersBad : EventNodeBase
{
    Button[] myButtons;
    int badKravester;

    private void Start()
    {
        myButtons = GetComponentsInChildren<Button>();
        badKravester = Random.Range(0, 4);
    }

    public void EatKravester(int buttonID)
    {
        //Disable button and iterate player
        myButtons[buttonID].GetComponent<EventNodeButton>().MultiplayerSelected = true;
        myButtons[buttonID].enabled = false;
        IteratePlayerReference();

        //Selects next available button
        foreach (Button button in myButtons)
            if (button.enabled)
                button.Select();

        //Handles good/bad kravesters
        if(buttonID == badKravester)
        {
            PlayerVesselManager.Instance.PlayerVessels[nodeHandler.Player].MyCharacter.Stun(true);
        }
        else
        {
            if(PlayerVesselManager.Instance.PlayerVessels[nodeHandler.Player].MyCharacter.CurrentHealth > 1)
                PlayerVesselManager.Instance.PlayerVessels[nodeHandler.Player].MyCharacter.TakeDamage(1, true);
        }

        //don't end minigame while any buttons are active
        foreach (Button button in myButtons)
            if (button.enabled)
                return;

        StartCoroutine(SelectionChosen());
    }
}
