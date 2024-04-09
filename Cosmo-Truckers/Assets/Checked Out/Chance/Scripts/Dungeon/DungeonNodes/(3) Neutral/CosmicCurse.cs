using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CosmicCurse : NCNodePopUpOptions
{
    public TMP_Text augPoolText;
    public string dilemmaHeader = "Will you accept the curse??";
    public int totalTimes = 3;
    int currentTimes = 0;


    DebuffStackSO[] augsPool;

    public override void SetUp(DebuffStackSO[] augs)
    {
        StartCoroutine(TextWait(augs));
    }

    //augs 0-2 possivite
    //augs 3-5 neg
    void OnButtonClick(DebuffStackSO[] augs)
    {
        currentTimes++;

        allPlayersSorted[0].AddDebuffStack(augs[Random.Range(0, 3)]);
        allPlayersSorted[0].AddDebuffStack(augs[Random.Range(3, 6)]);

        //Taken max curse stacks
        if (currentTimes == totalTimes)
        {
            NoButtonClick();
        }

    }

    void NoButtonClick()
    {
        currentTimes = 0;
        Debug.Log($"{allPlayersSorted[0].CharacterName} decline");

        allPlayersSorted.RemoveAt(0);


        if (allPlayersSorted.Count > 0)
            ShowPlayerName(allPlayersSorted[0].CharacterName);
        else
            Destroy(this.gameObject);
    }

    //Augs 0-5 possitive augs 6-11 negative
    IEnumerator TextWait(DebuffStackSO[] augs)
    {
        currentPlayer.text = dilemmaHeader;

        augsPool = new DebuffStackSO[6] {
            augs[Random.Range(0, 2)],
            augs[Random.Range(2, 4)],
            augs[Random.Range(4, 6)],
            augs[Random.Range(6, 8)],
            augs[Random.Range(8, 10)],
            augs[Random.Range(10, 12)]
        };

        augPoolText.text = "Current Pool \n";
        for (int i = 0; i < augsPool.Length; i++)
        {
            augPoolText.text += $"{augsPool[i].DebuffName}, ";
        }

        augPoolText.text = augPoolText.text.Substring(0, augPoolText.text.Length - 2);

        yield return new WaitForSeconds(2.0f);

        base.SetUp(augs);

        GameObject yesButton = Instantiate(buttonToAdd, buttonLocation.transform);
        yesButton.GetComponentInChildren<TMP_Text>().text = $"Take the curse";
        yesButton.GetComponent<Button>().onClick.AddListener(delegate { OnButtonClick(augsPool); });

        yesButton.transform.localScale = Vector3.one;


        GameObject noButton = Instantiate(buttonToAdd, buttonLocation.transform);
        noButton.GetComponentInChildren<TMP_Text>().text = $"Decline";
        noButton.GetComponent<Button>().onClick.AddListener(delegate { NoButtonClick(); });

        noButton.transform.localScale = Vector3.one;
    }
}
