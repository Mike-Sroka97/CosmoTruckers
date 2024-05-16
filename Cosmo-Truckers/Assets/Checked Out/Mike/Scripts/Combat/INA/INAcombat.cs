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
    [SerializeField] GameObject DungeonGen;
    [SerializeField] float screenOpenSpeed;
    [SerializeField] float screenGoalDistance;
    [SerializeField] Transform aboveMask;
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
    [HideInInspector] public bool HoldCountDown;

    [Space(20)]
    [Header("Face Variables")]
    [SerializeField] float faceWaitTime = 1f;
    [SerializeField] Animator face;

    string goText = "GO!";
    const float INAoffset = -0.5f;

    Vector3 startingPosition;
    float topMaskStartingY;
    float bottomMaskStartingY;

    private void Start()
    {
        HoldCountDown = false;
        startingPosition = transform.position;
        topMaskStartingY = topMask.localPosition.y;
        bottomMaskStartingY = bottomMask.localPosition.y;

        if (CombatData.Instance.TESTING)
        {
            DungeonGen.SetActive(false);
            EnemyManager.Instance.InitializeEnemys();
        }
        else
        {
            StartCoroutine(MoveINADungeon(true));
        }
    }

    public void CloseDungeonPage()
    {
        StartCoroutine(MoveINADungeon(false));
    }

    public void OpenDungeonPage()
    {
        StartCoroutine(MoveINADungeon(true));
    }

    IEnumerator MoveINADungeon(bool moveUp)
    {
        countDownTimer.enabled = false;
        timer.enabled = false;

        if (moveUp)
        {
            face.gameObject.SetActive(false);

            //Wait a frame to fix renderer threading issues
            yield return null;

            //Move up
            while (transform.position.y < goalPosition.y)
            {
                transform.position += new Vector3(0, moveSpeed * Time.deltaTime, 0);
                yield return null;
            }

            transform.localPosition = goalPosition;

            yield return new WaitForSeconds(faceWaitTime);

            //OpenScreen
            while (topMask.localPosition.y < topMaskStartingY + screenGoalDistance)
            {
                topMask.localPosition += new Vector3(0, screenOpenSpeed * Time.deltaTime, 0);
                bottomMask.localPosition -= new Vector3(0, screenOpenSpeed * Time.deltaTime, 0);

                yield return null;
            }

            topMask.localPosition = new Vector3(0, topMaskStartingY + screenGoalDistance, 0);
            bottomMask.localPosition = new Vector3(0, bottomMaskStartingY - screenGoalDistance, 0);

            DungeonGen.SetActive(true);
            FindObjectOfType<DungeonGen>().RegenMap();
        }
        else
        {
            face.gameObject.SetActive(false);

            DungeonGen.SetActive(false);

            //CloseScreen
            while (topMask.localPosition.y > topMaskStartingY)
            {
                topMask.localPosition -= new Vector3(0, screenOpenSpeed * Time.deltaTime, 0);
                bottomMask.localPosition += new Vector3(0, screenOpenSpeed * Time.deltaTime, 0);

                yield return null;
            }

            topMask.localPosition = new Vector3(0, topMaskStartingY, 0);
            bottomMask.localPosition = new Vector3(0, bottomMaskStartingY, 0);

            //Move Down
            while (transform.position.y > startingPosition.y)
            {
                transform.position -= new Vector3(0, moveSpeed * Time.deltaTime, 0);
                yield return null;
            }
            transform.localPosition = startingPosition;
        }

        countDownTimer.enabled = false;
    }

    public IEnumerator MoveINACombat(bool moveUp)
    {
        countDownTimer.enabled = false;
        timer.enabled = false;

        if (moveUp)
        {
            aboveMask.gameObject.SetActive(true);

            //Move Vessels out of the way blud
            StartCoroutine(PlayerVesselManager.Instance.MoveMe(!moveUp));
            foreach (PlayerVessel playerVessel in PlayerVesselManager.Instance.PlayerVessels)
                playerVessel.UpdateHealthText();

            //Generate Face
            face.gameObject.SetActive(true);
            AnimationClip randomFace = face.runtimeAnimatorController.animationClips[Random.Range(0, face.runtimeAnimatorController.animationClips.Length)];
            face.Play(randomFace.name);

            //Wait a frame to fix renderer threading issues
            yield return null; 

            CombatMove minigame = GetComponentInChildren<CombatMove>(); 

            //Disable Line/Trail Renderers
            foreach (LineRenderer line in minigame.GetComponentsInChildren<LineRenderer>())
                line.enabled = false;

            foreach (TrailRenderer trail in minigame.GetComponentsInChildren<TrailRenderer>())
                trail.enabled = false;

            //Move up
            while (transform.position.y < goalPosition.y)
            {
                transform.position += new Vector3(0, moveSpeed * Time.deltaTime, 0);
                yield return null;
            }

            //Enable Line/Trail Renderers
            foreach (LineRenderer line in GetComponentsInChildren<LineRenderer>())
                line.enabled = true;

            foreach (TrailRenderer trail in GetComponentsInChildren<TrailRenderer>())
                trail.enabled = true;

            transform.localPosition = goalPosition;

            yield return new WaitForSeconds(faceWaitTime);

            CombatManager.Instance.SpawnPlayers();
            GetComponentInChildren<CombatMove>().SetSpawns();

            //OpenScreen
            while (topMask.localPosition.y < topMaskStartingY + screenGoalDistance)
            {
                topMask.localPosition += new Vector3(0, screenOpenSpeed * Time.deltaTime, 0);
                bottomMask.localPosition -= new Vector3(0, screenOpenSpeed * Time.deltaTime, 0);

                yield return null;
            }

            face.gameObject.SetActive(false);

            topMask.localPosition = new Vector3(0, topMaskStartingY + screenGoalDistance, 0);
            bottomMask.localPosition = new Vector3(0, bottomMaskStartingY - screenGoalDistance, 0);

            //Timer
            countDownTimer.enabled = true;
            countDownTimer.color = new Color(1, 1, 1, 1);
            countDownTimer.transform.localScale = Vector3.one;

            float currentTime = maxTime;

            while (HoldCountDown)
                yield return null;

            while (currentTime > 0)
            {
                currentTime -= Time.deltaTime;
                int intTime = (int)currentTime;
                intTime++;
                countDownTimer.text = intTime.ToString();

                yield return null;
            }

            //Shake
            string text;
            if (minigame.GoTextReplacement != "")
            {
                text = minigame.GoTextReplacement.ToUpper();
                if (text[text.Length - 1] != '!')
                    text += "!";
            }

            else
                text = goText;

            countDownTimer.text = text;
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
            aboveMask.gameObject.SetActive(false);

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

            //Move Vessels into the way blud
            StartCoroutine(PlayerVesselManager.Instance.MoveMe(!moveUp));

            StartCoroutine(CombatManager.Instance.EndCombat());
        }

        countDownTimer.enabled = false;
        CombatManager.Instance.PauseManager = false;
    }
}
