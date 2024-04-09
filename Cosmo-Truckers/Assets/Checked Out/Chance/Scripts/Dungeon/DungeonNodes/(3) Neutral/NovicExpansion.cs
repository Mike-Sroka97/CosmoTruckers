using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NovicExpansion : NCNodePopUpOptions
{
    public string dilemmaHeader = "Will you expand your mind??";

    public override void SetUp(DebuffStackSO[] augs)
    {
        StartCoroutine(TextWait());
    }


    protected override void OnButtonClick(DebuffStackSO augToAdd)
    {
        allPlayersSorted[0].ProliferateAugment(3, 1);
        allPlayersSorted[0].ProliferateAugment(3, 0);

        allPlayersSorted.RemoveAt(0);


        if (allPlayersSorted.Count > 0)
            ShowPlayerName(allPlayersSorted[0].CharacterName);
        else
            Destroy(this.gameObject);
    }

    void NoButtonClick()
    {
        Debug.Log($"{allPlayersSorted[0].CharacterName} declined");

        allPlayersSorted.RemoveAt(0);


        if (allPlayersSorted.Count > 0)
            ShowPlayerName(allPlayersSorted[0].CharacterName);
        else
            Destroy(this.gameObject);
    }

    IEnumerator TextWait()
    {
        currentPlayer.text = dilemmaHeader;

        yield return new WaitForSeconds(2.0f);

        base.SetUp(null);

        GameObject yesButton = Instantiate(buttonToAdd, buttonLocation.transform);
        yesButton.GetComponentInChildren<TMP_Text>().text = $"Accept";
        yesButton.GetComponent<Button>().onClick.AddListener(delegate { OnButtonClick(null); });

        yesButton.transform.localScale = Vector3.one;


        GameObject noButton = Instantiate(buttonToAdd, buttonLocation.transform);
        noButton.GetComponentInChildren<TMP_Text>().text = $"Decline";
        noButton.GetComponent<Button>().onClick.AddListener(delegate { NoButtonClick(); });

        noButton.transform.localScale = Vector3.one;
    }
}
