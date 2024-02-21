using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
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

    [Header("Dialog Box")]
    [SerializeField] private Image dialogBoxImage, dialogBoxImageBorder; 
    [SerializeField] private Image nextLineIndicator; 
    [SerializeField] private float disableUITime = 1f;

    private DialogAnimations dialogAnimations;
    private string currentLine; 

    public bool dialogIsPlaying { get; private set; }

    #region Setup
    // Set the new Dialog Animatior here
    private void SetDialogAnimator()
    {
        dialogAnimations = new DialogAnimations(textBox, nextLineIndicator);
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

    private IEnumerator AnimateUIToSize(float timeToGrow = 1f, float startSize = 0.1f, float endSize = 1f, bool grow = true)
    {
        Vector3 startScale = new Vector3(startSize, startSize, startSize);
        Vector3 endScale = new Vector3(endSize, endSize, endSize);

        dialogBox.localScale = startScale;
        float timer = 0; 

        // Probably rewrite this weird conditional
        while ((grow && dialogBox.localScale.x < endSize) || (!grow && dialogBox.localScale.x > endSize))
        {
            timer += Time.deltaTime;
            dialogBox.localScale = Vector3.Lerp(startScale, endScale, timer / timeToGrow);
            yield return null;
        }

        dialogBox.localScale = endScale;
    }

    #endregion

    #region Dialog Mode
    // We're going to use a single coroutine so keep track of it
    private Coroutine typeRoutine = null;
    public void SpeakNextLine(string nextLine, string actorName, Material borderMaterial, Transform textBoxPosition)
    {
        SetDialogAnimator();
        SetActorUI(textBoxPosition, actorName);

        //TEST(); 

        // Stop the Coroutine
        this.EnsureCoroutineStopped(ref typeRoutine);
        dialogAnimations.isTextAnimating = false;

        AnimateUIToSize();

        List<DialogCommand> commands = DialogUtility.ProcessMessage(nextLine, out string processedMessage);
        typeRoutine = StartCoroutine(dialogAnimations.AnimateTextIn(commands, processedMessage, null));
    }
    
    public void EndDialog()
    {

    }
    private IEnumerator ExitDialogMode()
    {
        yield return new WaitForSeconds(disableUITime);

        dialogIsPlaying = false;
        dialogText.text = string.Empty;
    }
    #endregion




    public bool CheckIfDialogAnimating()
    {
        return dialogAnimations.isTextAnimating;
    }
    public void StopAnimating()
    {
        dialogAnimations.FinishCurrentAnimation();
    }
}
