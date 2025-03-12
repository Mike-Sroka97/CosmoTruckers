using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class KlippsolsKaper : EventNodeBase
{
    [SerializeField] string goodText;
    [SerializeField] string badText;

    bool redGood;

    protected override void Start()
    {
        redGood = HelperFunctions.RandomBool();
        base.Start();
    }

    public void PickRedBucket(int buttonID)
    {
        if(redGood)
        {
            PlayerVesselManager.Instance.PlayerVessels[nodeHandler.Player].MyCharacter.RemoveAmountOfAugments(2, 0);
            myButtons[buttonID].GetComponentInChildren<TextMeshProUGUI>().text = goodText;
        }
        else
        {
            PlayerVesselManager.Instance.PlayerVessels[nodeHandler.Player].MyCharacter.AddAugmentStack(augmentsToAdd[0], 2);
            myButtons[buttonID].GetComponentInChildren<TextMeshProUGUI>().text = badText;
        }

        MultiplayerSelection(buttonID);
        CheckEndEvent();
    }

    public void PickBlueBucket(int buttonID)
    {
        if (redGood)
        {
            PlayerVesselManager.Instance.PlayerVessels[nodeHandler.Player].MyCharacter.AddAugmentStack(augmentsToAdd[0], 2);
            myButtons[buttonID].GetComponentInChildren<TextMeshProUGUI>().text = badText;
        }
        else
        {
            PlayerVesselManager.Instance.PlayerVessels[nodeHandler.Player].MyCharacter.RemoveAmountOfAugments(2, 0);
            myButtons[buttonID].GetComponentInChildren<TextMeshProUGUI>().text = goodText;
        }

        MultiplayerSelection(buttonID);
        CheckEndEvent();
    }

    public override void HandleButtonSelect(int buttonId)
    {
        PopupOne.gameObject.SetActive(true);

        SetButtonWithAugInfo(augmentsToAdd[0]);
    }
}
