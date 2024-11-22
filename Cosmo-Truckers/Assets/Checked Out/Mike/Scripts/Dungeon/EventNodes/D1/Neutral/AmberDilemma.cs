using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmberDilemma : EventNodeBase
{
    [SerializeField] int healingAmount = 22;

    public void Ponder()
    {
        currentCharacter.TakeHealing(healingAmount);
        AddAugmentToPlayer(augmentsToAdd[0], 4);
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
