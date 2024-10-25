using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class INAcombat : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    [SerializeField] Vector3 goalPosition;

    [Space(20)]
    [Header("Screen Variables")]
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
    [HideInInspector] public UnityEvent AttackStarted = new UnityEvent();

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
        startingPosition = transform.localPosition;
        topMaskStartingY = topMask.localPosition.y;
        bottomMaskStartingY = bottomMask.localPosition.y;

        if (CombatData.Instance.TESTING)
        {
            if (EnemyManager.Instance.Enemies.Count <= 0)
                EnemyManager.Instance.InitializeEnemys();
        }
        //else
        //{
        //    StartCoroutine(MoveINADungeon(true));
        //}
    }

    private void OnDisable()
    {
        AttackStarted.RemoveAllListeners();
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
            while (transform.localPosition.y < goalPosition.y)
            {
                transform.position += new Vector3(0, moveSpeed * Time.deltaTime, 0);
                yield return null;
            }

            //Enable Line/Trail Renderers
            foreach (LineRenderer line in GetComponentsInChildren<LineRenderer>())
            {
                if (line.GetComponent<SetupMinigameLineRenderer>() != null)
                    line.GetComponent<SetupMinigameLineRenderer>().SetLineLocalPositions(minigame); 
                else
                    line.enabled = true;
            }

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

            // Set countdown timer text to null to start with
            countDownTimer.text = "";

            // Invoke Attack Started
            AttackStarted.Invoke();

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

            // Set Go Text
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

            //Shake
            currentTime = shakeDuration;

            while (currentTime > 0)
            {
                countDownTimer.transform.localPosition = new Vector3(Mathf.Sin(Time.time * shakeSpeedX) * shakeOffsetX, INAoffset + (Mathf.Sin(Time.time * shakeSpeedY) * shakeOffsetY), 0);
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
            while (transform.localPosition.y > startingPosition.y)
            {
                transform.position -= new Vector3(0, moveSpeed * Time.deltaTime, 0);
                yield return null;
            }
            transform.localPosition = startingPosition;

            //Move Vessels into the way blud
            StartCoroutine(PlayerVesselManager.Instance.MoveMe(!moveUp));

            while (PlayerVesselManager.Instance.IsMoving)
                yield return null;

            StartCoroutine(CombatManager.Instance.EndCombat());
        }

        countDownTimer.enabled = false;
        CombatManager.Instance.PauseManager = false;
    }
}
