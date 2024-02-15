using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DepressedWalkingNoise : NCNodePopUpOptions
{
    int currentAnswer;
    
    public override void SetUp(DebuffStackSO[] augs)
    {
        base.SetUp(augs);

        StartCoroutine(AskQuestion("Guess a number. . ."));
    }

    IEnumerator AskQuestion(string toSay)
    {
        currentPlayer.text = toSay;

        yield return new WaitForSeconds(2.0f);

        ShowPlayerName(allPlayersSorted[0].CharacterName);

        currentAnswer = Random.Range(1, 11);

        for (int i = 1; i < 11; i++)
        {
            GameObject button = Instantiate(buttonToAdd, buttonLocation.transform);
            button.GetComponentInChildren<TMP_Text>().text = i.ToString();

            int vote = i;
            button.GetComponentInChildren<Button>().onClick.AddListener(delegate { OnButtonClick(vote); });

            button.transform.localScale = Vector3.one;
        }
    }

    void OnButtonClick(int vote)
    {
        foreach(Transform child in buttonLocation.transform)
            Destroy(child.gameObject);

        string response;

        if(vote == currentAnswer)
        {
            allPlayersSorted[0].RemoveAmountOfAugments(999, 0);
            response = "Correct! Good job!";
        }
        else
            response = "Aww man. . . To bad. . .";

        allPlayersSorted.RemoveAt(0);

        if (allPlayersSorted.Count > 0)
            StartCoroutine(AskQuestion(response));
        else
            StartCoroutine(DelayRemove(response));
    }

    IEnumerator DelayRemove(string response)
    {
        currentPlayer.text = response;

        yield return new WaitForSeconds(2.0f);

        Destroy(this.gameObject);
    }
}
