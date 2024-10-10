using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LoonasDiscovery : EventNodeBase
{
    [SerializeField] int remainingStacks = 5;
    TextMeshProUGUI textToAdjust;
    string buttonString;
    int[] playerVotes = new int[4];

    protected override void Start()
    {
        base.Start();
        textToAdjust = myButtons[0].GetComponentInChildren<TextMeshProUGUI>();
        buttonString = $"Make a discovery. ({remainingStacks}) left.\n<color=green>[Gain (1) Solar Shock]</color>";
        textToAdjust.text = buttonString;
    }

    public void SparkOfDiscovery()
    {
        //reduce remaining stacks
        remainingStacks--;

        //make funny string
        buttonString = $"Make a discovery. ({remainingStacks}) left.\n<color=green>[Gain (1) Solar Shock]</color>";
        textToAdjust.text = buttonString;

        //add augment
        AddAugmentToPlayer(augmentsToAdd[0]);

        //add to vote
        playerVotes[nodeHandler.Player]++;

        //end minigame
        if (remainingStacks == 0)
        {
            IteratePlayerReference();
            StartCoroutine(SelectionChosen());
        }
    }
    public void Pass(int buttonID = 1)
    {
        IteratePlayerReference();
        AutoSelectNextButton();
        currentTurns++;

        if (currentTurns == 3)
            myButtons[buttonID].enabled = false;
    }

    protected override IEnumerator SelectionChosen()
    {
        int playerWithMostVotes = 0;

        for(int i = 0; i < playerVotes.Length; i++)
        {
            if (playerVotes[i] > playerVotes[playerWithMostVotes])
                playerWithMostVotes = i;
        }

        //if (EnemyManager.Instance.PlayerSummons.Count >= EnemyManager.Instance.Players.Count || CheckForSummon(allPlayersSorted[mostVotes]))
        //    NoSummonForYou();
        //else
        //    SummonForYou(mostVotes);

        return base.SelectionChosen();
    }
}
