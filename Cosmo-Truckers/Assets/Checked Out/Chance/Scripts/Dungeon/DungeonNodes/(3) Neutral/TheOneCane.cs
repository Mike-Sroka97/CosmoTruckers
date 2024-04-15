using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TheOneCane : NCNodePopUpOptions
{
    public string header = "Will you touch the cane or pass it on??";
    public int damage = 10;

    public float timeToPass = 45.0f;
    public TMPro.TMP_Text timer;

    bool passing = true;
    int passes = 1;

    public override void SetUp(DebuffStackSO[] augs)
    {
        StartCoroutine(TextWait(augs));
        StartCoroutine(CountDown(augs));
    }

    void OnButtonClick(bool taken, DebuffStackSO[] augs)
    {
        if (taken)
        {
            passing = false;

            allPlayersSorted[0].AddDebuffStack(augs[0], passes);
        }
        else
        {
            passes++;

            passes = Math.Clamp(passes, 1, 4);

            allPlayersSorted[0].TakeDamage(damage);
        }

        allPlayersSorted.RemoveAt(0);

        if (allPlayersSorted.Count <= 0 && !taken)
            base.SetUp(augs);
        else if (taken)
            Destroy(this.gameObject);
        else
            ShowPlayerName(allPlayersSorted[0].CharacterName);
    }

    IEnumerator CountDown(DebuffStackSO[] augs)
    {
        while(passing && timeToPass > 0)
        {
            timeToPass -= Time.deltaTime;
            timer.text = ((int)timeToPass).ToString();

            yield return null;
        }

        if(passing)
        {
            base.SetUp(augs);

            allPlayersSorted[UnityEngine.Random.Range(0, allPlayersSorted.Count)].AddDebuffStack(augs[0], passes);
            Destroy(this.gameObject);
        }
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
        yesButton.GetComponentInChildren<TMPro.TMP_Text>().text = "Touch the cane";


        GameObject noButton = Instantiate(buttonToAdd);
        noButton.transform.SetParent(buttonLocation.transform);

        noButton.transform.localScale = Vector3.one;

        noButton.GetComponent<Button>().onClick.AddListener(delegate { OnButtonClick(false, augs); });
        noButton.GetComponentInChildren<TMPro.TMP_Text>().text = "Pass the cane";
    }
}
