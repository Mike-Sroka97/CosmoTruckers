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
            myButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = $"Give {PlayerVesselManager.Instance.PlayerVessels[i].MyCharacter.CharacterName} a <color=green>candy</color>.";
    }

    public void GiveMadness(int buttonID)
    {
        PlayerVesselManager.Instance.PlayerVessels[buttonID].MyCharacter.TakeHealing(healingAmount);
        IgnoreOption();
    }
}
