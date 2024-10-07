using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KleptorsKravestersGood : EventNodeBase
{
    Button[] myButtons;
    int goodKravester;

    private void Start()
    {
        myButtons = GetComponentsInChildren<Button>();
        goodKravester = Random.Range(0, 4);
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
            {
                button.Select();
                break;
            }

        //Handles good/bad kravesters
        if (buttonID == goodKravester)
            PlayerVesselManager.Instance.PlayerVessels[nodeHandler.Player].MyCharacter.Energize(true);
        else
            PlayerVesselManager.Instance.PlayerVessels[nodeHandler.Player].MyCharacter.TakeHealing(1, true);

        //don't end minigame while any buttons are active
        foreach (Button button in myButtons)
            if (button.enabled)
                return;

        StartCoroutine(SelectionChosen());
    }
}
