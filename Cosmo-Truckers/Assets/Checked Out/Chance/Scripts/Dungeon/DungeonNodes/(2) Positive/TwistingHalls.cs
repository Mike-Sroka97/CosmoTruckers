using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TwistingHalls : NCNodePopUpOptions
{
    int[] votes = new int[2];

    public override void SetUp(DebuffStackSO[] augs)
    {
        base.SetUp(augs);

        GameObject leftButton = Instantiate(buttonToAdd, buttonLocation.transform);
        leftButton.GetComponentInChildren<TMP_Text>().text = $"Go to the LEFT  <--- \n{votes[0]}";
        leftButton.GetComponent<Button>().onClick.AddListener(delegate { OnButtonClick(0, leftButton, augs[0]); });

        leftButton.transform.localScale = Vector3.one;


        GameObject rightButton = Instantiate(buttonToAdd, buttonLocation.transform);
        rightButton.GetComponentInChildren<TMP_Text>().text = $"Go to the RIGHT ---> \n{votes[1]}";
        rightButton.GetComponent<Button>().onClick.AddListener(delegate { OnButtonClick(1, rightButton, augs[0]); });

        rightButton.transform.localScale = Vector3.one;
    }

    void OnButtonClick(int loc, GameObject button, DebuffStackSO aug)
    {
        votes[loc]++;

        button.GetComponentInChildren<TMP_Text>().text = loc == 0 ? $"Go to the LEFT  <--- \n{votes[0]}" : $"Go to the RIGHT ---> \n{votes[1]}";


        allPlayersSorted.RemoveAt(0);

        if (allPlayersSorted.Count > 0)
            ShowPlayerName(allPlayersSorted[0].CharacterName);
        else
            VotingOver(aug);
    }

    void VotingOver(DebuffStackSO aug)
    {
        int bestOption = UnityEngine.Random.Range(0, 2);

        if(votes[0] != votes[1])
        {
            if((bestOption == 0 && votes[0] > votes[1]) || (bestOption == 1 && votes[0] < votes[1]))
            {
                foreach (var player in EnemyManager.Instance.Players)
                {
                    player.AddDebuffStack(aug, 1);
                }
            }
        }

        Destroy(this.gameObject);
    }
}
