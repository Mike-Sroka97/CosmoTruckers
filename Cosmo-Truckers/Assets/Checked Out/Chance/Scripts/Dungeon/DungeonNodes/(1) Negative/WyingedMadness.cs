using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WyingedMadness : NCNodePopUpOptions
{
    [SerializeField] bool topVote = false;

    int[] votes;
    List<PlayerCharacter> players;

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
            button.GetComponent<Button>().onClick.AddListener(delegate { OnButtonClick(location, button, augs[0]); });

            button.transform.localScale = Vector3.one;
        }
    }

    void OnButtonClick(int loc, GameObject button, DebuffStackSO aug)
    {
        votes[loc]++;
        button.GetComponentInChildren<TMP_Text>().text = $"{players[loc].CharacterName}\nVotes: {votes[loc]}";


        allPlayersSorted.RemoveAt(0);

        if (allPlayersSorted.Count > 0)
            ShowPlayerName(allPlayersSorted[0].CharacterName);
        else
            VotingOver(aug);
    }

    void VotingOver(DebuffStackSO aug)
    {
        int highVote = 1;

        if(topVote)
            for (int i = 0; i < votes.Length; i++)
                highVote = Math.Max(highVote, votes[i]);

        for (int i = 0; i < players.Count; i++)
        {
            if (votes[i] >= highVote)
            {
                players[i].AddDebuffStack(aug);
            }
        }

        Destroy(this.gameObject);
    }
}
