using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MagicHat : EventNodeBase
{
    int random;
    string color;

    protected override void Start()
    {
        base.Start();
        random = Random.Range(0, 3);
        if (random == 2)
            color = "<color=red>";
        else
            color = "<color=green>";
    }

    public void ReachInside()
    {
        AddAugmentToPlayer(augmentsToAdd[random]);
        AddAugmentToPlayer(augmentsToAdd[random]);
        myButtons[0].GetComponentInChildren<TextMeshProUGUI>().text = $"{color}Gained (1) {augmentsToAdd[random].DebuffName}";
        IteratePlayerReference();
        currentTurns = 4;
        CheckEndEvent();
    }
}
