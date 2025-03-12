using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CrustyCorridor : EventNodeBase
{
    [SerializeField] int damage = 30;

    bool safePath;

    protected override void Start()
    {
        base.Start();
        safePath = HelperFunctions.RandomBool();
    }

    public void TraverseTheCrustyCorridor()
    {
        if(safePath)
        {
            foreach (PlayerVessel player in PlayerVesselManager.Instance.PlayerVessels)
            {
                player.MyCharacter.AddAugmentStack(augmentsToAdd[0], 3);
                player.MyCharacter.AddAugmentStack(augmentsToAdd[1], 3);
            }

            myButtons[0].GetComponentInChildren<TextMeshProUGUI>().text = "<color=green>The path yields great rewards.";
        }
        else
        {
            foreach (PlayerVessel player in PlayerVesselManager.Instance.PlayerVessels)
            {
                int currentDamage = damage;
                if (currentDamage >= player.MyCharacter.CurrentHealth)
                    currentDamage = player.MyCharacter.CurrentHealth - 1;

                player.MyCharacter.TakeDamage(currentDamage, true);
            }

            myButtons[0].GetComponentInChildren<TextMeshProUGUI>().text = "<color=red>The path crumbles injuring the party.";
        }

        IteratePlayerReference();
        StartCoroutine(SelectionChosen());
    }

    public override void IgnoreOption()
    {
        IteratePlayerReference();
        currentTurns = 69;
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
