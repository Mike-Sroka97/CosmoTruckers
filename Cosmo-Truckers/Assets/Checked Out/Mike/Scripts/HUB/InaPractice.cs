using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.Video;

public class InaPractice : INAcombat
{
    [Space(100)]
    [Header("TrainingInaStuffs")]

    [Space(40)]
    [Header("CharacterSelectStuffs")]
    [SerializeField] GameObject characterSelectGo;
    [SerializeField] TextMeshProUGUI[] characterSelectTMPs;
    [SerializeField] string[] characterSelectTextLines;
    [SerializeField] float characterWaitTime;
    [SerializeField] GameObject[] characterSelectButtonGOs;
    [SerializeField] float characterSelectButtonsWaitTime;
    [SerializeField] GameObject[] trainees;
    [SerializeField] TextMeshProUGUI characterNameText;

    [Space(40)]
    [Header("CharacterSelectStuffs")]
    [SerializeField] GameObject minigameCharacterSelectGo;
    [SerializeField] string minigameCharacterSelectText;
    [SerializeField] TextMeshProUGUI minigameCharacterSelectTMP;
    [SerializeField] GameObject[] minigameCharacterSelectButtonGOs;
    [SerializeField] float minigameCharacterSelectButtonsWaitTime = 0.1f;
    [SerializeField] Sprite[] characterImages;

    [Space(40)]
    [Header("EnemySelectStuffs")]
    [SerializeField] GameObject enemySelectGo;
    [SerializeField] TextMeshProUGUI[] enemySelectTMPs;
    [SerializeField] string[] enemySelectTextLines;
    [SerializeField] GameObject[] enemySelectButtons;
    [SerializeField] TextMeshProUGUI enemyNameText;

    [Space(40)]
    [Header("OtherEnemySelectStuffs")]
    [SerializeField] GameObject otherEnemySelectGo;
    [SerializeField] TextMeshProUGUI[] otherEnemySelectTMPs;
    [SerializeField] string[] otherEnemySelectTextLines;
    [SerializeField] GameObject[] otherEnemySelectButtons;
    [SerializeField] TextMeshProUGUI otherEnemyNameText;
    [SerializeField] string[] maliceMemes;

    [Space(40)]
    [Header("MinigameSelectStuffs")]
    [SerializeField] GameObject minigameSelectGo;
    [SerializeField] string minigameSelectTitleText;
    [SerializeField] TextMeshProUGUI minigameSelectTitle;
    [SerializeField] TextMeshProUGUI minigameSelectMinigameDescription;
    [SerializeField] TextMeshProUGUI minigameSelectMinigameName;
    [SerializeField] GameObject[] minigameSelectButtons;
    [SerializeField] TextMeshProUGUI[] miniGameCountTmps;
    [SerializeField] VideoPlayer videoPlayer;
    [SerializeField] float endMinigameTime = 1.2f;

    [Space(20)]
    [Header("EnemySprites")] //ID starts with 0 for dimension 1 feeble foe 1. +8 to id based on dimension ID
    [SerializeField] Sprite[] enemySprites;
    [SerializeField] string[] enemyNames;
    [SerializeField] GameObject[] playerPrefabs;
    [SerializeField] GameObject[] enemyPrefabs;
    [SerializeField] GameObject[] otherPrefabs;

    AutoSelectMeButton firstCharacterButton;
    AutoSelectMeButton firstMinigameCharacterButton;
    AutoSelectMeButton firstEnemySelectButton;
    AutoSelectMeButton firstOtherEnemySelectButton;
    AutoSelectMeButton firstMinigameSelectButton;
    GameObject currentTrainee;
    [HideInInspector] public HUBController Hub;
    [HideInInspector] public BaseAttackSO CurrentAttack => currentCharacterAttacks[currentMinigameIndex];
    public Material PracticeMaterial;
    int traineeID;
    int enemyID;
    int otherID;
    string traineeName;
    int dimensionID;
    bool printingString;
    int inCharacterMinigameSelect; //0 == player, 1 == enemy, 2 == other
    int currentMinigameIndex = 0;
    List<BaseAttackSO> currentCharacterAttacks = new List<BaseAttackSO>();
    TrainingCombatHandler trainingCombatHandler;
    TextMeshProUGUI successText;
    protected override void Start()
    {
        base.Start();

        firstCharacterButton = transform.Find("CharacterSelectStuffs/Buttons/AeglarButton").GetComponent<AutoSelectMeButton>();
        firstMinigameCharacterButton = transform.Find("MinigameCharacterSelect/Buttons/MinigameCharacterButton").GetComponent<AutoSelectMeButton>();
        firstEnemySelectButton = transform.Find("EnemySelectStuffs/Buttons/ReturnButton").GetComponent<AutoSelectMeButton>();
        firstOtherEnemySelectButton = transform.Find("OtherEnemySelectStuffs/Buttons/ReturnButton").GetComponent<AutoSelectMeButton>();
        firstMinigameSelectButton = transform.Find("MinigameSelectStuff/Buttons/Start").GetComponent<AutoSelectMeButton>();

        trainingCombatHandler = GetComponent<TrainingCombatHandler>();

        topMask.localPosition = new Vector3(0, topMaskStartingY + screenGoalDistance, 0);
        bottomMask.localPosition = new Vector3(0, bottomMaskStartingY - screenGoalDistance, 0);

        successText = transform.Find("Score Text").GetComponent<TextMeshProUGUI>();
    }

    public void StartPracticeShutDown()
    {
        StartCoroutine(MoveINACombat(false));
    }

    /// <summary>
    /// Handles moving and setup/cleanup for training ina
    /// </summary>
    /// <param name="moveUp"></param>
    /// <returns></returns>
    public override IEnumerator MoveINACombat(bool moveUp)
    {
        if (moveUp)
        {
            characterSelectGo.SetActive(true);
            aboveMask.gameObject.SetActive(true);

            //Move up
            while (transform.localPosition.y < goalPosition.y)
            {
                transform.position += new Vector3(0, moveSpeed * Time.deltaTime, 0);
                yield return null;
            }

            transform.localPosition = goalPosition;

            StartCoroutine(PrintCharacterSelectText());
        }
        else
        {
            firstCharacterButton.enabled = false;
            aboveMask.gameObject.SetActive(false);

            //Move Down
            while (transform.localPosition.y > startingPosition.y)
            {
                transform.position -= new Vector3(0, moveSpeed * Time.deltaTime, 0);
                yield return null;
            }

            CleanUp();
            transform.localPosition = startingPosition;
            Hub.Training(false);
        }
    }

    /// <summary>
    /// pretty setup for character select screen
    /// </summary>
    /// <returns></returns>
    private IEnumerator PrintCharacterSelectText()
    {
        for (int i = 0; i < characterSelectTMPs.Length; i++)
        {
            int currentCharacter = 0;

            while (currentCharacter < characterSelectTextLines[i].Length)
            {
                characterSelectTMPs[i].text += characterSelectTextLines[i][currentCharacter];

                currentCharacter++;

                yield return new WaitForSeconds(characterWaitTime);
            }
        }

        yield return new WaitForSeconds(characterSelectButtonsWaitTime);

        foreach (GameObject characterSelectButton in characterSelectButtonGOs)
        {
            characterSelectButton.SetActive(true);
            yield return new WaitForSeconds(characterSelectButtonsWaitTime);
        }

        firstCharacterButton.enabled = true;
    }

    /// <summary>
    /// Pretty setup for minigame character select screen
    /// </summary>
    /// <returns></returns>
    private IEnumerator PrintMinigameCharacterSelect()
    {
        minigameCharacterSelectGo.SetActive(true);

        int currentCharacter = 0;

        while (currentCharacter < minigameCharacterSelectText.Length)
        {
            minigameCharacterSelectTMP.text += minigameCharacterSelectText[currentCharacter];

            currentCharacter++;

            yield return new WaitForSeconds(characterWaitTime);
        }

        yield return new WaitForSeconds(minigameCharacterSelectButtonsWaitTime);

        foreach (GameObject minigameCharacterSelectButton in minigameCharacterSelectButtonGOs)
        {
            minigameCharacterSelectButton.SetActive(true);
            yield return new WaitForSeconds(minigameCharacterSelectButtonsWaitTime);
        }

        //TODO select first button
        firstMinigameCharacterButton.enabled = true;
    }

    /// <summary>
    /// God help me
    /// </summary>
    /// <returns></returns>
    private IEnumerator PrintEnemySelect()
    {
        enemySelectGo.SetActive(true);

        //Print title text
        StartCoroutine(PrintString(enemySelectTextLines[0], enemySelectTMPs[0]));
        while (printingString)
            yield return null;

        //Enable return button
        yield return new WaitForSeconds(minigameCharacterSelectButtonsWaitTime);
        enemySelectButtons[0].SetActive(true);

        //Print Feeble foes and enable buttons
        StartCoroutine(PrintString(enemySelectTextLines[1], enemySelectTMPs[1]));
        while (printingString)
            yield return null;

        yield return new WaitForSeconds(minigameCharacterSelectButtonsWaitTime);
        SetupEnemyButton(1);
        yield return new WaitForSeconds(minigameCharacterSelectButtonsWaitTime);
        SetupEnemyButton(2);
        yield return new WaitForSeconds(minigameCharacterSelectButtonsWaitTime);
        SetupEnemyButton(3);
        yield return new WaitForSeconds(minigameCharacterSelectButtonsWaitTime);
        SetupEnemyButton(4);

        //Print Fierce foes and enable buttons
        StartCoroutine(PrintString(enemySelectTextLines[2], enemySelectTMPs[2]));
        while (printingString)
            yield return null;

        yield return new WaitForSeconds(minigameCharacterSelectButtonsWaitTime);
        SetupEnemyButton(5);
        yield return new WaitForSeconds(minigameCharacterSelectButtonsWaitTime);
        SetupEnemyButton(6);
        yield return new WaitForSeconds(minigameCharacterSelectButtonsWaitTime);
        SetupEnemyButton(7);
        yield return new WaitForSeconds(minigameCharacterSelectButtonsWaitTime);
        SetupEnemyButton(8);

        //Print Minibosses and enable buttons
        StartCoroutine(PrintString(enemySelectTextLines[3], enemySelectTMPs[3]));
        while (printingString)
            yield return null;

        yield return new WaitForSeconds(minigameCharacterSelectButtonsWaitTime);
        SetupEnemyButton(9);
        yield return new WaitForSeconds(minigameCharacterSelectButtonsWaitTime);
        SetupEnemyButton(10);

        //Print Bosses and enable buttons
        StartCoroutine(PrintString(enemySelectTextLines[4], enemySelectTMPs[4]));
        while (printingString)
            yield return null;

        yield return new WaitForSeconds(minigameCharacterSelectButtonsWaitTime);
        SetupEnemyButton(11);
        yield return new WaitForSeconds(minigameCharacterSelectButtonsWaitTime);
        SetupEnemyButton(12);

        //Enable player interaction
        yield return new WaitForSeconds(minigameCharacterSelectButtonsWaitTime);
        firstEnemySelectButton.enabled = true;
    }

    /// <summary>
    /// God help me 2
    /// </summary>
    /// <returns></returns>
    private IEnumerator PrintOtherEnemySelect()
    {
        otherEnemySelectGo.SetActive(true);

        //Print title text
        StartCoroutine(PrintString(otherEnemySelectTextLines[0], otherEnemySelectTMPs[0]));
        while (printingString)
            yield return null;

        //Enable return button
        yield return new WaitForSeconds(minigameCharacterSelectButtonsWaitTime);
        otherEnemySelectButtons[0].SetActive(true);

        //Print all other foes and enable buttons
        StartCoroutine(PrintString(otherEnemySelectTextLines[1], otherEnemySelectTMPs[1]));
        while (printingString)
            yield return null;

        for (int i = 1; i < otherEnemySelectButtons.Length; i++)
        {
            yield return new WaitForSeconds(minigameCharacterSelectButtonsWaitTime);
            otherEnemySelectButtons[i].SetActive(true);
        }

        //Enable player interaction
        yield return new WaitForSeconds(minigameCharacterSelectButtonsWaitTime);
        firstOtherEnemySelectButton.enabled = true;
    }

    /// <summary>
    /// Pretty stuffs for minigame select screen
    /// </summary>
    /// <returns></returns>
    private IEnumerator PrintMinigameSelect(int type)
    {
        minigameSelectGo.SetActive(true);

        //Print title text
        StartCoroutine(PrintString(minigameSelectTitleText, minigameSelectTitle));
        while (printingString)
            yield return null;

        for (int i = 0; i < minigameSelectButtons.Length; i++)
        {
            yield return new WaitForSeconds(minigameCharacterSelectButtonsWaitTime);
            minigameSelectButtons[i].SetActive(true);
        }

        //Enable player interaction
        yield return new WaitForSeconds(minigameCharacterSelectButtonsWaitTime);
        firstMinigameSelectButton.enabled = true;

        //Setup minigame count stuffs
        yield return new WaitForSeconds(minigameCharacterSelectButtonsWaitTime);
        currentMinigameIndex = 0;
        miniGameCountTmps[0].text = "1";
        miniGameCountTmps[1].text = "/";

        //Setup up minigame info based on type
        if (type == 0)
        {
            currentCharacterAttacks = playerPrefabs[traineeID].GetComponent<PlayerCharacter>().GetAllBaseAttacks;
        }
        else if (type == 1)
        {
            currentCharacterAttacks = enemyPrefabs[enemyID].GetComponent<Enemy>().GetAllAttacks.ToList();
        }
        else
        {
            currentCharacterAttacks = otherPrefabs[otherID].GetComponent<Enemy>().GetAllAttacks.ToList();
        }

        List<BaseAttackSO> tempAttacks = new List<BaseAttackSO>();

        foreach (BaseAttackSO attack in currentCharacterAttacks)
            if (!attack.AutoCast)
                tempAttacks.Add(attack);

        currentCharacterAttacks = tempAttacks;

        videoPlayer.targetTexture.Release();
        videoPlayer.targetTexture.Create();

        miniGameCountTmps[2].text = $"{currentCharacterAttacks.Count}";
        SetupMinigameDisplay();
    }

    /// <summary>
    /// Setups up the necessary data for dimension buttons
    /// </summary>
    /// <param name="buttonID"></param>
    private void SetupEnemyButton(int buttonID)
    {
        enemySelectButtons[buttonID].SetActive(true);
        enemySelectButtons[buttonID].GetComponent<TrainingButtonInfo>().CharacterName = enemyNames[buttonID - 1 + getEnemyIdModifier];
        enemySelectButtons[buttonID].GetComponent<TrainingButtonInfo>().EnemyID = buttonID - 1 + getEnemyIdModifier;
        enemySelectButtons[buttonID].transform.Find("Mask/GameObject").GetComponent<Image>().sprite = enemySprites[buttonID - 1 + getEnemyIdModifier];
    }

    //Displays minigame stuffs
    private void SetupMinigameDisplay()
    {
        StartCoroutine(PrintString(currentCharacterAttacks[currentMinigameIndex].AttackName, minigameSelectMinigameName));
        StartCoroutine(PrintString(currentCharacterAttacks[currentMinigameIndex].AttackDescription, minigameSelectMinigameDescription));
        videoPlayer.clip = currentCharacterAttacks[currentMinigameIndex].MinigameDemo;
    }

    /// <summary>
    /// Handles the index of the minigames
    /// </summary>
    /// <param name="increment"></param>
    public void HandleMinigameChange(bool increment)
    {
        StopAllCoroutines();

        if (increment)
        {
            currentMinigameIndex++;
            if (currentMinigameIndex >= currentCharacterAttacks.Count)
                currentMinigameIndex = 0;
        }
        else
        {
            currentMinigameIndex--;
            if (currentMinigameIndex < 0)
                currentMinigameIndex = currentCharacterAttacks.Count - 1;
        }

        miniGameCountTmps[0].text = $"{currentMinigameIndex + 1}";

        SetupMinigameDisplay();
    }

    /// <summary>
    /// Clean version of accessing your enemy ID mod
    /// </summary>
    private int getEnemyIdModifier => dimensionID * 12;

    /// <summary>
    /// Clears all text field and resets GOs
    /// </summary>
    private void CleanUp()
    {
        //Screen 1 - Character Select
        foreach (TextMeshProUGUI tmp in characterSelectTMPs)
            tmp.text = "";

        characterNameText.text = "";

        foreach (GameObject characterSelectButton in characterSelectButtonGOs)
            characterSelectButton.SetActive(false);

        firstCharacterButton.enabled = false;

        //Screen 2 - Minigame Character Select Primary
        minigameCharacterSelectTMP.text = "";

        foreach (GameObject minigameCharacterButton in minigameCharacterSelectButtonGOs)
            minigameCharacterButton.SetActive(false);

        firstMinigameCharacterButton.enabled = false;

        //Screen 3 - Minigame Character Select Secondary (Dimension)
        foreach (TextMeshProUGUI tmp in enemySelectTMPs)
            tmp.text = "";

        enemyNameText.text = "";

        foreach (GameObject characterSelectButton in enemySelectButtons)
            characterSelectButton.SetActive(false);

        firstEnemySelectButton.enabled = false;

        //Screen 4 - Minigame Character Select Secondary (Other)
        foreach (TextMeshProUGUI tmp in otherEnemySelectTMPs)
            tmp.text = "";

        foreach (GameObject characterSelectButton in otherEnemySelectButtons)
            characterSelectButton.SetActive(false);

        otherEnemyNameText.text = "";

        //Screen 5 - Minigame Select
        minigameSelectTitle.text = "";
        minigameSelectMinigameDescription.text = "";
        minigameSelectMinigameName.text = "";

        foreach (GameObject characterSelectButton in minigameSelectButtons)
            characterSelectButton.SetActive(false);

        foreach (TextMeshProUGUI tmp in miniGameCountTmps)
            tmp.text = "";

        //Set all screens off
        SetAllScreensDisabled();
    }

    /// <summary>
    /// Disables all INA screens
    /// </summary>
    private void SetAllScreensDisabled()
    {
        characterSelectGo.SetActive(false);
        minigameCharacterSelectGo.SetActive(false);
        enemySelectGo.SetActive(false);
        otherEnemySelectGo.SetActive(false);
        minigameSelectGo.SetActive(false);
    }

    /// <summary>
    /// Sets up the character to play as screen
    /// </summary>
    public void SetCharacterSelectScreen()
    {
        CleanUp();
        characterSelectGo.SetActive(true);
        StartCoroutine(PrintCharacterSelectText());
    }

    /// <summary>
    /// Sets player data for minigames
    /// </summary>
    /// <param name="traineeIndex"></param>
    /// <param name="characterName"></param>
    public void SetPlayer(int traineeIndex, string characterName)
    {
        traineeID = traineeIndex;
        traineeName = characterName;
        currentTrainee = trainees[traineeIndex];
        SetMinigameCharacterSelectScreen();
    }

    /// <summary>
    /// Sets up the dimension screen
    /// </summary>
    public void SetMinigameCharacterSelectScreen()
    {
        CleanUp();
        StartCoroutine(PrintMinigameCharacterSelect());
    }

    /// <summary>
    /// Sets up the dimension specific screen
    /// </summary>
    /// <param name="dimension"></param>
    public void SetEnemySelectScreen(int dimension)
    {
        CleanUp();
        dimensionID = dimension;
        StartCoroutine(PrintEnemySelect());
    }

    /// <summary>
    /// Sets up all other enemy selection screen
    /// </summary>
    public void SetOtherEnemySelectScreen()
    {
        CleanUp();
        StartCoroutine(PrintOtherEnemySelect());
    }

    /// <summary>
    /// For UI navigation we need a different method for player minigame setup
    /// </summary>
    public void SetMinigamePlayerSelectScreen()
    {
        CleanUp();
        inCharacterMinigameSelect = 0;
        StartCoroutine(PrintMinigameSelect(0));
    }

    /// <summary>
    /// Minigame select screen setup
    /// </summary>
    public void SetMinigameEnemySelectScreen()
    {
        CleanUp();
        inCharacterMinigameSelect = 1;
        StartCoroutine(PrintMinigameSelect(1));
    }

    /// <summary>
    /// Minigame select if you came from other screen
    /// </summary>
    public void SetMinigameOtherSelectScreen(int otherId = -1)
    {
        if (otherId != -1)
            otherID = otherId;

        CleanUp();
        inCharacterMinigameSelect = 2;
        StartCoroutine(PrintMinigameSelect(2));
    }

    private void ReturnToMinigameSelect()
    {
        CleanUp();
        StartCoroutine(PrintMinigameSelect(inCharacterMinigameSelect));
    }

    /// <summary>
    /// For minigame select return button to return you to the right screen
    /// </summary>
    public void ReturnFromMinigameSelect()
    {
        if (inCharacterMinigameSelect == 0)
            SetMinigameCharacterSelectScreen();
        else if (inCharacterMinigameSelect == 2)
            SetOtherEnemySelectScreen();
        else
            SetEnemySelectScreen(dimensionID);
    }

    public void StartMinigame()
    {
        CleanUp();
        topMask.GetComponent<SpriteMask>().enabled = true;
        bottomMask.GetComponent<SpriteMask>().enabled = true;
        trainingCombatHandler.PopulateMinigameData();
    }

    public void TypeCharacterName(string characterName)
    {
        StartCoroutine(PrintString(characterName, characterNameText));
    }

    public void TypeEnemyName(string enemyName, TextMeshProUGUI tmpToPrint)
    {
        StartCoroutine(PrintString(enemyName, tmpToPrint, true));
    }

    private IEnumerator PrintString(string stringToPrint, TextMeshProUGUI textElementToPrintTo, bool replaceSpaces = false)
    {
        printingString = true;

        textElementToPrintTo.text = "";

        if (replaceSpaces)
            stringToPrint = stringToPrint.Replace(" ", "\n");

        int currentCharacter = 0;

        while (currentCharacter < stringToPrint.Length)
        {
            textElementToPrintTo.text += stringToPrint[currentCharacter];

            currentCharacter++;

            yield return new WaitForSeconds(characterWaitTime);
        }

        printingString = false;
    }

    public void SetPlayerMinigameButton(GameObject characterButton)
    {
        Image characterImage = characterButton.transform.Find("CharacterImage").GetComponent<Image>();
        TextMeshProUGUI textElement = characterButton.transform.Find("GameObject").GetComponent<TextMeshProUGUI>();

        characterImage.sprite = characterImages[traineeID];
        textElement.text = traineeName;
    }

    /// <summary>
    /// Handles strings for selecting Malice in training
    /// </summary>
    public void MaliceMeme()
    {
        string currentMeme = maliceMemes[Random.Range(0, maliceMemes.Length)];

        otherEnemySelectTMPs[0].text = "";
        StartCoroutine(PrintString(currentMeme, otherEnemySelectTMPs[0]));
    }

    public void SetEnemyId(int newEnemyId)
    {
        enemyID = newEnemyId;
    }

    public IEnumerator PrePreMinigameStuffs()
    {
        trainingCombatHandler.PauseCoroutine = true;

        yield return new WaitForSeconds(endMinigameTime);

        topMask.localPosition = new Vector3(0, topMaskStartingY + screenGoalDistance, 0);
        bottomMask.localPosition = new Vector3(0, bottomMaskStartingY - screenGoalDistance, 0);

        //CloseScreen
        while (topMask.localPosition.y > topMaskStartingY)
        {
            topMask.localPosition -= new Vector3(0, screenOpenSpeed * Time.deltaTime, 0);
            bottomMask.localPosition += new Vector3(0, screenOpenSpeed * Time.deltaTime, 0);

            yield return null;
        }

        trainingCombatHandler.PauseCoroutine = false;
    }

    public IEnumerator PreMinigameStuffs()
    {
        trainingCombatHandler.PauseCoroutine = true;

        aboveMask.gameObject.SetActive(true);

        CombatMove minigame = GetComponentInChildren<CombatMove>();

        //Disable Line/Trail Renderers
        foreach (LineRenderer line in minigame.GetComponentsInChildren<LineRenderer>())
            line.enabled = false;

        foreach (TrailRenderer trail in minigame.GetComponentsInChildren<TrailRenderer>())
            trail.enabled = false;

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

        trainingCombatHandler.SpawnPlayers(playerPrefabs[traineeID].GetComponent<PlayerCharacter>());
        GetComponentInChildren<CombatMove>().SetSpawns();

        //OpenScreen
        while (topMask.localPosition.y < topMaskStartingY + screenGoalDistance)
        {
            topMask.localPosition += new Vector3(0, screenOpenSpeed * Time.deltaTime, 0);
            bottomMask.localPosition -= new Vector3(0, screenOpenSpeed * Time.deltaTime, 0);

            yield return null;
        }

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

        GetComponentInChildren<CombatMove>().StartMove();

        timer.enabled = true;

        trainingCombatHandler.PauseCoroutine = false;
    }

    public IEnumerator MinigameCleanup()
    {
        aboveMask.gameObject.SetActive(false);
        FindObjectOfType<Player>().enabled = false;

        yield return new WaitForSeconds(endMinigameTime);

        //CloseScreen
        while (topMask.localPosition.y > topMaskStartingY)
        {
            topMask.localPosition -= new Vector3(0, screenOpenSpeed * Time.deltaTime, 0);
            bottomMask.localPosition += new Vector3(0, screenOpenSpeed * Time.deltaTime, 0);

            yield return null;
        }

        //TODO CUSTOM DISPLAY
        successText.text = trainingCombatHandler.MiniGame.GetComponent<CombatMove>().TrainingDisplayText;
        trainingCombatHandler.CleanupMinigame();

        yield return new WaitForSeconds(endMinigameTime * 3);

        successText.text = "";

        //OpenScreen
        while (topMask.localPosition.y < topMaskStartingY + screenGoalDistance)
        {
            topMask.localPosition += new Vector3(0, screenOpenSpeed * Time.deltaTime, 0);
            bottomMask.localPosition -= new Vector3(0, screenOpenSpeed * Time.deltaTime, 0);

            yield return null;
        }

        topMask.localPosition = new Vector3(0, topMaskStartingY + screenGoalDistance, 0);
        bottomMask.localPosition = new Vector3(0, bottomMaskStartingY - screenGoalDistance, 0);

        ReturnToMinigameSelect();
    }
}
