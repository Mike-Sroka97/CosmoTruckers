using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WishingStar : EventNodeBase
{
    [SerializeField] int minStatAdjustment = 10;
    [SerializeField] int maxStatAdjustment = 25;
    [SerializeField] string[] stats;

    List<int> randomStatList = new List<int>();
    int statAdjustmentOne;
    int statAdjustmentTwo;
    int statAdjustmentThree;
    int statAdjustmentFour;

    protected override void Start()
    {
        base.Start();
        GenerateStatCombos();
    }

    private void GenerateStatCombos()
    {
        while(randomStatList.Count < 4)
        {
            int random = Random.Range(0, 4);

            while (randomStatList.Contains(random))
                random = Random.Range(0, 4);

            randomStatList.Add(random);
        }

        //Generate the four stat adjustments
        statAdjustmentOne = Random.Range(minStatAdjustment, maxStatAdjustment + 1);
        statAdjustmentTwo = Random.Range(minStatAdjustment, maxStatAdjustment + 1);
        statAdjustmentThree = Random.Range(minStatAdjustment, maxStatAdjustment + 1);
        statAdjustmentFour = Random.Range(minStatAdjustment, maxStatAdjustment + 1);

        myButtons[0].GetComponentInChildren<TextMeshProUGUI>().text = $"<color=green>[Gain ({statAdjustmentOne}%) {stats[randomStatList[0]]}] <color=red>[Gain -({statAdjustmentTwo}%) {stats[randomStatList[1]]}]";
        myButtons[1].GetComponentInChildren<TextMeshProUGUI>().text = $"<color=green>[Gain ({statAdjustmentThree}%) {stats[randomStatList[2]]}] <color=red>[Gain -({statAdjustmentFour}%) {stats[randomStatList[3]]}]";
    }

    public void WishOne()
    {
        HandleStatAdjustments(randomStatList[0], statAdjustmentOne);
        HandleStatAdjustments(randomStatList[1], -statAdjustmentTwo);

        MultiplayerSelection(0);
        CheckEndEvent();
    }

    public void WishTwo()
    {
        HandleStatAdjustments(randomStatList[2], statAdjustmentThree);
        HandleStatAdjustments(randomStatList[3], -statAdjustmentFour);

        MultiplayerSelection(1);
        CheckEndEvent();
    }

    private void HandleStatAdjustments(int stat, int amount)
    {
        switch (stat)
        {
            case 0:
                currentCharacter.AdjustDefense(amount);
                break;
            case 1:
                currentCharacter.AdjustVigor(amount);
                break;
            case 2:
                currentCharacter.AdjustSpeed(amount);
                break;
            case 3:
                currentCharacter.AdjustDamage(amount);
                break;
            case 4:
                currentCharacter.AdjustRestoration(amount);
                break;
            default:
                break;
        }
    }
}
