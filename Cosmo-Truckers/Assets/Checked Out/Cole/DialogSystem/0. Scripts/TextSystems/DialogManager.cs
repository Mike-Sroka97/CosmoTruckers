using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour
{
    public static DialogManager Instance;

    [Header("Main Text Components")]
    [SerializeField] private TMP_Text textBox;
    [SerializeField] private TextMeshProUGUI displayNameText;

    [Header("Dialog Box UI")]
    [SerializeField] private Transform dialogBox;
    [SerializeField] private Image dialogBoxImage; 
    [SerializeField] private Image dialogBoxImageBorder; 
    [SerializeField] private Image actorDirection; 
    [SerializeField] private Image nextLineIndicator;
    [SerializeField] private float disableUITime = 1f;
    private Animator indicatorAnimator;

    // Dialog Box Stuff
    // 0:normal 1:box 2:spiky 3:happy
    [SerializeField] private Sprite[] dialogBoxBases; 
    [SerializeField] private Sprite[] dialogBoxBorders;
    [SerializeField] private Sprite defaultNextIndicator; 
    private int boxNumber = 0;
    private bool boxIsActive = false;

    // Dialog Scene Variables
    private GameObject sceneLayout;
    TextAsset textFile;

    private AudioSource audioSource;
    private DialogTextAnimations dialogTextAnimations;
    private List<BaseActor> currentActorsToAnimate;

    public List<BaseActor> PlayerActors { get; private set; }
    public TextParser TextParser;
    public ActorList ActorList;

    private BaseActor[] actors;
    private string[] dialogs;
    private string baseDialog;
    private int currentLineIndex = 0;
    private int currentID = 1;
    private int lastID = -1;
    private int allLinesCount = 0;
    
    private bool firstTimeSetupComplete;
    private Vector3 newBoxPosition = Vector3.zero; 
    private Vector3 lastBoxPosition = Vector3.zero; 
    private bool noWaitDialog = false;
    private float noWaitDialogTimeBetween = 0.35f; 
    private float cutsceneDialogWaitTime = 2.5f; 

    // Public Variables
    public int CurrentLineIndex { get; private set; } = 0;
    public int CurrentTextFile { get; private set; } = 0;

    public static readonly string[] BasePlayerNames = new string[] { "AEGLAR", "SAFE-T", "PROTO", "SIX FACE" };

    // Public bools
    public bool AnimatingDialogBox { get; private set; }
    public bool UpdatingDialogBox { get; private set; }
    public bool CutsceneDialog { get; private set; }

    public bool TextSpeedNormal = true; 

    public bool DialogIsPlaying;

    void Awake()
    {
        if (Instance == null)
        {
            // Add the audio source
            audioSource = gameObject.AddComponent<AudioSource>();

            // Make sure it has scripts
            SetupLocalScripts();

            Instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    #region New Dialog Info
    public void SetPlayerActors(List<BaseActor> actors, bool isCutscene = false)
    {
        PlayerActors = actors;
        CutsceneDialog = isCutscene;
    }
    public void SetupLocalScripts()
    {
        if (TextParser == null)
        {
            TextParser = GetComponent<TextParser>();
            ActorList = GetComponent<ActorList>();
        }
    }
    public void SetupDialogs(TextAsset textFile)
    {
        dialogs = new string[PlayerActors.Count];
        baseDialog = TextParser.GetAllDialogs(textFile)[0];
    }
    public void SetBaseActors(BaseActor[] _actors)
    {
        actors = _actors; 
    }

    #endregion

    #region Next Dialog Setup
    private void SetDialogBox(BaseActor speaker)
    {
        dialogBoxImage.sprite = dialogBoxBases[boxNumber]; 
        dialogBoxImageBorder.sprite = dialogBoxBorders[boxNumber];
        dialogBoxImageBorder.material = speaker.actorTextMaterial;

        if (speaker.actorNextIndicator != null)
            nextLineIndicator.sprite = speaker.actorNextIndicator;
        else
            nextLineIndicator.sprite = defaultNextIndicator; 
    }
    private void SetDialogUI(Transform newPosition, string direction)
    {
        newBoxPosition = newPosition.position; 

        switch (direction)
        {
            case "right":
                actorDirection.enabled = true; 
                actorDirection.rectTransform.eulerAngles = new Vector3(0, 180f, 0);
                break;
            case "none": 
                actorDirection.enabled = false;
                break;
            default:
                actorDirection.enabled = true;
                actorDirection.rectTransform.eulerAngles = new Vector3(0, 0f, 0);
                break; 
        }
    }

    private IEnumerator AnimateUIToSize(float timeToAnimate = 0.25f, float minVal = 0.1f, float maxVal = 1f, bool grow = true)
    {
        float minAlpha = 0.1f;
        float maxAlpha = 1f;
        Vector3 endPosition = FindObjectOfType<CameraController>().transform.position;
        endPosition = new Vector3(endPosition.x, endPosition.y, 0f); 

        /// Grow or shrink the Dialog Box
        if (grow)
        {
            if (!boxIsActive)
            {
                boxIsActive = true;
                SetUIBoxActiveStates(true);
            }

            lastBoxPosition = newBoxPosition;
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

            Vector3 tempPosition = endPosition;
            endPosition = lastBoxPosition;
            lastBoxPosition = tempPosition; 
        }

        Vector3 startScale = new Vector3(minVal, minVal, minVal);
        Vector3 endScale = new Vector3(maxVal, maxVal, maxVal);
        float newAlpha; 

        dialogBox.localScale = startScale;
        float timer = 0;

        // Maybe rewrite this weird conditional
        while ((grow && dialogBox.localScale.x < maxVal) || (!grow && dialogBox.localScale.x > maxVal))
        {
            timer += Time.deltaTime;
            dialogBox.localScale = Vector3.Lerp(startScale, endScale, timer / timeToAnimate);
            newAlpha = Mathf.Lerp(minAlpha, maxAlpha, timer / timeToAnimate);

            // Set the alpha of the UI Dialog Box elements
            dialogBoxImage.color = new Color(dialogBoxImage.color.r, dialogBoxImage.color.g, dialogBoxImage.color.b, newAlpha);
            dialogBoxImageBorder.color = new Color(dialogBoxImageBorder.color.r, dialogBoxImageBorder.color.g, dialogBoxImageBorder.color.b, newAlpha);
            actorDirection.color = new Color(actorDirection.color.r, actorDirection.color.g, actorDirection.color.b, newAlpha);

            // Grow the box to the center or shrink to the old starting position
            dialogBox.position = Vector3.Lerp(lastBoxPosition, endPosition, timer / timeToAnimate); 

            yield return null;
        }

        dialogBox.localScale = endScale;
        AnimatingDialogBox = false;

        if (boxIsActive && !grow)
        {
            boxIsActive = false;
            SetUIBoxActiveStates(false);
        }
    }

    private void SetUIBoxActiveStates(bool state)
    {
        /// Set the active state of the UI Dialog Box elements
        dialogBoxImage.gameObject.SetActive(state);
        dialogBoxImageBorder.gameObject.SetActive(state);
        actorDirection.gameObject.SetActive(state);
    }

    /// <summary>
    /// Set which actors will be animated
    /// </summary>
    /// <param name="actors"></param>
    public void ActorsToAnimate(List<BaseActor> actors)
    {
        if (actors == null)
            currentActorsToAnimate.Clear();
        else
            currentActorsToAnimate = actors;
    } 

    /// <summary>
    /// Enable or disable the next line indicator
    /// </summary>
    /// <param name="state"></param>
    public void SetNextLineIndicatorState(bool state)
    {
        if (!CutsceneDialog)
        {
            if (indicatorAnimator == null)
                indicatorAnimator = nextLineIndicator.GetComponent<Animator>();

            if (state == true)
                indicatorAnimator.Play("DM_NextLineIndicator_Grow");

            nextLineIndicator.enabled = state;
        }
        else
        {
            nextLineIndicator.enabled = false; 
        }
    }
    #endregion

    #region Dialog Mode

    public IEnumerator AdvanceScene()
    {
        if (noWaitDialog)
            yield break; 

        // Increment the current line
        currentLineIndex++;

        allLinesCount = TextParser.GetAllLinesInThisDialogCount(dialogs[0]);

        SetNextLineIndicatorState(false);

        // End the dialog if we've reached the line count
        if (currentLineIndex >= allLinesCount)
        {
            StartCoroutine(EndDialog());
        }
        // Otherwise, continue the dialog
        else
        {
            // At the current line in the base dialog, get the tags
            string[] tags = TextParser.GetTagsAtCurrentDialogLine(baseDialog, currentLineIndex);
            string speakerDialog;

            // Get the actor ID for this line and the dialog associated with that actor
            if (int.TryParse(tags[0], out currentID))
            {
                int dialogToGrab = currentID - 1;

                // Non-players don't need to worry about this. Grab the base dialog
                if (dialogToGrab > 3)
                    dialogToGrab = 0;

                speakerDialog = dialogs[dialogToGrab]; // ID's in text will be on a starting scale of 1
            }
            else
            {
                Debug.LogError("Unable to parse int out of first tag!");
                speakerDialog = baseDialog;
            }

            // Handle the Pre Text Tags, and return any variables needed to pass into actors
            string speakerDirection = string.Empty;
            float pBefore = 0f;
            List<int> actorsToAnim = new List<int> { };
            string animToPlay = string.Empty;
            bool waitForAnim = false;
            float waitTime = 0f;
            string vcType = string.Empty;
            int vcRate = -1; // If -1 is passed in, use default voice rate

            HandlePreTextTags(tags, ref speakerDirection, ref pBefore, ref actorsToAnim, ref animToPlay,
                ref waitForAnim, ref vcType, ref vcRate);

            // Set wait time to pause before value. This will get overwritten if waitForAnim is true 
            if (pBefore > 0f)
                waitTime = pBefore;

            // Animate actors or clear their animation
            List<BaseActor> actorsToAnimate = new List<BaseActor>();
            if (actorsToAnim != null)
            {
                float animationTime = 0;

                for (int i = 0; i < actorsToAnim.Count; i++)
                {
                    foreach (BaseActor actor in actors)
                    {
                        if (actor.actorID == actorsToAnim[i])
                        {
                            actor.GetAnimationInfo(animToPlay, ref animationTime);
                            actorsToAnimate.Add(actor);
                            break;
                        }
                    }
                }

                ActorsToAnimate(actorsToAnimate);

                if (waitForAnim)
                    waitTime = animationTime;
            }
            else
            {
                foreach (BaseActor actor in actors)
                {
                    actor.ClearAnimationInfo();
                }

                ActorsToAnimate(null);
            }

            // Get the line associated with this actor and their dialog
            string currentLine = TextParser.GetDialogTextAtCurrentLine(speakerDialog, currentLineIndex);

            // Check if it's the first line in the dialog
            bool firstDialog = false;
            if (currentLineIndex == 1)
                firstDialog = true;

            // Tell the actor to deliver the line
            actors[currentID - 1].DeliverLine(currentLine, lastID, firstDialog, speakerDirection, waitTime);

            // Set last id after delivering
            lastID = currentID;

            // Prevents user from spamming
            firstTimeSetupComplete = true; 

            if (noWaitDialog)
            {
                while (noWaitDialog)
                {
                    if (CheckIfDialogTextAnimating() || UpdatingDialogBox)
                    {
                        yield return null;
                    }
                    else
                    {
                        yield return new WaitForSeconds(noWaitDialogTimeBetween);
                        noWaitDialog = false;
                        StartCoroutine(AdvanceScene());
                        yield break;
                    }
                }
            }

            else
            {
                if (CutsceneDialog)
                {
                    while (CheckIfDialogTextAnimating() || UpdatingDialogBox)
                    {
                        yield return null;
                    }

                    yield return new WaitForSeconds(cutsceneDialogWaitTime);
                    StartCoroutine(AdvanceScene());
                    yield break;
                }
            }


            yield return null;
        }
    }

    /// Start the next dialog
    /// Currently called by BaseActor
    private Coroutine lineRoutine = null;
    public IEnumerator StartNextDialog(string nextLine, BaseActor speaker, Material borderMaterial, 
        Transform textBoxPosition, bool sameSpeaker = false, bool firstDialog = false, 
        float waitTimeBetweenDialogs = 0.5f, string actorDirection = "left")
    {
        string actorName = speaker.actorName; 

        // If it's the first time dialog, don't animate box out and in, just in
        if (firstDialog)
        {
            UpdatingDialogBox = true;

            SetDialogUI(textBoxPosition, actorDirection);
            ClearDialogText();

            foreach (BaseActor actor in currentActorsToAnimate)
                actor.StartAnimation(); 

            // We can use tags to alter this
            yield return new WaitForSeconds(waitTimeBetweenDialogs);

            SetDialogBox(speaker);
            StartCoroutine(AnimateUIToSize());
            AnimatingDialogBox = true; 

            while (AnimatingDialogBox)
                yield return null;

            UpdatingDialogBox = false;
        }
        //If it's not the same speaker, animate box out and in to new speaker
        else if (!sameSpeaker)
        {
            UpdatingDialogBox = true; 

            StartCoroutine(AnimateUIToSize(grow:false));
            AnimatingDialogBox = true;

            while (AnimatingDialogBox)
                yield return null;

            // Wait until text box is shrunk before moving positions
            SetDialogUI(textBoxPosition, actorDirection);
            ClearDialogText();

            // Wait before shrinking Dialog Box to animate
            foreach (BaseActor actor in currentActorsToAnimate)
                actor.StartAnimation();

            // We can use tags to alter this
            yield return new WaitForSeconds(waitTimeBetweenDialogs);

            SetDialogBox(speaker);
            StartCoroutine(AnimateUIToSize());
            AnimatingDialogBox = true;

            while (AnimatingDialogBox)
                yield return null;

            UpdatingDialogBox = false;
        }
        
        // Otherwise, stop Animating and Updating Dialog Box
        else 
        { 
            AnimatingDialogBox = false;
            UpdatingDialogBox = false;
        }

        dialogTextAnimations = new DialogTextAnimations(textBox, nextLineIndicator, audioSource);
        displayNameText.text = actorName;

        // Stop the Coroutine
        this.EnsureCoroutineStopped(ref lineRoutine);
        dialogTextAnimations.IsTextPlaying = false;

        // Speak the next line of dialog. Process the dialog commands and start animating the text into the text box
        List<DialogCommand> commands = DialogUtility.ProcessMessage(nextLine, out string processedMessage);
        Action<bool> action = SetNextLineIndicatorState; 
        lineRoutine = StartCoroutine(dialogTextAnimations.AnimateTextIn(commands, processedMessage, action, speaker, TextSpeedNormal));
    }

    /// <summary>
    /// Grab all of the tags before the text starts playing
    /// </summary>
    /// <param name="tags"></param>
    /// <param name="speakerDirection"></param>
    /// <param name="pBefore"></param>
    /// <param name="actorsToAnimate"></param>
    /// <param name="animToPlay"></param>
    /// <param name="waitForAnim"></param>
    /// <param name="vcType"></param>
    /// <param name="vcRate"></param>
    /// <param name="boxNumber"></param>
    public void HandlePreTextTags(string[] tags, ref string speakerDirection, ref float pBefore, ref List<int> actorsToAnimate,
    ref string animToPlay, ref bool waitForAnim, ref string vcType, ref int vcRate)
    {
        // Start at 1, first tag is actorID
        for (int i = 1; i < tags.Length; i++)
        {
            string[] tagValues = tags[i].Split(":");
            string tagKey = tagValues[0].Trim();
            string tagValue = tagValues[1].Trim();

            if (tagKey == "direction")
            {
                speakerDirection = tagValue;
            }
            else if (tagKey == "pBefore")
            {
                pBefore = float.Parse(tagValue);
            }
            else if (tagKey == "animWait" || tagKey == "animDefault")
            {
                if (tagKey == "animWait")
                    waitForAnim = true;

                List<string> allValues = tagValue.Split(",").ToList();
                animToPlay = allValues[allValues.Count - 1];
                allValues.RemoveAt(allValues.Count - 1);

                foreach (string actorID in allValues)
                {
                    int thisID = 0;
                    if (int.TryParse(actorID, out thisID))
                        actorsToAnimate.Add(thisID);
                    else
                        Debug.LogError("Animation actor ID is not an integer!");
                }
            }
            else if (tagKey == "vc")
            {
                // VC Type is 0, VC Rate is 1 
                List<string> allValues = tagValue.Split(",").ToList();
                vcType = allValues[0];

                int rate;
                if (int.TryParse(allValues[1], out rate))
                    vcRate = rate;
                else
                    Debug.LogError("VC Rate is not an integer - going with default rate!");
            }
            else if (tagKey == "bub")
            {
                if (int.TryParse(tagValue, out int parsedNumber))
                    boxNumber = parsedNumber;
            }
            else if (tagKey == "noWait")
                noWaitDialog = true;
            else
            {
                Debug.Log("No additional Pre-Text tag found!");
            }
        }
    }

    /// <summary>
    /// End the Dialog. 
    /// Shrink the dialog box, clear the text, disable UI, and set DialogIsPlaying to false
    /// </summary>
    /// <returns></returns>
    public IEnumerator EndDialog()
    {
        if (boxIsActive)
        {
            StartCoroutine(AnimateUIToSize(maxVal: dialogBox.transform.localScale.x, grow:false));
            AnimatingDialogBox = true;
        }
        else { AnimatingDialogBox = false; }

        while (AnimatingDialogBox)
            yield return null;

        ClearDialogText();
        SetUIBoxActiveStates(false); 

        yield return new WaitForSeconds(disableUITime);

        DialogIsPlaying = false;
        firstTimeSetupComplete = false; 
    }
    #endregion

    #region Methods and Checks
    private void ClearDialogText()
    {
        textBox.text = string.Empty;
        displayNameText.text = string.Empty;

        if (dialogTextAnimations != null)
        {
            dialogTextAnimations.ClearText();
        }
    }
    public bool CheckIfDialogTextAnimating()
    {
        if (dialogTextAnimations != null)
            return dialogTextAnimations.IsTextPlaying;
        else
            return false; 
    }
    public void StopAnimating()
    {
        dialogTextAnimations.FinishCurrentAnimation();
    }
    public void SetDialog(int index, string dialog)
    {
        dialogs[index] = dialog;
    }
    public bool CanAdvanceDialog()
    {
        bool canAdvance = true;

        if (currentLineIndex >= allLinesCount)
            canAdvance = false;

        return canAdvance && !UpdatingDialogBox && DialogIsPlaying && firstTimeSetupComplete && CutsceneDialog;
    }

    #endregion
}
