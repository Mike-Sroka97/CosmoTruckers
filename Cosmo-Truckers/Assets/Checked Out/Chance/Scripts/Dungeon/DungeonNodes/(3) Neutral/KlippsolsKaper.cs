
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class KlippsolsKaper : NCNodePopUpOptions
{
    public string dilemmaHeader = "Choice your fate. . .";
    int[] votes = new int[4];
    string[] outComes = new string[4];

    public override void SetUp(DebuffStackSO[] augs)
    {
        StartCoroutine(TextWait(augs));
    }

    void OnButtonClick(int loc, GameObject button, DebuffStackSO[] aug)
    {
        votes[loc]++;

        button.GetComponentInChildren<TMP_Text>().text = $"{outComes[loc]}\n{votes[loc]}";

        allPlayersSorted.RemoveAt(0);

        if (allPlayersSorted.Count > 0)
            ShowPlayerName(allPlayersSorted[0].CharacterName);
        else
            VotingOver(aug);
    }

    void VotingOver(DebuffStackSO[] aug)
    {
        int bestOption = Random.Range(0, 4);
        bool success = false;

        int highVote = 0;
        int voteLoc = 0;
        for (int i = 0; i < votes.Length; i++)
        {
            highVote = System.Math.Max(highVote, votes[i]);
            if (highVote == votes[i])
                voteLoc = i;
        }

        if (bestOption == voteLoc)
        {
            success = true;

            foreach (var player in EnemyManager.Instance.Players)
                player.RemoveAmountOfAugments(1, 0);
        }
        else
        {
            foreach (var player in EnemyManager.Instance.Players)
                player.AddDebuffStack(aug[0], 1);
        }

        StartCoroutine(EndWait(success));
    }

    IEnumerator EndWait(bool success)
    {
        currentPlayer.text = success ? "You managed to avoid the Kaper" : "Seems Klippsol has tricked you";

        yield return new WaitForSeconds(2.0f);

        Destroy(this.gameObject);
    }

    IEnumerator TextWait(DebuffStackSO[] augs)
    {
        currentPlayer.text = dilemmaHeader;

        yield return new WaitForSeconds(2.0f);

        base.SetUp(augs);

        for(int i = 0; i < 4; i++)
        {
            int amountOfWords = Random.Range(3, 7);
            for(int words = 0; words < amountOfWords; words++)
            {
                int amountOfLetters = Random.Range(2, 6);
                for(int letters = 0; letters < amountOfLetters; letters++)
                {
                    outComes[i] += System.Convert.ToChar(Random.Range(0, 26) + 65);
                }

                outComes[i] += " ";
            }

            GameObject button = Instantiate(buttonToAdd, buttonLocation.transform);
            button.GetComponentInChildren<TMP_Text>().text = $"{outComes[i]}\n{votes[i]}";

            int loc = i;
            button.GetComponent<Button>().onClick.AddListener(delegate { OnButtonClick(loc, button, augs); });

            button.transform.localScale = Vector3.one;
        }
    }
}
