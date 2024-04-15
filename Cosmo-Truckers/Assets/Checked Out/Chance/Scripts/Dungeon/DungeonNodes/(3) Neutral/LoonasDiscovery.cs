using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoonasDiscovery : NCNodePopUpOptions
{
    public string dilemmaHeader = "Loona has found tiny empty Nova";
    public string dilemmaHeaderTwo = "There is a limit amount of Solar SHOCK to take";
    public int totalStacks = 10;
    [SerializeField] GameObject emptyNova;
    int[] votes;
    

    public override void SetUp(DebuffStackSO[] augs)
    {
        StartCoroutine(TextWait(augs));
    }

    void OnButtonClick(int loc, DebuffStackSO[] aug)
    {
        votes[loc]++;
        totalStacks--;

        allPlayersSorted.RemoveAt(0);

        if (allPlayersSorted.Count <= 0 && totalStacks > 0)
            base.SetUp(aug);
        else if (totalStacks <= 0)
            VotingOver(aug);
        else
            ShowPlayerName(allPlayersSorted[0].CharacterName);
    }

    void VotingOver(DebuffStackSO[] aug)
    {
        base.SetUp(aug);

        int mostVotes = Random.Range(0, allPlayersSorted.Count);

        for(int i = 0; i < allPlayersSorted.Count; i++)
        {
            if(votes[i] > 0)
                allPlayersSorted[i].AddDebuffStack(aug[0], votes[i]);

            if ((votes[i] > votes[mostVotes] && !CheckForSummon(allPlayersSorted[i])) || CheckForSummon(allPlayersSorted[mostVotes]))
                mostVotes = i;
        }

        if (EnemyManager.Instance.PlayerSummons.Count >= EnemyManager.Instance.Players.Count || CheckForSummon(allPlayersSorted[mostVotes]))
            NoSummonForYou();
        else
            SummonForYou(mostVotes);

    }

    void NoSummonForYou()
    {
        StartCoroutine(EndTextWait("There is no room to bring the Nova with you."));
    }

    void SummonForYou(int loc)
    {
        EnemyManager.Instance.UpdatePlayerSummons(emptyNova, allPlayersSorted[loc]);

        StartCoroutine(EndTextWait($"{allPlayersSorted[loc].CharacterName} has a new Empty Nova"));
    }

    IEnumerator EndTextWait(string text)
    {
        currentPlayer.text = text;

        yield return new WaitForSeconds(2.0f);

        Destroy(this.gameObject);
    }
    IEnumerator TextWait(DebuffStackSO[] augs)
    {
        currentPlayer.text = dilemmaHeader;

        yield return new WaitForSeconds(2.0f);

        base.SetUp(augs);

        votes = new int[allPlayersSorted.Count];

        for(int i = 0; i < allPlayersSorted.Count; i++)
        {
            GameObject button = Instantiate(buttonToAdd, buttonLocation.transform);
            button.GetComponentInChildren<TMP_Text>().text = $"Give stack of Solar SHOCK to {allPlayersSorted[i].CharacterName}";

            int location = i;
            button.GetComponent<Button>().onClick.AddListener(delegate { OnButtonClick(location, augs); });

            button.transform.localScale = Vector3.one;
        }
    }

    bool CheckForSummon(PlayerCharacter player)
    {
        foreach (PlayerCharacterSummon sum in EnemyManager.Instance.PlayerSummons)
        {
            if (sum.CombatSpot + 4 == player.CombatSpot)
                return true;
        }

        return false;
    }
}
