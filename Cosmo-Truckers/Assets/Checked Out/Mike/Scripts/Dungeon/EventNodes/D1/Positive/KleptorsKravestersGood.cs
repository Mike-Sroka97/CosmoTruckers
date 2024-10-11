using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KleptorsKravestersGood : EventNodeBase
{
    int goodKravester;

    protected override void Start()
    {
        base.Start();
        goodKravester = Random.Range(0, 4);
    }

    public void EatKravester(int buttonID)
    {
        //Handles good/bad kravesters
        if (buttonID == goodKravester)
            PlayerVesselManager.Instance.PlayerVessels[nodeHandler.Player].MyCharacter.Energize(true);
        else
            PlayerVesselManager.Instance.PlayerVessels[nodeHandler.Player].MyCharacter.TakeHealing(1, true);

        MultiplayerSelection(buttonID);
        CheckEndEvent();
    }
}
