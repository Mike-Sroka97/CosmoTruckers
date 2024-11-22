using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThoroughTonguing : EventNodeBase
{
    public void GetTongued()
    {
        PlayerVesselManager.Instance.PlayerVessels[nodeHandler.Player].MyCharacter.RemoveAmountOfAugments(999, 0);
        AddAugmentToPlayer(augmentsToAdd[0], 3);
        IteratePlayerReference();
        currentTurns = 4;
        CheckEndEvent();
    }

    public override void HandleButtonSelect(int buttonId)
    {
        if (buttonId == 0)
        {
            PopupOne.gameObject.SetActive(true);

            SetButtonWithAugInfo(augmentsToAdd[buttonId]);
        }
        else
            HandleButtonDeselect();
    }
}
