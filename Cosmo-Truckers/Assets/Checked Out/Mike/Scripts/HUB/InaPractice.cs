using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InaPractice : INAcombat
{
    [Header("CharacterSelectStuffs")]
    [SerializeField] GameObject characterSelectGo;
    [SerializeField] TextMeshProUGUI[] characterSelectTMPs;
    [SerializeField] string[] characterSelectTextLines;
    [SerializeField] float characterWaitTime;
    [SerializeField] GameObject[] characterSelectButtonGOs;
    [SerializeField] float characterSelectButtonsWaitTime;
    [SerializeField] GameObject[] trainees;

    [Header("CharacterSelectStuffs")]
    [Space(20)]
    [SerializeField] GameObject minigameCharacterSelectGo;

    AutoSelectMeButton firstCharacterButton;
    GameObject currentTrainee;
    [HideInInspector] public HUBController Hub;
    protected override void Start()
    {
        base.Start();

        firstCharacterButton = transform.Find("CharacterSelectStuffs/Buttons/AeglarButton").GetComponent<AutoSelectMeButton>();
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

    public void SetPlayer(int traineeIndex)
    {
        currentTrainee = trainees[traineeIndex];
        Debug.Log("Current trainee = " + currentTrainee.name);
        characterSelectGo.SetActive(false);
        minigameCharacterSelectGo.SetActive(true);
        //TODO open next Ina screen
    }
}
