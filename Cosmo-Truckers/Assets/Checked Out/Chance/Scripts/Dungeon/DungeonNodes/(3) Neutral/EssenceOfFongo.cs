using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EssenceOfFongoOBSOLETE : NCNodePopUpOptions
{
    public string dilemmaHeader = "Will you accept the Essence??";

    public override void SetUp(DebuffStackSO[] augs)
    {
        StartCoroutine(TextWait(augs));
    }


    protected override void OnButtonClick(DebuffStackSO augToAdd)
    {

        allPlayersSorted[0].RandomizeStats(20, 2, 2);

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

    IEnumerator TextWait(DebuffStackSO[] augs)
    {
        currentPlayer.text = dilemmaHeader;

        yield return new WaitForSeconds(2.0f);

        base.SetUp(augs);

        GameObject yesButton = Instantiate(buttonToAdd, buttonLocation.transform);
        yesButton.GetComponentInChildren<TMP_Text>().text = $"Accept";
        yesButton.GetComponent<Button>().onClick.AddListener(delegate { OnButtonClick(augs[0]); });

        yesButton.transform.localScale = Vector3.one;


        GameObject noButton = Instantiate(buttonToAdd, buttonLocation.transform);
        noButton.GetComponentInChildren<TMP_Text>().text = $"Decline";
        noButton.GetComponent<Button>().onClick.AddListener(delegate { NoButtonClick(); });

        noButton.transform.localScale = Vector3.one;
    }
}
