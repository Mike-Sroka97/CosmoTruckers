using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StarCandy : EventNodeBase
{
    [SerializeField] int healingAmount = 4;

    protected override void Start()
    {
        base.Start();
        for (int i = 0; i < myButtons.Length; i++)
            myButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = $"Give <color={characterNameColor}>{PlayerVesselManager.Instance.PlayerVessels[i].MyCharacter.CharacterName}</color> a <color=green>candy</color>.";
    }

    public void GiveCandy(int buttonID)
    {
        PlayerVesselManager.Instance.PlayerVessels[buttonID].MyCharacter.TakeHealing(healingAmount);
        IgnoreOption();
    }
}
