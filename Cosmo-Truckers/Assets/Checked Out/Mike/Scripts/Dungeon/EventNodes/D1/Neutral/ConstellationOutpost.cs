using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ConstellationOutpost : EventNodeBase
{
    [SerializeField] int minSafeChance;
    [SerializeField] int maxSafeChance;
    [SerializeField] AugmentStackSO[] victoryAugments;
    [SerializeField] AugmentStackSO[] failureAugments;
    [SerializeField] int riskyAmount = 3;

    int safeChance;
    int riskyChance;

    protected override void Start()
    {
        base.Start();

        safeChance = Random.Range(minSafeChance, maxSafeChance + 1);
        riskyChance = 100 - safeChance; //difference from 100%

        myButtons[0].GetComponentInChildren<TextMeshProUGUI>().text = $"Safe path ({safeChance}%)";
        myButtons[1].GetComponentInChildren<TextMeshProUGUI>().text = $"Risky path ({riskyChance}%)";
    }

    public void SafeBet()
    {
        int random = Random.Range(1, 101);
        
        //success
        if(random <= safeChance)
        {
            foreach (PlayerVessel player in PlayerVesselManager.Instance.PlayerVessels)
                player.MyCharacter.AddAugmentStack(victoryAugments[Random.Range(0, victoryAugments.Length)]);

            myButtons[0].GetComponentInChildren<TextMeshProUGUI>().text = $"<color=green>The low risk path yields rewards";
        }

        //failure
        else
        {
            foreach (PlayerVessel player in PlayerVesselManager.Instance.PlayerVessels)
                player.MyCharacter.AddAugmentStack(failureAugments[Random.Range(0, victoryAugments.Length)]);
            myButtons[0].GetComponentInChildren<TextMeshProUGUI>().text = $"<color=red>The low risk path yields debuffs";
        }


        IteratePlayerReference();

        StartCoroutine(SelectionChosen());
    }

    public void RiskyBet()
    {
        int random = Random.Range(1, 101);

        //success
        if (random <= riskyChance)
        {
            foreach (PlayerVessel player in PlayerVesselManager.Instance.PlayerVessels)
                player.MyCharacter.AddAugmentStack(victoryAugments[Random.Range(0, victoryAugments.Length)], riskyAmount);

            myButtons[1].GetComponentInChildren<TextMeshProUGUI>().text = $"<color=green>The high risk path yields rewards";
        }

        //failure
        else
        {
            foreach (PlayerVessel player in PlayerVesselManager.Instance.PlayerVessels)
                player.MyCharacter.AddAugmentStack(failureAugments[Random.Range(0, victoryAugments.Length)], riskyAmount);

            myButtons[1].GetComponentInChildren<TextMeshProUGUI>().text = $"<color=red>The high risk path yields debuffs";
        }


        IteratePlayerReference();
        StartCoroutine(SelectionChosen());
    }
}
