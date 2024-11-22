using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursedMoon : EventNodeBase
{
    public void StareIntoTheMoon()
    {
        AddAugmentToPlayer(augmentsToAdd[0]);
        IgnoreOption();
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
