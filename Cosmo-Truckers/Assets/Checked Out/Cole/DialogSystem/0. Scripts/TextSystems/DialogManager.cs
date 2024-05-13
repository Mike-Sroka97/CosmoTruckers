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
    private RegularTextManager regularTextManager;

    public List<BaseActor> PlayerActors { get; private set; }
    public TextParser TextParser;
    public ActorList ActorList;
    private bool noWaitDialog = false;

    // Public bools
    public bool AnimatingDialogBox { get; private set; }
    public bool UpdatingDialogBox { get; private set; }

    public bool DialogIsPlaying;

    void Awake()
    {
        if (Instance == null)
        {
            // Add the audio source
            audioSource = gameObject.AddComponent<AudioSource>();

            // Make sure it has scripts
            GetLocalScripts();

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
    public void SetPlayerActors(List<BaseActor> actors)
    {
        PlayerActors = actors;
    }
    public void GetLocalScripts()
    {
        if (TextParser == null)
        {
            TextParser = GetComponent<TextParser>();
            ActorList = GetComponent<ActorList>();
        }
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
    private void SetDialogUI(Transform newPosition, string direction, float scale)
    {
        dialogBox.position = newPosition.position;
        dialogBox.localScale = new Vector3(scale, scale, scale); 

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

    /// Grow or shrink the Dialog Box
    private IEnumerator AnimateUIToSize(float timeToAnimate = 0.5f, float minVal = 0.1f, float maxVal = 1f, bool grow = true)
    {
        if (!boxIsActive && grow)
        {
            boxIsActive = true; 
            SetUIBoxActiveStates(true);
        }

        float minAlpha = 0.1f;
        float maxAlpha = 1f; 

        if (!grow)
        {
            float tempVal = minVal;
            minVal = maxVal; 
            maxVal = tempVal;

            float tempAlpha = minAlpha;
            minAlpha = maxAlpha; 
            maxAlpha = tempAlpha;
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

    /// Set the active state of the UI Dialog Box elements
    private void SetUIBoxActiveStates(bool state)
    {
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
        if (indicatorAnimator == null)
            indicatorAnimator = nextLineIndicator.GetComponent<Animator>();

        if (state == true)
            indicatorAnimator.Play("DM_NextLineIndicator_Grow"); 

        nextLineIndicator.enabled = state; 
    }
    #endregion

    #region Dialog Mode

    /// Start the next dialog
    /// Currently called by BaseActor

    private Coroutine lineRoutine = null;
    public IEnumerator StartNextDialog(string nextLine, BaseActor speaker, Material borderMaterial, 
        Transform textBoxPosition, float scale, bool sameSpeaker = false, bool firstDialog = false, 
        float waitTimeBetweenDialogs = 0.5f, string actorDirection = "left")
    {
        string actorName = speaker.actorName; 

        // If it's the first time dialog, don't animate box out and in, just in
        if (firstDialog)
        {
            UpdatingDialogBox = true;

            SetDialogUI(textBoxPosition, actorDirection, scale);
            ClearDialogText();

            foreach (BaseActor actor in currentActorsToAnimate)
                actor.StartAnimation(); 

            // We can use tags to alter this
            yield return new WaitForSeconds(waitTimeBetweenDialogs);

            SetDialogBox(speaker);
            AnimatingDialogBox = true;
            StartCoroutine(AnimateUIToSize(maxVal: scale));

            while (AnimatingDialogBox)
                yield return null;

            UpdatingDialogBox = false;
        }
        //If it's not the same speaker, animate box out and in to new speaker
        else if (!sameSpeaker)
        {
            UpdatingDialogBox = true; 

            AnimatingDialogBox = true;
            StartCoroutine(AnimateUIToSize(maxVal: scale, grow:false));

            while (AnimatingDialogBox)
                yield return null;

            // Wait until text box is shrunk before moving positions
            SetDialogUI(textBoxPosition, actorDirection, scale);
            ClearDialogText();

            // Wait before shrinking Dialog Box to animate
            foreach (BaseActor actor in currentActorsToAnimate)
                actor.StartAnimation();

            // We can use tags to alter this
            yield return new WaitForSeconds(waitTimeBetweenDialogs);

            SetDialogBox(speaker);
            AnimatingDialogBox = true;
            StartCoroutine(AnimateUIToSize(maxVal: scale));

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
        dialogTextAnimations.isTextAnimating = false;

        // Speak the next line of dialog. Process the dialog commands and start animating the text into the text box
        List<DialogCommand> commands = DialogUtility.ProcessMessage(nextLine, out string processedMessage);
        lineRoutine = StartCoroutine(dialogTextAnimations.AnimateTextIn(commands, processedMessage, speaker));
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
            AnimatingDialogBox = true;
            StartCoroutine(AnimateUIToSize(maxVal: dialogBox.transform.localScale.x, grow:false));
        }
        else { AnimatingDialogBox = false; }

        while (AnimatingDialogBox)
            yield return null;

        ClearDialogText();
        SetUIBoxActiveStates(false); 

        yield return new WaitForSeconds(disableUITime);

        DialogIsPlaying = false;
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
            return dialogTextAnimations.isTextAnimating;
        else
            return false; 
    }
    public void StopAnimating()
    {
        dialogTextAnimations.FinishCurrentAnimation();
    }

    #endregion

    #region Regular Text Methods
    public void StartRegularTextMode(TextAsset _textFile, TMP_Text _textBox, Image _nextLineIndicator)
    {
        // Get the Regular Text Manager
        regularTextManager = GetComponent<RegularTextManager>();

        if (regularTextManager == null)
            Debug.LogError("No Regular Text Manager!");
        else 
        {
            if (!regularTextManager.DialogIsPlaying)
                regularTextManager.StartRegularTextMode(_textFile, _textBox, _nextLineIndicator, audioSource);
        }
    }
    #endregion
}
