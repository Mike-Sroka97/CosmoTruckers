using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class RegularTextManager : MonoBehaviour
{
    [Header("Main Text Components")]
    [SerializeField] private Transform boxParent;
    [SerializeField] private TMP_Text textBox;
    [SerializeField] private Image boxImage;
    [SerializeField] private Image nextLineIndicator;
    private bool boxIsActive = false;

    private AudioSource audioSource;
    private DialogTextAnimations dialogTextAnimations;
    private TextParser myTextParser;
    private string[] allLines;
    private int currentLineIndex = -1;
    private int allLinesCount = -1;
    private Animator indicatorAnimator;

    private Vector3 currentBoxPosition;

    [HideInInspector]
    public UnityEvent DialogStarted = new UnityEvent();
    public UnityEvent DialogEnded = new UnityEvent();

    public bool DialogIsPlaying { get; private set; }
    public bool AnimatingDialogBox { get; private set; }

    // Use these for additional check when trying to skip dialog
    private bool AdvanceTextCommencing = false;
    private bool FirstTimeSetupComplete;

    // Start is called before the first frame update
    void Start()
    {
        GetScripts(); 
    }

    public void StartRegularTextMode(TextAsset _textFile, Transform newBoxPosition)
    {
        if (newBoxPosition != null)
            currentBoxPosition = newBoxPosition.position;
        else
            currentBoxPosition = Vector3.zero; 

        if (!DialogIsPlaying && !DialogManager.Instance.DialogIsPlaying)
        {
            SetupVariables(_textFile);
            DialogIsPlaying = true;

            // Invoke Dialog Started Event
            DialogStarted.Invoke();

            // Go through the process of advancing the text
            StartCoroutine(AdvanceText());
        }
    }

    #region Text Mode

    private IEnumerator AnimateUIToSize(float timeToAnimate = 0.25f, float minVal = 0.1f, float maxVal = 1f, bool grow = true)
    {

        float minAlpha = 0.1f;
        float maxAlpha = 1f;

        Vector3 enablePosition = currentBoxPosition; 
        Vector3 disablePosition = new Vector3(-1000, -1000, 0);

        /// Grow or shrink the Dialog Box
        if (grow)
        {
            if (!boxIsActive)
            {
                boxIsActive = true;
                SetUIBoxActiveStates(true);
            }
        }

        if (!grow)
        {
            // We don't want new actor to shrink box to their scale
            float tempVal = maxVal;
            maxVal = minVal;
            minVal = tempVal;

            float tempAlpha = minAlpha;
            minAlpha = maxAlpha;
            maxAlpha = tempAlpha;
        }

        Vector3 startScale = new Vector3(minVal, minVal, minVal);
        Vector3 endScale = new Vector3(maxVal, maxVal, maxVal);


        float newAlpha;

        boxParent.transform.localScale = startScale;
        float timer = 0;

        boxParent.transform.position = enablePosition;

        // Maybe rewrite this weird conditional
        while ((grow && boxParent.transform.localScale.x < maxVal) || (!grow && boxParent.transform.localScale.x > maxVal))
        {
            timer += Time.deltaTime;
            boxParent.transform.localScale = Vector3.Lerp(startScale, endScale, timer / timeToAnimate);
            newAlpha = Mathf.Lerp(minAlpha, maxAlpha, timer / timeToAnimate);

            // Set the alpha of the UI Dialog Box elements
            boxImage.color = new Color(boxImage.color.r, boxImage.color.g, boxImage.color.b, newAlpha);

            yield return null;
        }
        boxParent.transform.localScale = endScale;
        AnimatingDialogBox = false;

        if (boxIsActive && !grow)
        {
            boxIsActive = false;
            SetUIBoxActiveStates(false);
            boxParent.transform.position = disablePosition;
        }
    }

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
        dialogTextAnimations.IsTextPlaying = false;

        List<DialogCommand> commands = DialogUtility.ProcessMessage(nextLine, out string processedMessage);
        Action<bool> action = SetNextLineIndicatorState;
        lineRoutine = StartCoroutine(dialogTextAnimations.AnimateTextIn(commands, processedMessage, action, null));
    }

    private IEnumerator AdvanceText()
    {
        AdvanceTextCommencing = true; 

        // Increment the current line
        currentLineIndex++;
        allLinesCount = allLines.Length;

        // Change this
        SetNextLineIndicatorState(false);

        if (currentLineIndex == 0)
        { 
            StartCoroutine(AnimateUIToSize());
            AnimatingDialogBox = true;

            while (AnimatingDialogBox)
                yield return null;
        }

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
            bool emptyBoolReference = false;

            float pBefore = 0.1f;
            string vcType = string.Empty;
            int vcRate = -1; // If -1 is passed in, use default voice rate

            DialogManager.Instance.HandlePreTextTags(tags, ref emptyBoolReference, ref pBefore, ref emptyActors, ref emptyStringReference,
                ref emptyBoolReference, ref vcType, ref vcRate);

            // Get the line associated with this actor and their dialog
            string currentLine = myTextParser.GetTrueRegularTextLine(allLines[currentLineIndex]);

            // Check if it's the first line in the text
            bool firstDialog = false; 
            if (currentLineIndex == 0)
                firstDialog = true;

            // Start delivering the text
            StartCoroutine(StartNextText(currentLine, waitTimeBetweenText: pBefore, firstDialog));

            yield return null;

            FirstTimeSetupComplete = true; 
            AdvanceTextCommencing = false; 
        }
    }

    public IEnumerator EndDialog()
    {
        allLinesCount = -1; 

        dialogTextAnimations.ClearText();
        SetNextLineIndicatorState(false);

        StartCoroutine(AnimateUIToSize(grow: false));
        AnimatingDialogBox = true;

        while (AnimatingDialogBox)
            yield return null;

        yield return null; 

        DialogIsPlaying = false;
        
        // Invoke Dialog Ended Event
        DialogEnded.Invoke();

        FirstTimeSetupComplete = false; 
    }

    #endregion

    #region Setup and Checks
    private void SetupVariables(TextAsset _textFile)
    {
        dialogTextAnimations = new DialogTextAnimations(textBox, nextLineIndicator, audioSource);
        currentLineIndex = -1; 
        GetScripts();

        allLines = myTextParser.GetAllLinesInRegularText(_textFile); 
    }
    private void GetScripts()
    {
        myTextParser = GetComponent<TextParser>();
        // Add the audio source
        audioSource = gameObject.AddComponent<AudioSource>();

        if (myTextParser == null)
            myTextParser = gameObject.AddComponent<TextParser>();
    }
    public bool CheckIfDialogTextAnimating()
    {
        if (dialogTextAnimations != null)
            return dialogTextAnimations.IsTextPlaying;
        else
            return false;
    }
    
    public bool CanSkipDialogText()
    {
        return dialogTextAnimations.CanSkipText && !AdvanceTextCommencing; 
    }

    private void SetUIBoxActiveStates(bool state)
    {
        /// Set the active state of the UI Dialog Box elements
        boxImage.gameObject.SetActive(state); 
        textBox.gameObject.SetActive(state);
    }

    void SetNextLineIndicatorState(bool state)
    {
        if (indicatorAnimator == null)
            indicatorAnimator = nextLineIndicator.GetComponent<Animator>();

        if (state == true)
            indicatorAnimator.Play("DM_NextLineIndicator_Grow");

        nextLineIndicator.enabled = state; 
    }
    #endregion

    #region Methods
    private bool CanAdvance()
    {
        bool canAdvance = true;

        if (currentLineIndex >= allLinesCount)
            canAdvance = false;

        return canAdvance && !DialogManager.Instance.DialogIsPlaying && FirstTimeSetupComplete;
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
                if (CheckIfDialogTextAnimating())
                {
                    if (CanSkipDialogText())
                        StopAnimating();
                }
                else { StartCoroutine(AdvanceText()); }
            }
        }
    }
    #endregion

    void Update()
    {
        CheckPlayerInput();
    }

    private void OnDisable()
    {
        DialogStarted.RemoveAllListeners();
        DialogEnded.RemoveAllListeners();
    }
}
