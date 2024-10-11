using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CosmicCandyBowl : EventNodeBase
{
    Dictionary<int, int> randomIndeces = new Dictionary<int, int>();

    protected override void Start()
    {
        base.Start();
        SetupDictionary();
    }

    private void SetupDictionary()
    {
        int random = Random.Range(0, augmentsToAdd.Length);
        myButtons[randomIndeces.Count].GetComponentInChildren<TextMeshProUGUI>().text = $"<color=red>Gain (1) {augmentsToAdd[random].DebuffName}</color>";
        randomIndeces.Add(randomIndeces.Count, random);

        while(randomIndeces.Count < 4)
        {
            while(randomIndeces.ContainsValue(random))
                random = Random.Range(0, augmentsToAdd.Length);

            myButtons[randomIndeces.Count].GetComponentInChildren<TextMeshProUGUI>().text = $"<color=red>Gain (1) {augmentsToAdd[random].DebuffName}</color>";
            randomIndeces.Add(randomIndeces.Count, random);
        }
    }

    public void EatCandy(int buttonID)
    {
        AddAugmentToPlayer(augmentsToAdd[randomIndeces[buttonID]]);
        MultiplayerSelection(buttonID);
        CheckEndEvent();
    }
}
