using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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

    [Space(20)]
    [Header("EnemySprites")] //ID starts with 0 for dimension 1 feeble foe 1. +8 to id based on dimension ID
    [SerializeField] Sprite[] enemySprites;
    [SerializeField] string[] enemyNames;
    [SerializeField] Sprite[] otherSprites;
    [SerializeField] string[] otherNames;

    AutoSelectMeButton firstCharacterButton;
    AutoSelectMeButton firstMinigameCharacterButton;
    AutoSelectMeButton firstEnemySelectButton;
    GameObject currentTrainee;
    [HideInInspector] public HUBController Hub;
    int traineeID;
    string traineeName;
    int dimensionID;
    bool printingString;
    protected override void Start()
    {
        base.Start();

        firstCharacterButton = transform.Find("CharacterSelectStuffs/Buttons/AeglarButton").GetComponent<AutoSelectMeButton>();
        firstMinigameCharacterButton = transform.Find("MinigameCharacterSelect/Buttons/MinigameCharacterButton").GetComponent<AutoSelectMeButton>();
        firstEnemySelectButton = transform.Find("EnemySelectStuffs/Buttons/ReturnButton").GetComponent<AutoSelectMeButton>();
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
        for(int i = 0; i < characterSelectTMPs.Length; i++)
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
    /// Setups up the necessary data for dimension buttons
    /// </summary>
    /// <param name="buttonID"></param>
    private void SetupEnemyButton(int buttonID)
    {
        enemySelectButtons[buttonID].SetActive(true);
        enemySelectButtons[buttonID].GetComponent<TrainingButtonInfo>().CharacterName = enemyNames[buttonID - 1 + getEnemyIdModifier];
        enemySelectButtons[buttonID].transform.Find("Mask/GameObject").GetComponent<Image>().sprite = enemySprites[buttonID - 1 + getEnemyIdModifier];
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

        //Screen 5 - Minigame Select

        //Set all screens off
        SetAllScreensDisabled();
    }

    private void SetAllScreensDisabled()
    {
        characterSelectGo.SetActive(false);
        minigameCharacterSelectGo.SetActive(false);
    }    

    public void SetCharacterSelectScreen()
    {
        CleanUp();
        characterSelectGo.SetActive(true);
        StartCoroutine(PrintCharacterSelectText());
    }

    public void SetPlayer(int traineeIndex, string characterName)
    {
        traineeID = traineeIndex;
        traineeName = characterName;
        currentTrainee = trainees[traineeIndex];
        SetMinigameCharacterSelectScreen();
    }

    public void SetMinigameCharacterSelectScreen()
    {
        CleanUp();
        StartCoroutine(PrintMinigameCharacterSelect());
    }

    public void SetEnemySelectScreen(int dimension)
    {
        CleanUp();
        dimensionID = dimension;
        StartCoroutine(PrintEnemySelect());
    }

    public void TypeCharacterName(string characterName)
    {
        StartCoroutine(PrintString(characterName, characterNameText));
    }

    public void TypeEnemyName(string enemyName)
    {
        StartCoroutine(PrintString(enemyName, enemyNameText, true));
    }

    private IEnumerator PrintString(string stringToPrint, TextMeshProUGUI textElementToPrintTo, bool replaceSpaces = false)
    {
        printingString = true;

        textElementToPrintTo.text = "";

        if(replaceSpaces)
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
}
