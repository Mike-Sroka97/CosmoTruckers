using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour
{
    public static DialogManager instance;

    [Header("Main Text Components")]
    [SerializeField] private TMP_Text textBox;
    [SerializeField] private TextMeshProUGUI dialogText;
    [SerializeField] private TextMeshProUGUI displayNameText;

    [Header("Dialog UI")]
    [SerializeField] private Image mainPortrait;
    [SerializeField] private float disableUITime = 1f;

    private DialogAnimations dialogAnimations;
    private string currentLine; 

    public bool dialogIsPlaying { get; private set; }

    //Set instance or remove object
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }
    
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
    public void SpeakNextLine(string nextLine, TMP_Text _textBox, TextMeshProUGUI _dialogText, TextMeshProUGUI _displayNameText)
    {
        SetUIVariables(_textBox, _dialogText, _displayNameText); 

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

    private void SetUIVariables(TMP_Text _textBox, TextMeshProUGUI _dialogText, TextMeshProUGUI _displayNameText)
    {
        textBox = _textBox;
        dialogText = _dialogText;
        displayNameText = _displayNameText;

        dialogAnimations = new DialogAnimations(textBox);
    }
}
