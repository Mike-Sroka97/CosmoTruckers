using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StarCandy : NCNodePopUpOptions
{
    int[] votes;
    List<PlayerCharacter> players;
    [SerializeField] int HealingAmount = 4; 

    public override void SetUp(DebuffStackSO[] augs)
    {
        base.SetUp(augs);
        players = new List<PlayerCharacter>(allPlayersSorted);

        votes = new int[players.Count];

        for (int i = 0; i < players.Count; i++)
        {
            GameObject button = Instantiate(buttonToAdd);
            button.transform.SetParent(buttonLocation.transform);

            button.GetComponentInChildren<TMP_Text>().text = $"{players[i].CharacterName}\nVotes: {votes[i]}";

            int location = i;
            button.GetComponent<Button>().onClick.AddListener(delegate { OnButtonClick(location, button); });

            button.transform.localScale = Vector3.one;
        }
    }

    void OnButtonClick(int loc, GameObject button)
    {
        votes[loc]++;
        button.GetComponentInChildren<TMP_Text>().text = $"{players[loc].CharacterName}\nVotes: {votes[loc]}";


        allPlayersSorted.RemoveAt(0);

        if (allPlayersSorted.Count > 0)
            ShowPlayerName(allPlayersSorted[0].CharacterName);
        else
            VotingOver();
    }

    void VotingOver()
    {
        for (int i = 0; i < players.Count; i++)
        {
            for(int j = 0; j < votes[i]; i++)
            {
                players[i].TakeHealing(HealingAmount, true);
            }
        }

        Destroy(this.gameObject);
    }
}
