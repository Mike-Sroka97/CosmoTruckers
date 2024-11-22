using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AstroDog : EventNodeBase
{
    public void PetTheDog(int buttonID)
    {
        PlayerVesselManager.Instance.PlayerVessels[nodeHandler.Player].MyCharacter.ProliferateAugment(1, 1);
        MultiplayerSelection(buttonID);
        CheckEndEvent();
    }

    public void PlayerWithTheDog(int buttonID)
    {
        PlayerVesselManager.Instance.PlayerVessels[nodeHandler.Player].MyCharacter.RemoveAmountOfAugments(1, 0);
        MultiplayerSelection(buttonID);
        CheckEndEvent();
    }

    public void ShakeTheDogsPaw(int buttonID)
    {
        PlayerVesselManager.Instance.PlayerVessels[nodeHandler.Player].MyCharacter.AddAugmentStack(augmentsToAdd[0]);
        MultiplayerSelection(buttonID);
        CheckEndEvent();
    }

    public void FeedTheDog(int buttonID)
    {
        PlayerVesselManager.Instance.PlayerVessels[nodeHandler.Player].MyCharacter.TakeHealing(5);
        MultiplayerSelection(buttonID);
        CheckEndEvent();
    }

    public override void HandleButtonSelect(int buttonId)
    {
        if (buttonId == 2)
        {
            PopupOne.gameObject.SetActive(true);

            SetButtonWithAugInfo(augmentsToAdd[0]);
        }
        else
            HandleButtonDeselect();
    }
}
