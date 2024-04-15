using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MagicHat : NCNodePopUpOptions
{
    public string header = "One of you must reach into the Magic Hat. . .";

    public override void SetUp(DebuffStackSO[] augs)
    {
        StartCoroutine(TextWait(augs));
    }

    void OnButtonClick(bool taken, DebuffStackSO[] augs)
    {
        if (taken)
        {
            int randomChance = Random.Range(0, 100);

            if(randomChance <= 35)
                allPlayersSorted[0].AddDebuffStack(augs[0], 1);
            else if(randomChance <= 70)
                allPlayersSorted[0].AddDebuffStack(augs[1], 1);
            else
                allPlayersSorted[0].AddDebuffStack(augs[2], 1);
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
        yesButton.GetComponentInChildren<TMPro.TMP_Text>().text = "Reach into the hat";


        GameObject noButton = Instantiate(buttonToAdd);
        noButton.transform.SetParent(buttonLocation.transform);

        noButton.transform.localScale = Vector3.one;

        noButton.GetComponent<Button>().onClick.AddListener(delegate { OnButtonClick(false, augs); });
        noButton.GetComponentInChildren<TMPro.TMP_Text>().text = "No thanks";
    }
}
