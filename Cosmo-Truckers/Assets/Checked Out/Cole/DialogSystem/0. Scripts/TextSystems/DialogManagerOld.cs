using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Ink.Runtime;
using UnityEngine.EventSystems;

public class DialogManagerOld : MonoBehaviour
{
    public static DialogManagerOld instance;

    [Header("Main Text Components")]
    [SerializeField] private TMP_Text textBox;
    [SerializeField] private TextMeshProUGUI dialogText; 
    [SerializeField] private TextMeshProUGUI displayNameText; 

    [Header("Dialog UI")]
    [SerializeField] private Image mainPortrait;
    [SerializeField] private float disableUITime = 1f;
    [SerializeField] private Image nextLineIndicator;

    public bool dialogIsPlaying { get; private set; }

    private DialogAnimations dialogAnimations;
    private Story currentStory; //Inky file

    #region TAG VARIABLES
    private const string SPEAKER_TAG = "speaker";
    private const string IMAGE_TAG = "image";
    #endregion

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

    private void Start()
    {
        dialogIsPlaying = false;
        SetDialogAnimations(textBox); 
    }

    public void EnterDialogMode(TextAsset inkJson)
    {
        // Story is an object type specific to Inky
        currentStory = new Story(inkJson.text);
        dialogIsPlaying = true;

        ContinueStory(); 
    }

    // We're going to use a single coroutine so keep track of it
    private Coroutine typeRoutine = null; 

    /// <summary>
    /// Checks if dialogAnimations is animating. If the story can continue, then continue it. 
    /// Otherwise, exit DialogMode. 
    /// </summary>
    void ContinueStory()
    {
        if (!dialogAnimations.isTextAnimating)
        {
            if (currentStory.canContinue)
            {
                // Stop the Coroutine
                this.EnsureCoroutineStopped(ref typeRoutine);
                dialogAnimations.isTextAnimating = false;

                List<DialogCommand> commands = DialogUtility.ProcessMessage(currentStory.Continue(), out string processedMessage);
                typeRoutine = StartCoroutine(dialogAnimations.AnimateTextIn(commands, processedMessage, null));

                HandleTags(currentStory.currentTags);
            }
            else
            {
                StartCoroutine(ExitDialogMode()); 
            }

        }
        else
        {
            dialogAnimations.FinishCurrentAnimation(); 
        }
    }
    
    private IEnumerator ExitDialogMode()
    {
        yield return new WaitForSeconds(disableUITime);

        dialogIsPlaying = false; 
        dialogText.text = string.Empty;
    }

    void HandleTags(List<string> currentTags)
    {
        // Go through all of the tags. Parse them and do things with the info
        foreach (string tag in currentTags)
        {
            // parse the tag into array of key and value
            string[] splitTag = tag.Split(':');

            if (splitTag.Length != 2)
                Debug.LogError("Parsing error with tag: " + tag);

            // remove whitespace
            string key = splitTag[0].Trim(); 
            string value = splitTag[1].Trim();

            switch (key)
            {
                case SPEAKER_TAG:
                    displayNameText.text = value;
                    break;
                default:
                    Debug.LogWarning($"Tag: {key} came in but is not currently being used.");
                    break; 
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        CheckDialogInputs(); 
    }

    void CheckDialogInputs()
    {
        if (!dialogIsPlaying)
            return;

        if (Input.GetKeyDown(KeyCode.Space))
            ContinueStory(); 
    }

    // If we want to call this script from external source, pass in new TMP vars
    public void SetTextVariables(TMP_Text _textBox, TextMeshProUGUI _dialogText, TextMeshProUGUI _displayNameText)
    {
        textBox = _textBox;
        dialogText = _dialogText;
        displayNameText = _displayNameText;

        SetDialogAnimations(textBox);
    }

    void SetDialogAnimations(TMP_Text _textBox)
    {
        dialogAnimations = new DialogAnimations(textBox, nextLineIndicator);
    }
}


