using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GloomyGloop : EventNodeBase
{
    public void GetGlooped()
    {
        AddAugmentToPlayer(augmentsToAdd[0]);
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
