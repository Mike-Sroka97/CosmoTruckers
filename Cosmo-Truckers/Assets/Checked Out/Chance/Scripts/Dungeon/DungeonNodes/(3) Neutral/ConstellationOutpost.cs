using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ConstellationOutpost : NCNodePopUpOptions
{
    [SerializeField] string message = "Will this constellation be seen??";
    [SerializeField] Image goodAug;
    [SerializeField] Image badAug;

    [Space(10)]
    [SerializeField] float fastRotSpeed = 10;
    [SerializeField] float slowRotSpeed = 5;

    [SerializeField] Button safeBetButton;
    [SerializeField] Image safeBetRotate;
    [SerializeField] Button riskyBetButton;
    [SerializeField] Image riskyBetRotate;

    int toFall;

    public override void SetUp(DebuffStackSO[] augs)
    {
        StartCoroutine(TextWait(augs));
    }

    void OnButtonClick(GameObject toRotate, DebuffStackSO augWin, DebuffStackSO augLoss, bool safeBet)
    {
        safeBetButton.interactable = false;
        riskyBetButton.interactable = false;

        StartCoroutine(SpinWheel(toRotate, augWin, augLoss, safeBet));
    }

    IEnumerator SpinWheel(GameObject toRotate, DebuffStackSO augWin, DebuffStackSO augLoss, bool safeBet)
    {
        float randomFastSpinTime = UnityEngine.Random.Range(1.0f, 3.0f);
        float randomSlowSpinTime = UnityEngine.Random.Range(1.0f, 3.0f);
        float reallySlowTime = 1.0f;

        while(randomFastSpinTime > 0)
        {
            randomFastSpinTime -= Time.deltaTime;

            toRotate.transform.Rotate(0, 0, fastRotSpeed);

            yield return null;
        }

        while (randomSlowSpinTime > 0)
        {
            randomSlowSpinTime -= Time.deltaTime;

            toRotate.transform.Rotate(0, 0, slowRotSpeed);

            yield return null;
        }

        while (reallySlowTime > 0)
        {
            reallySlowTime -= Time.deltaTime;

            toRotate.transform.Rotate(0, 0, 1);

            yield return null;
        }

        Debug.Log(toRotate.transform.eulerAngles.z);

        //Equals success for safe bet and fail for risky
        if (toRotate.transform.eulerAngles.z >= 0 && toRotate.transform.eulerAngles.z <= 235)
        {
            if(safeBet)
            {
                Debug.Log("Success");
                allPlayersSorted[0].AddDebuffStack(augWin);
            }
            else
            {
                Debug.Log("Fail");
                allPlayersSorted[0].AddDebuffStack(augLoss, 3);
            }
        }
        else
        {
            if(safeBet)
            {
                Debug.Log("Fail");
                allPlayersSorted[0].AddDebuffStack(augLoss);
            }
            else
            {
                Debug.Log("Success");
                allPlayersSorted[0].AddDebuffStack(augWin, 3);
            }
        }

        yield return new WaitForSeconds(2.0f);

        toRotate.transform.rotation = Quaternion.identity;

        safeBetButton.interactable = true;
        riskyBetButton.interactable = true;
        allPlayersSorted.RemoveAt(0);

        if (allPlayersSorted.Count > 0)
            ShowPlayerName(allPlayersSorted[0].CharacterName);
        else
            Destroy(this.gameObject);
    }

    IEnumerator TextWait(DebuffStackSO[] augs)
    {
        toFall = UnityEngine.Random.Range(0, 3);

        goodAug.sprite = augs[toFall].AugmentSprite;
        badAug.sprite = augs[toFall + 3].AugmentSprite;

        Debug.Log($"Falling {toFall}");

        currentPlayer.text = message;

        yield return new WaitForSeconds(2.0f);

        base.SetUp(augs);

        safeBetButton.onClick.AddListener(delegate { OnButtonClick(safeBetRotate.gameObject, augs[toFall], augs[toFall + 3], true); });
        riskyBetButton.onClick.AddListener(delegate { OnButtonClick(riskyBetRotate.gameObject, augs[toFall], augs[toFall + 3], false); });
    }
}
