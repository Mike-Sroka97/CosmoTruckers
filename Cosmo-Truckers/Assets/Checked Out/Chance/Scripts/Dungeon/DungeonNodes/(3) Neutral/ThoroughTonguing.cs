using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ThoroughTonguingOBSOLETE : NCNodePopUpOptions
{
    public string header = "One of you must get tounged, who will it be??";

    public override void SetUp(DebuffStackSO[] augs)
    {
        StartCoroutine(TextWait(augs));
    }

    void OnButtonClick(bool taken, DebuffStackSO[] augs)
    {
        if (taken)
        {
            allPlayersSorted[0].RemoveAmountOfAugments(999, 2);
            allPlayersSorted[0].AddDebuffStack(augs[0], 3);
        }

        allPlayersSorted.RemoveAt(0);

        if (allPlayersSorted.Count <= 0 && !taken)
            base.SetUp(augs);
        else if (taken)
            Destroy(this.gameObject);
        else
            ShowPlayerName(allPlayersSorted[0].CharacterName);
    }

    IEnumerator TextWait(DebuffStackSO[] augs)
    {
        currentPlayer.text = header;

        yield return new WaitForSeconds(2.0f);

        base.SetUp(augs);

        GameObject yesButton = Instantiate(buttonToAdd);
        yesButton.transform.SetParent(buttonLocation.transform);

        yesButton.transform.localScale = Vector3.one;

        yesButton.GetComponent<Button>().onClick.AddListener(delegate { OnButtonClick(true, augs); });
        yesButton.GetComponentInChildren<TMPro.TMP_Text>().text = "Get tongued";


        GameObject noButton = Instantiate(buttonToAdd);
        noButton.transform.SetParent(buttonLocation.transform);

        noButton.transform.localScale = Vector3.one;

        noButton.GetComponent<Button>().onClick.AddListener(delegate { OnButtonClick(false, augs); });
        noButton.GetComponentInChildren<TMPro.TMP_Text>().text = "No thanks";
    }
}
