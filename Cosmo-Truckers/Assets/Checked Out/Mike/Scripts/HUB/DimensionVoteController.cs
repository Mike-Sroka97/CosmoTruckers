using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DimensionVoteController : MonoBehaviour
{
    [SerializeField] GameObject confirmationScreen;
    [SerializeField] GameObject voteScreen;

    int lastVote;
    Dictionary<int, int> dimensionVotes = new Dictionary<int, int>();
    const string confirmationString = "Are you sure you want to enter the ";

    private void Start()
    {
        for(int i = 1; i <= 9; i++)
            dimensionVotes.Add(i, 0);
    }

    public void OpenConfirmationMenu(int voteValue, string dimensionName)
    {
        //Count vote
        dimensionVotes[voteValue]++;
        lastVote = voteValue;

        voteScreen.SetActive(false);
        confirmationScreen.SetActive(true);
        confirmationScreen.transform.Find("ConfirmationText").GetComponent<TextMeshProUGUI>().text = confirmationString + dimensionName + "?";
    }

    public void CloseConfirmationMenu()
    {
        //Revoke democracy from those who are indecisive
        dimensionVotes[lastVote]--;

        voteScreen.SetActive(true);
        confirmationScreen.SetActive(false);
    }
}
