using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RegularTextManager : MonoBehaviour
{
    [Header("Main Text Components")]
    [SerializeField] private TMP_Text textBox;
    [SerializeField] private Image nextLineIndicator;

    private AudioSource audioSource;
    private DialogTextAnimations dialogTextAnimations;
    private TextParser myTextParser;
    private string[] allLines;
    private int currentLineIndex = -1;
    private int allLinesCount = 0;

    public bool DialogIsPlaying { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        GetTextParser(); 
    }

    public void StartRegularTextMode(TextAsset _textFile, TMP_Text _textBox, Image _nextLineIndicator, AudioSource _audioSource)
    {
        if (!DialogIsPlaying && !DialogManager.Instance.DialogIsPlaying)
        {
            textBox = _textBox;
            nextLineIndicator = _nextLineIndicator;
            audioSource = _audioSource;
            SetupVariables(_textFile);
            DialogIsPlaying = true;

            StartCoroutine(AdvanceText());
        }
    }

    #region Text Mode

    public IEnumerator StartNextText(string nextLine, float waitTimeBetweenText = 0.1f, bool firstDialog = false)
    {
        // If it's the first time dialog, don't animate out and in, just in
        if (!firstDialog)
            yield return new WaitForSeconds(waitTimeBetweenText);

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

    private IEnumerator AdvanceText()
    {
        // Increment the current line
        currentLineIndex++;
        allLinesCount = allLines.Length; 

        // Change this
        DialogManager.Instance.SetNextLineIndicatorState(false);

        // End the text if we've reached the line count
        if (currentLineIndex >= allLinesCount)
        {
            StartCoroutine(EndDialog());
        }
        // Otherwise, continue the text
        else
        {
            // At the current line in the base dialog, get the tags
            string[] tags = myTextParser.GetTagsInRegularTextLine(allLines[currentLineIndex]);

            // Handle the Pre Text Tags
            // Using the same method as dialogs for simplicity, but we don't need some info, so pass in a variable that we don't care about
            string emptyStringReference = string.Empty;
            List<int> emptyActors = new List<int> { };
            int emptyIntReference = 0;
            bool emptyBoolReference = false;

            float pBefore = 0.1f;
            string vcType = string.Empty;
            int vcRate = -1; // If -1 is passed in, use default voice rate

            DialogManager.Instance.HandlePreTextTags(tags, ref emptyStringReference, ref pBefore, ref emptyActors, ref emptyStringReference,
                ref emptyBoolReference, ref vcType, ref vcRate, ref emptyIntReference);

            // Get the line associated with this actor and their dialog
            string currentLine = myTextParser.GetTrueRegularTextLine(allLines[currentLineIndex]);

            // Check if it's the first line in the text
            bool firstDialog = false; 
            if (currentLineIndex == 0)
                firstDialog = true;

            // Start delivering the text
            StartCoroutine(StartNextText(currentLine, waitTimeBetweenText: pBefore, firstDialog));

            yield return null;
        }
    }

    public IEnumerator EndDialog()
    {
        dialogTextAnimations.ClearText();
        DialogManager.Instance.SetNextLineIndicatorState(false);

        yield return null; 

        DialogIsPlaying = false;
    }

    #endregion

    #region Setup and Checks
    private void SetupVariables(TextAsset _textFile)
    {
        dialogTextAnimations = new DialogTextAnimations(textBox, nextLineIndicator, audioSource);
        currentLineIndex = -1; 
        GetTextParser();

        allLines = myTextParser.GetAllLinesInRegularText(_textFile); 
    }
    private void GetTextParser()
    {
        myTextParser = GetComponent<TextParser>();
        
        if (myTextParser == null)
            myTextParser = gameObject.AddComponent<TextParser>();
    }
    public bool CheckIfDialogTextAnimating()
    {
        if (dialogTextAnimations != null)
            return dialogTextAnimations.isTextAnimating;
        else
            return false;
    }
    #endregion

    #region Methods
    private bool CanAdvance()
    {
        bool canAdvance = true;

        if (currentLineIndex >= allLinesCount)
            canAdvance = false;

        return canAdvance && !DialogManager.Instance.UpdatingDialogBox;
    }
    public void StopAnimating()
    {
        dialogTextAnimations.FinishCurrentAnimation();
    }
    private void CheckPlayerInput()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (CanAdvance())
            {
                if (CheckIfDialogTextAnimating()) { StopAnimating(); }
                else { StartCoroutine(AdvanceText()); }
            }
        }
    }
    #endregion

    void Update()
    {
        CheckPlayerInput();
    }
}
