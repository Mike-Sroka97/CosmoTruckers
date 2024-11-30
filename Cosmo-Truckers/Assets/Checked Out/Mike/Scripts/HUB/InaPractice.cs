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

    AutoSelectMeButton firstCharacterButton;
    AutoSelectMeButton firstMinigameCharacterButton;
    GameObject currentTrainee;
    [HideInInspector] public HUBController Hub;
    int traineeID;
    string traineeName;
    protected override void Start()
    {
        base.Start();

        firstCharacterButton = transform.Find("CharacterSelectStuffs/Buttons/AeglarButton").GetComponent<AutoSelectMeButton>();
        firstMinigameCharacterButton = transform.Find("MinigameCharacterSelect/Buttons/MinigameCharacterButton").GetComponent<AutoSelectMeButton>();
    }

    public void StartPracticeShutDown()
    {
        StartCoroutine(MoveINACombat(false));
    }

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

        //TODO set buttons up

        //TODO select first button
        firstMinigameCharacterButton.enabled = true;
    }

    private void CleanUp()
    {
        foreach (TextMeshProUGUI tmp in characterSelectTMPs)
            tmp.text = "";

        foreach (GameObject characterSelectButton in characterSelectButtonGOs)
            characterSelectButton.SetActive(false);

        SetAllScreensDisabled();
    }

    private void SetAllScreensDisabled()
    {
        characterSelectGo.SetActive(false);
        minigameCharacterSelectGo.SetActive(false);
    }    

    public void SetCharacterSelectScreen()
    {
        SetAllScreensDisabled();
        characterSelectGo.SetActive(true);
        StartCoroutine(PrintCharacterSelectText());
    }

    public void SetPlayer(int traineeIndex, string characterName)
    {
        traineeID = traineeIndex;
        traineeName = characterName;
        currentTrainee = trainees[traineeIndex];
        characterSelectGo.SetActive(false);
        StartCoroutine(PrintMinigameCharacterSelect());
    }

    public void TypeCharacterName(string characterName)
    {
        StartCoroutine(PrintString(characterName, characterNameText));
    }

    private IEnumerator PrintString(string stringToPrint, TextMeshProUGUI textElementToPrintTo)
    {
        textElementToPrintTo.text = "";

        int currentCharacter = 0;

        while (currentCharacter < stringToPrint.Length)
        {
            textElementToPrintTo.text += stringToPrint[currentCharacter];

            currentCharacter++;

            yield return new WaitForSeconds(characterWaitTime);
        }
    }

    public void SetPlayerMinigameButton(GameObject characterButton)
    {
        Image characterImage = characterButton.transform.Find("CharacterImage").GetComponent<Image>();
        TextMeshProUGUI textElement = characterButton.transform.Find("GameObject").GetComponent<TextMeshProUGUI>();

        characterImage.sprite = characterImages[traineeID];
        textElement.text = traineeName;
    }
}
