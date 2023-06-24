using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StartAttack : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI countDownText;
    [SerializeField] float countDown = 1.5f;
    [SerializeField] string[] combatStartLines;

    CombatMove moveToStart;

    private void Start()
    {
        Initialize();
    }
    public void Initialize() //Add params for color flash, screen split, etc
    {
        //set flash color
        //set screens
        StartCoroutine(CountDown(countDown));

    }

    IEnumerator CountDown(float timeToCount)
    {
        countDownText.enabled = true;
        int intTime = (int)timeToCount;

        countDownText.transform.position = Vector3.zero;
        float startX = countDownText.transform.position.x;
        float startY = countDownText.transform.position.y;

        while(timeToCount > -1.5f) //-1.5 so that we can display zero
        {
            timeToCount -= Time.deltaTime;
            if(timeToCount > -0.5f)
            {
                intTime = (int)(timeToCount * 2 + 1); //trust me (this simply makes it count down from 3 even though its 1.5 seconds)
                countDownText.text = intTime.ToString();
            }
            else
            {
                countDownText.transform.position = Vector2.zero + Random.insideUnitCircle * .25f;

                countDownText.fontSize = 20;
                countDownText.text = "GO!";
            }

            yield return new WaitForSeconds(Time.deltaTime);
        }

        countDownText.enabled = false;
        StartMiniGame();
    }

    private void StartMiniGame()
    {
        moveToStart.enabled = true;
    }
}
