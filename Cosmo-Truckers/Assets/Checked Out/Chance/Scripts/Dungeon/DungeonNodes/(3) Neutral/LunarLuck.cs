using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LunarLuck : NCNodePopUpOptions
{
    public string dilemmaHeader = "What door will you open??";

    public override void SetUp(DebuffStackSO[] augs)
    {
        StartCoroutine(TextWait(augs));
    }


    protected override void OnButtonClick(DebuffStackSO augToAdd)
    {

        allPlayersSorted[0].AddDebuffStack(augToAdd);

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

        GameObject buttonOne = Instantiate(buttonToAdd, buttonLocation.transform);
        buttonOne.GetComponentInChildren<TMP_Text>().text = $"Open door one";
        buttonOne.GetComponent<Button>().onClick.AddListener(delegate { OnButtonClick(augs[Random.Range(0, augs.Length)]); });

        buttonOne.transform.localScale = Vector3.one;


        GameObject buttonTwo = Instantiate(buttonToAdd, buttonLocation.transform);
        buttonTwo.GetComponentInChildren<TMP_Text>().text = $"Open door two";
        buttonTwo.GetComponent<Button>().onClick.AddListener(delegate { OnButtonClick(augs[Random.Range(0, augs.Length)]); });

        buttonTwo.transform.localScale = Vector3.one;
    }
}
