using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CrustyCorridor : NCNodePopUpOptions
{
    public string dilemmaHeader = "Vote on a direction to go. . .";
    public int damage = 30;
    int[] votes = new int[2];

    public override void SetUp(DebuffStackSO[] augs)
    {
        StartCoroutine(TextWait(augs));
    }

    void OnButtonClick(int loc, GameObject button, DebuffStackSO[] aug)
    {
        votes[loc]++;

        button.GetComponentInChildren<TMP_Text>().text = loc == 0 ? $"Go to the LEFT  <--- \n{votes[0]}" : $"Go to the RIGHT ---> \n{votes[1]}";


        allPlayersSorted.RemoveAt(0);

        if (allPlayersSorted.Count > 0)
            ShowPlayerName(allPlayersSorted[0].CharacterName);
        else
            VotingOver(aug);
    }

    void VotingOver(DebuffStackSO[] aug)
    {
        int bestOption = UnityEngine.Random.Range(0, 2);
        bool success = false;

        if ((bestOption == 0 && votes[0] > votes[1]) || (bestOption == 1 && votes[0] < votes[1]))
        {
            success = true;

            foreach (var player in EnemyManager.Instance.Players)
            {
                player.AddDebuffStack(aug[0], 3);
                player.AddDebuffStack(aug[1], 3);
            }
        }
        else
        {
            foreach (var player in EnemyManager.Instance.Players)
            {
                player.TakeDamage(damage);
            }
        }

        StartCoroutine(EndWait(success));
    }

    IEnumerator EndWait(bool success)
    {
        currentPlayer.text = success ? "You navigated the corridor!" : "Seems you got lost";

        yield return new WaitForSeconds(2.0f);

        Destroy(this.gameObject);
    }

    IEnumerator TextWait(DebuffStackSO[] augs)
    {
        currentPlayer.text = dilemmaHeader;

        yield return new WaitForSeconds(2.0f);

        base.SetUp(augs);

        GameObject leftButton = Instantiate(buttonToAdd, buttonLocation.transform);
        leftButton.GetComponentInChildren<TMP_Text>().text = $"Go to the LEFT  <--- \n{votes[0]}";
        leftButton.GetComponent<Button>().onClick.AddListener(delegate { OnButtonClick(0, leftButton, augs); });

        leftButton.transform.localScale = Vector3.one;


        GameObject rightButton = Instantiate(buttonToAdd, buttonLocation.transform);
        rightButton.GetComponentInChildren<TMP_Text>().text = $"Go to the RIGHT ---> \n{votes[1]}";
        rightButton.GetComponent<Button>().onClick.AddListener(delegate { OnButtonClick(1, rightButton, augs); });

        rightButton.transform.localScale = Vector3.one;
    }
}
