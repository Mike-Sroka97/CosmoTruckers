using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour
{
    [Header("Main Text Components")]
    [SerializeField] private TMP_Text textBox;
    [SerializeField] private TextMeshProUGUI dialogText;
    [SerializeField] private TextMeshProUGUI displayNameText;

    [Header("Dialog Box UI")]
    [SerializeField] private Transform dialogBox;
    [SerializeField] private Image dialogBoxImage; 
    [SerializeField] private Image dialogBoxImageBorder; 
    [SerializeField] private Image actorDirection; 
    [SerializeField] private Image nextLineIndicator; 
    [SerializeField] private float disableUITime = 1f;

    private DialogTextAnimations dialogTextAnimations;
    private string currentLine; 

    public bool DialogIsPlaying { get; private set; }
    public bool UpdatingDialogBox { get; private set; }

    private bool animatingDialogBox;
    private bool boxIsActive = false; 

    #region Setup
    private void SetDialogAnimator() // Set the new Dialog Animatior here
    {
        dialogTextAnimations = new DialogTextAnimations(textBox, nextLineIndicator);
    }

    private void SetDialogPosition(Transform newPosition)
    {
        dialogBox.position = newPosition.position;
    }

    private IEnumerator AnimateUIToSize(float timeToAnimate = 0.5f, float minVal = 0.1f, float maxVal = 1f, bool grow = true)
    {
        if (!boxIsActive && grow)
        {
            boxIsActive = true; 
            SetUIBoxActiveStates(true);
        }

        if (!grow)
        {
            float tempVal = minVal;
            minVal = maxVal; 
            maxVal = tempVal;
        }

        Vector3 startScale = new Vector3(minVal, minVal, minVal);
        Vector3 endScale = new Vector3(maxVal, maxVal, maxVal);
        float newAlpha = 1f; 

        dialogBox.localScale = startScale;
        float timer = 0;

        // Maybe rewrite this weird conditional
        while ((grow && dialogBox.localScale.x < maxVal) || (!grow && dialogBox.localScale.x > maxVal))
        {
            timer += Time.deltaTime;
            dialogBox.localScale = Vector3.Lerp(startScale, endScale, timer / timeToAnimate);
            newAlpha = Mathf.Lerp(minVal, maxVal, timer / timeToAnimate);
            UpdateUIBoxColors(newAlpha);
            yield return null;
        }

        dialogBox.localScale = endScale;
        animatingDialogBox = false;

        if (boxIsActive && !grow)
        {
            boxIsActive = false;
            SetUIBoxActiveStates(false);
        }
    }

    private void SetUIBoxActiveStates(bool state)
    {
        dialogBoxImage.gameObject.SetActive(state);
        dialogBoxImageBorder.gameObject.SetActive(state);
        actorDirection.gameObject.SetActive(state);
    }

    private void UpdateUIBoxColors(float alpha)
    {
        dialogBoxImage.color = new Color(dialogBoxImage.color.r, dialogBoxImage.color.g, dialogBoxImage.color.b, alpha); 
        dialogBoxImageBorder.color = new Color(dialogBoxImageBorder.color.r, dialogBoxImageBorder.color.g, dialogBoxImageBorder.color.b, alpha); 
        actorDirection.color = new Color(actorDirection.color.r, actorDirection.color.g, actorDirection.color.b, alpha);
    }

    #endregion

    #region Dialog Mode
    public IEnumerator StartNextDialog(string nextLine, string actorName, Material borderMaterial, 
        Transform textBoxPosition, bool sameSpeaker = false, bool firstDialog = false, float waitTimeBetweenDialogs = 0.5f)
    {
        // If it's the first time dialog, don't animate out and in, just in
        if (firstDialog)
        {
            UpdatingDialogBox = true;

            SetDialogPosition(textBoxPosition);
            ClearDialogText(); 

            animatingDialogBox = true;
            StartCoroutine(AnimateUIToSize());

            while (animatingDialogBox)
                yield return null;

            UpdatingDialogBox = false;

            // We can use tags to alter this
            yield return new WaitForSeconds(waitTimeBetweenDialogs);
        }
        //If it's not the same speaker, animate out and in to new speaker
        else if (!sameSpeaker)
        {
            UpdatingDialogBox = true; 

            animatingDialogBox = true;
            StartCoroutine(AnimateUIToSize(grow:false));

            while (animatingDialogBox)
                yield return null;

            // Wait until text box is shrunk before moving positions
            SetDialogPosition(textBoxPosition);
            ClearDialogText();

            animatingDialogBox = true;
            StartCoroutine(AnimateUIToSize());

            while (animatingDialogBox)
                yield return null;

            UpdatingDialogBox = false;

            // We can use tags to alter this
            yield return new WaitForSeconds(waitTimeBetweenDialogs);
        }
        else 
        { 
            animatingDialogBox = false;
            UpdatingDialogBox = false;
        }

        SetDialogAnimator();
        displayNameText.text = actorName;
        SpeakNextLine(nextLine);
    }

    private Coroutine lineRoutine = null;
    private void SpeakNextLine(string nextLine)
    {
        // Stop the Coroutine
        this.EnsureCoroutineStopped(ref lineRoutine);
        dialogTextAnimations.isTextAnimating = false;

        List<DialogCommand> commands = DialogUtility.ProcessMessage(nextLine, out string processedMessage);
        lineRoutine = StartCoroutine(dialogTextAnimations.AnimateTextIn(commands, processedMessage, null));
    }
    
    public IEnumerator EndDialog()
    {
        if (boxIsActive)
        {
            animatingDialogBox = true;
            StartCoroutine(AnimateUIToSize(grow:false));
        }
        else { animatingDialogBox = false; }

        while (animatingDialogBox)
            yield return null;

        ClearDialogText();
        SetUIBoxActiveStates(false); 

        yield return new WaitForSeconds(disableUITime);

        DialogIsPlaying = false;
    }
    #endregion

    private void ClearDialogText()
    {
        dialogText.text = string.Empty;
        displayNameText.text = string.Empty;
        
        if (dialogTextAnimations != null)
        {
            dialogTextAnimations.ClearText();
        }
    }
    public bool CheckIfDialogAnimating()
    {
        return dialogTextAnimations.isTextAnimating;
    }
    public void StopAnimating()
    {
        dialogTextAnimations.FinishCurrentAnimation();
    }
}
