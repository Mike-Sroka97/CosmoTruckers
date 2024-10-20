using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TheOneCane : EventNodeBase
{
    [SerializeField] int passDamage = 10;

    int timesPassed = 1;

    protected override void Start()
    {
        base.Start();

        myButtons[0].GetComponentInChildren<TextMeshProUGUI>().text = $"<color=yellow>[Gain ({timesPassed}) Mysterious Cane";
    }

    public override void Initialize(EventNodeHandler handler)
    {
        base.Initialize(handler);
        if (currentCharacter.CurrentHealth <= passDamage)
            myButtons[1].enabled = false;
    }

    public void TakeCane()
    {
        AddAugmentToPlayer(augmentsToAdd[0], timesPassed);
        IteratePlayerReference();
        StartCoroutine(SelectionChosen());
    }

    public void PassCane()
    {
        currentCharacter.TakeDamage(passDamage, true);
        timesPassed++;
        myButtons[0].GetComponentInChildren<TextMeshProUGUI>().text = $"<color=yellow>[Gain ({timesPassed}) Mysterious Cane";

        IteratePlayerReference();
        currentTurns++;
        if (currentCharacter.CurrentHealth <= passDamage || currentTurns >= 3)
            myButtons[1].enabled = false;
        AutoSelectNextButton();
    }
}
