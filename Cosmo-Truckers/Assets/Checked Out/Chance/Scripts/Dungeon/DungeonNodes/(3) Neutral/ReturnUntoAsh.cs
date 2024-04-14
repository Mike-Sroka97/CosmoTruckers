using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReturnUntoAsh : NCNodePopUpOptions
{
    public string header = "One of you may die and be reborn. . .";

    public override void SetUp(DebuffStackSO[] augs)
    {
        StartCoroutine(TextWait(augs));
    }

    void OnButtonClick(bool taken)
    {
        if (taken)
        {
            allPlayersSorted[0].RemoveAmountOfAugments(999, 2);
            allPlayersSorted[0].TakeDamage(999, true);
            allPlayersSorted[0].Resurrect(allPlayersSorted[0].Health / 2);
        }

        allPlayersSorted.RemoveAt(0);

        if (allPlayersSorted.Count > 0)
            ShowPlayerName(allPlayersSorted[0].CharacterName);
        else
            Destroy(this.gameObject);
    }

    IEnumerator TextWait(DebuffStackSO[] augs)
    {
        currentPlayer.text = header;

        yield return new WaitForSeconds(2.0f);

        base.SetUp(augs);

        GameObject yesButton = Instantiate(buttonToAdd);
        yesButton.transform.SetParent(buttonLocation.transform);

        yesButton.transform.localScale = Vector3.one;

        yesButton.GetComponent<Button>().onClick.AddListener(delegate { OnButtonClick(true); });
        yesButton.GetComponentInChildren<TMPro.TMP_Text>().text = "Kill your self";


        GameObject noButton = Instantiate(buttonToAdd);
        noButton.transform.SetParent(buttonLocation.transform);

        noButton.transform.localScale = Vector3.one;

        noButton.GetComponent<Button>().onClick.AddListener(delegate { OnButtonClick(false); });
        noButton.GetComponentInChildren<TMPro.TMP_Text>().text = "No thanks";
    }
}
