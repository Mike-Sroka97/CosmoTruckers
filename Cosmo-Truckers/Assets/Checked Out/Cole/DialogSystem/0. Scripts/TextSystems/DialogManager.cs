using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour
{
    [Header("Main Text Components")]
    Transform dialogBox; 
    [SerializeField] private TMP_Text textBox;
    [SerializeField] private TextMeshProUGUI dialogText;
    [SerializeField] private TextMeshProUGUI displayNameText;

    [Header("Dialog UI")]
    [SerializeField] private Image nextLineIndicator; 
    [SerializeField] private float disableUITime = 1f;

    private DialogAnimations dialogAnimations;
    private string currentLine; 

    public bool dialogIsPlaying { get; private set; }
    
    public bool CheckDialogCompletion()
    {
        return dialogAnimations.isTextAnimating; 
    }

    public void StopAnimating()
    {
        dialogAnimations.FinishCurrentAnimation();
    }

    // We're going to use a single coroutine so keep track of it
    private Coroutine typeRoutine = null;
    public void SpeakNextLine(string nextLine, string actorName, Material borderMaterial, Transform textBoxPosition)
    {
        SetActorUI(textBoxPosition, actorName);
        SetDialogAnimator();

        TEST(); 

        // Stop the Coroutine
        this.EnsureCoroutineStopped(ref typeRoutine);
        dialogAnimations.isTextAnimating = false;

        List<DialogCommand> commands = DialogUtility.ProcessMessage(nextLine, out string processedMessage);
        typeRoutine = StartCoroutine(dialogAnimations.AnimateTextIn(commands, processedMessage, null));
    }

    private IEnumerator ExitDialogMode()
    {
        yield return new WaitForSeconds(disableUITime);

        dialogIsPlaying = false;
        dialogText.text = string.Empty;
    }

    // Set Actor specific UI elements here
    private void SetActorUI(Transform textBoxPosition, string actorName)
    {
        if (dialogBox == null)
        {
            dialogBox = textBox.transform.parent; 
        }

        dialogBox.position = textBoxPosition.position;
        displayNameText.text = actorName; 
    }

    // Set the new Dialog Animatior here
    private void SetDialogAnimator()
    {
        dialogAnimations = new DialogAnimations(textBox, nextLineIndicator);
    }

    private void TEST()
    {
        textBox.gameObject.transform.parent.gameObject.SetActive(true);
    }
}
