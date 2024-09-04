using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RecklessConductorAhoy : NCNodePopUpOptions
{
    public string dilemmaHeader = "Will you surrender half your HP to skip the next dungeon??";
    int[] votes = new int[2];

    public override void SetUp(DebuffStackSO[] augs)
    {
        StartCoroutine(TextWait(augs));
    }

    void OnButtonClick(int loc, GameObject button)
    {
        votes[loc]++;

        button.GetComponentInChildren<TMP_Text>().text = loc == 0 ? $"Make the sacrifice\n{votes[0]}" : $"Don't make the sacrifice\n{votes[1]}";


        allPlayersSorted.RemoveAt(0);

        if (allPlayersSorted.Count > 0)
            ShowPlayerName(allPlayersSorted[0].CharacterName);
        else
            VotingOver();
    }

    void VotingOver()
    {
        bool taken = false;

        if (votes[0] > votes[1])
        {
            taken = true;

            foreach (var player in EnemyManager.Instance.Players)
            {
                if(player.CurrentHealth - (player.Health / 2) <= 0)
                    player.TakeDamage(player.CurrentHealth - 1, true);
                else
                    player.TakeDamage(player.Health / 2, true);
            }
        }

        StartCoroutine(EndWait(taken));
    }

    IEnumerator EndWait(bool taken)
    {
        currentPlayer.text = taken ? "You have sacrificed your life to move forward" : "You have taken fate into your own hands";

        yield return new WaitForSeconds(2.0f);

        Destroy(this.gameObject);
    }

    IEnumerator TextWait(DebuffStackSO[] augs)
    {
        currentPlayer.text = dilemmaHeader;

        yield return new WaitForSeconds(2.0f);

        base.SetUp(augs);

        GameObject leftButton = Instantiate(buttonToAdd, buttonLocation.transform);
        leftButton.GetComponentInChildren<TMP_Text>().text = $"Make the sacrifice\n{votes[0]}";
        leftButton.GetComponent<Button>().onClick.AddListener(delegate { OnButtonClick(0, leftButton); });

        leftButton.transform.localScale = Vector3.one;


        GameObject rightButton = Instantiate(buttonToAdd, buttonLocation.transform);
        rightButton.GetComponentInChildren<TMP_Text>().text = $"Don't make the sacrifice\n{votes[1]}";
        rightButton.GetComponent<Button>().onClick.AddListener(delegate { OnButtonClick(1, rightButton); });

        rightButton.transform.localScale = Vector3.one;
    }
}
