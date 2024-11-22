using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NebularCharge : EventNodeBase
{
    public void GetShocked()
    {
        AddAugmentToPlayer(augmentsToAdd[0]);
        AddAugmentToPlayer(augmentsToAdd[1]);
        IteratePlayerReference();
        currentTurns = 4;
        CheckEndEvent();
    }

    public override void HandleButtonSelect(int buttonId)
    {
        if (buttonId == 0)
        {
            PopupOne.gameObject.SetActive(true);
            PopupTwo.gameObject.SetActive(true);

            SetButtonWithAugInfo(augmentsToAdd[0]);
            SetButtonWithAugInfo(augmentsToAdd[1], false);
        }
        else
            HandleButtonDeselect();
    }
}
