using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class INAcombat : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    [SerializeField] Vector3 goalPosition;

    [Space(20)]
    [Header("Screen Variables")]
    [SerializeField] float screenOpenSpeed;
    [SerializeField] float screenGoalDistance;
    [SerializeField] Transform topMask;
    [SerializeField] Transform bottomMask;

    [Space(20)]
    [Header("Timer Variables")]
    [SerializeField] TextMeshProUGUI countDownTimer;
    [SerializeField] TextMeshProUGUI timer;
    [SerializeField] int maxTime = 3;
    [SerializeField] int shakeDuration;
    [SerializeField] float shakeSpeedX;
    [SerializeField] float shakeOffsetX;
    [SerializeField] float shakeSpeedY;
    [SerializeField] float shakeOffsetY;

    const string goText = "GO!";
    const float INAoffset = -0.5f;

    Vector3 startingPosition;
    float topMaskStartingY;
    float bottomMaskStartingY;

    private void Start()
    {
        startingPosition = transform.position;
        topMaskStartingY = topMask.localPosition.y;
        bottomMaskStartingY = bottomMask.localPosition.y;
    }

    public IEnumerator MoveINA(bool moveUp)
    {
        countDownTimer.enabled = false;
        timer.enabled = false;

        if (moveUp)
        {
            //Move up
            while(transform.position.y < goalPosition.y)
            {
                transform.position += new Vector3(0, moveSpeed * Time.deltaTime, 0);
                yield return null;
            }
            transform.localPosition = goalPosition;
            CombatManager.Instance.SpawnPlayers();
            FindObjectOfType<CombatMove>().SetSpawns();

            //OpenScreen
            while (topMask.localPosition.y < topMaskStartingY + screenGoalDistance)
            {
                topMask.localPosition += new Vector3(0, screenOpenSpeed * Time.deltaTime, 0);
                bottomMask.localPosition -= new Vector3(0, screenOpenSpeed * Time.deltaTime, 0);

                yield return null;
            }

            topMask.localPosition = new Vector3(0, topMaskStartingY + screenGoalDistance, 0);
            bottomMask.localPosition = new Vector3(0, bottomMaskStartingY - screenGoalDistance, 0);

            //Timer
            countDownTimer.enabled = true;
            countDownTimer.color = new Color(1, 1, 1, 1);
            countDownTimer.transform.localScale = Vector3.one;

            float currentTime = maxTime;

            while (currentTime > 0)
            {
                currentTime -= Time.deltaTime;
                int intTime = (int)currentTime;
                intTime++;
                countDownTimer.text = intTime.ToString();

                yield return null;
            }

            //Shake
            countDownTimer.text = goText;
            currentTime = shakeDuration;

            while (currentTime > 0)
            {
                countDownTimer.transform.position = new Vector3(Mathf.Sin(Time.time * shakeSpeedX) * shakeOffsetX, INAoffset + (Mathf.Sin(Time.time * shakeSpeedY) * shakeOffsetY), 0);
                currentTime -= Time.deltaTime;
                countDownTimer.color = new Color(1, 1, 1, currentTime);
                countDownTimer.transform.localScale = new Vector3(1 + (1 - currentTime), 1 + (1 - currentTime), 1 + (1 - currentTime));

                yield return null;
            }

            FindObjectOfType<CombatMove>().StartMove();

            timer.enabled = true;
        }
        else
        {
            //CloseScreen

            while (topMask.localPosition.y > topMaskStartingY)
            {
                topMask.localPosition -= new Vector3(0, screenOpenSpeed * Time.deltaTime, 0);
                bottomMask.localPosition += new Vector3(0, screenOpenSpeed * Time.deltaTime, 0);

                yield return null;
            }

            CombatManager.Instance.CleanupMinigame();

            topMask.localPosition = new Vector3(0, topMaskStartingY, 0);
            bottomMask.localPosition = new Vector3(0, bottomMaskStartingY, 0);

            //Move Down
            while (transform.position.y > startingPosition.y)
            {
                transform.position -= new Vector3(0, moveSpeed * Time.deltaTime, 0);
                yield return null;
            }
            transform.localPosition = startingPosition;

            StartCoroutine(CombatManager.Instance.EndCombat());
        }

        countDownTimer.enabled = false;
        CombatManager.Instance.INAmoving = false;
    }
}
