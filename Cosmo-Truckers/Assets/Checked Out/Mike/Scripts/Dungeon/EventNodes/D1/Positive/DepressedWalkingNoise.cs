using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DepressedWalkingNoise : EventNodeBase
{
    [SerializeField] float waitTime = .2f;
    [SerializeField] string failureText;
    [SerializeField] TextMeshProUGUI descriptionText;
    [SerializeField] string description;

    TextMeshProUGUI buttonText;
    int timmysNumber = 1;
    int lastNumber;

    protected override void Start()
    {
        base.Start();
        buttonText = myButtons[0].GetComponentInChildren<TextMeshProUGUI>();
        timmysNumber = Random.Range(1, 11); //1-10
        descriptionText.text = $"{description} <color=green>{timmysNumber}</color>.";
        buttonText.text = timmysNumber.ToString();
        StartCoroutine(ChangeNumber());
    }

    public void GuessNumber()
    { 
        if(lastNumber == timmysNumber)
        {
            StopAllCoroutines();
            currentCharacter.RemoveAmountOfAugments(0, 999);
            buttonText.text = $"<color=green>Timmy fully cleanses {currentCharacter.CharacterName}";
            StartCoroutine(SelectionChosen());
        }
        else
        {
            StopAllCoroutines();
            IgnoreOption();
        }
    }

    protected override void CheckEndEvent()
    {
        currentTurns++;

        if (currentTurns > 3)
        {
            buttonText.text = failureText;
            StartCoroutine(SelectionChosen());
        }
    }

    IEnumerator ChangeNumber()
    {
        //update number
        buttonText.text = lastNumber.ToString();
        int textNumber = int.Parse(buttonText.text);
        textNumber++;
        if (textNumber > 10)
            textNumber = 1;

        lastNumber = textNumber;
        //set text
        if (textNumber == timmysNumber)
            buttonText.text = $"<color=green>{textNumber}";
        else
            buttonText.text = textNumber.ToString();

        //reset
        yield return new WaitForSeconds(waitTime);
        StartCoroutine(ChangeNumber());
    }
}
