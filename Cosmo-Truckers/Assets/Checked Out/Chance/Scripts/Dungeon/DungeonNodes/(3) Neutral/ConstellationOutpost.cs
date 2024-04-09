using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ConstellationOutpost : NCNodePopUpOptions
{
    int[] votes;
    bool[] voteChoice;

    Dictionary<PlayerCharacter, int> playerVotes;

    int toFall;

    public override void SetUp(DebuffStackSO[] augs)
    {
        StartCoroutine(TextWait(augs));
    }

    void OnButtonClick(int loc, GameObject button, GameObject buttonTwo, DebuffStackSO[] aug)
    {
        votes[loc - 1]++;

        playerVotes.Add(allPlayersSorted[0], loc);

        int totalVotes = loc > 3 ? (votes[loc -1 ] + votes[loc - 2]) : (votes[loc - 1] + votes[loc + 2]);

        int realLoc = loc > 3 ? loc - 3 : loc;

        button.GetComponentInChildren<TMP_Text>().text = $"Constelation {realLoc} Safe \nVotes: {totalVotes}";
        buttonTwo.GetComponentInChildren<TMP_Text>().text = $"Constelation {realLoc} Risky \nVotes: {totalVotes}";


        allPlayersSorted.RemoveAt(0);

        if (allPlayersSorted.Count > 0)
            ShowPlayerName(allPlayersSorted[0].CharacterName);
        else
            VotingOver(aug);
    }

    void VotingOver(DebuffStackSO[] aug)
    {
        foreach(var player in playerVotes)
        {
            //Guessed right safe
            if(player.Value == toFall)
            {
                player.Key.AddDebuffStack(aug[toFall - 1]);
            }
            //Guessed right risky
            else if(player.Value - 3 == toFall)
            {
                player.Key.AddDebuffStack(aug[toFall - 1], 3);
            }
            //Guessed wrong risky
            else if(player.Value > 3)
            {
                player.Key.AddDebuffStack(aug[toFall + 2], 3);
            }
            //Guessed wrong safe
            else
            {
                player.Key.AddDebuffStack(aug[toFall + 2]);
            }
        }

        Destroy(this.gameObject);
    }

    IEnumerator TextWait(DebuffStackSO[] augs)
    {
        toFall = UnityEngine.Random.Range(1, 4);

        Debug.Log($"Falling {toFall}");

        currentPlayer.text = "Choose a constelation. . .";

        yield return new WaitForSeconds(2.0f);

        base.SetUp(augs);

        playerVotes = new Dictionary<PlayerCharacter, int>();

        votes = new int[6];

        for (int i = 1; i < 4; i++)
        {
            int location = i;

            GameObject button = Instantiate(buttonToAdd);
            button.transform.SetParent(buttonLocation.transform);

            button.GetComponentInChildren<TMP_Text>().text = $"Constelation {location} Safe \nVotes: {votes[i - 1] + votes[i + 2]}";

            button.transform.localScale = Vector3.one;

            GameObject buttonTwo = Instantiate(buttonToAdd);
            buttonTwo.transform.SetParent(buttonLocation.transform);
            buttonTwo.GetComponentInChildren<TMP_Text>().text = $"Constelation {location} Risky \nVotes: {votes[i - 1] + votes[i + 2]}";

            buttonTwo.transform.localScale = Vector3.one;

            button.GetComponent<Button>().onClick.AddListener(delegate { OnButtonClick(location, button, buttonTwo, augs); });
            buttonTwo.GetComponent<Button>().onClick.AddListener(delegate { OnButtonClick(location + 3, button, buttonTwo, augs); });
        }
    }
}
