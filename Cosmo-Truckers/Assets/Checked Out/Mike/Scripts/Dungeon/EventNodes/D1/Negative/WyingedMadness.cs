using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WyingedMadness : EventNodeBase
{
    protected override void Start()
    {
        base.Start();
        for(int i = 0; i < myButtons.Length; i++)
            myButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = $"<color=red>Give <color={characterNameColor}>{PlayerVesselManager.Instance.PlayerVessels[i].MyCharacter.CharacterName}</color> (1) Nitemare";
    }

    public void GiveMadness(int buttonID)
    {
        PlayerVesselManager.Instance.PlayerVessels[buttonID].MyCharacter.AddAugmentStack(augmentsToAdd[0]);
        IgnoreOption();
    }

    public override void HandleButtonSelect(int buttonId)
    {
        PopupOne.gameObject.SetActive(true);

        SetButtonWithAugInfo(augmentsToAdd[0]);
    }
}
