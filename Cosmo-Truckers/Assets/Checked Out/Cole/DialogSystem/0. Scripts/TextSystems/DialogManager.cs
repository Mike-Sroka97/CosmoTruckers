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
    private Animator indicatorAnimator; 
    [SerializeField] private float disableUITime = 1f;
    // 0:normal 1:box 2:spiky 3:happy
    [SerializeField] private Sprite[] dialogBoxBases; 
    [SerializeField] private Sprite[] dialogBoxBorders;
    [SerializeField] private Sprite defaultNextIndicator; 
    private int boxNumber = 0; 
    
    [Header("Dialog Scene Loading")]
    [SerializeField] private Image fader; 
    private const float FadeTime = 2f;
    private bool fading = false; 

    // Dialog Scene Variables
    private GameObject sceneLayout;
    TextAsset textFile;
    List<BaseActor> playerActors;

    private AudioSource audioSource;
    private DialogTextAnimations dialogTextAnimations;
    private List<BaseActor> currentActorsToAnimate;
    private RegularTextManager regularTextManager; 
    private string currentLine;
    private bool boxIsActive = false;

    // Public bools
    public bool AnimatingDialogBox { get; private set; }
    public bool DialogIsPlaying { get; private set; }
    public bool UpdatingDialogBox { get; private set; }

    void Awake()
    {
        if (Instance == null)
        {
            // Add the audio source
            audioSource = this.gameObject.AddComponent<AudioSource>(); 

            Instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    #region New Dialog Scene
    public IEnumerator LoadDialogScene(GameObject _sceneLayout, TextAsset _textFile, List<BaseActor> _playerActors)
    {
        if (!DialogIsPlaying && !regularTextManager.DialogIsPlaying)
        {
            sceneLayout = _sceneLayout;
            textFile = _textFile;
            playerActors = _playerActors;

            SetFading(true);
            StartCoroutine(FadeFader());

            while (fading)
                yield return null;

            // Load into new scene after fading
            SceneManager.LoadScene(7);
        }
    }
    public void GetSceneInformation(ref GameObject _sceneLayout, ref TextAsset _textFile, ref List<BaseActor> _playerActors)
    {
        _sceneLayout = sceneLayout;
        _textFile = textFile;
        _playerActors = playerActors;
    }
    public IEnumerator FadeFader(float fadeTime = FadeTime, bool fadeIn = true)
    {
        float timer = 0;
        float newAlpha = fader.color.a;
        float startAlpha = 0.05f;
        float endAlpha = 1f; 

        if (!fadeIn)
        {
            startAlpha = 1f; 
            endAlpha = 0.05f; 
        }

        while ((fadeIn && newAlpha < endAlpha) || (!fadeIn && newAlpha > endAlpha))
        {
            timer += Time.deltaTime;
            newAlpha = Mathf.Lerp(startAlpha, endAlpha, timer / fadeTime);
            fader.color = new Color(fader.color.r, fader.color.g, fader.color.b, newAlpha); 
            yield return null;
        }

        // Make sure the fader alpha is correct
        if (!fadeIn)
            fader.color = new Color(fader.color.r, fader.color.g, fader.color.b, 0f);
        else
            fader.color = new Color(fader.color.r, fader.color.g, fader.color.b, 1f);

        SetFading(false); 
    } 
    #endregion

    #region Next Dialog Setup
    private void SetDialogAnimator() // Set the new Dialog Animatior here
    {
        dialogTextAnimations = new DialogTextAnimations(textBox, nextLineIndicator, audioSource);
    }
    public void SetDialogBoxNumber(int dialogBoxNumber)
    {
        boxNumber = dialogBoxNumber; 
    }
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
        dialogBox.position = newPosition.position;

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
        AnimatingDialogBox = false;

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
    public void ActorsToAnimate(List<BaseActor> actors)
    {
        if (actors == null)
            currentActorsToAnimate.Clear();
        else
            currentActorsToAnimate = actors;
    } //Set the actors to animate
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
    public IEnumerator StartNextDialog(string nextLine, BaseActor speaker, Material borderMaterial, 
        Transform textBoxPosition, bool sameSpeaker = false, bool firstDialog = false, 
        float waitTimeBetweenDialogs = 0.5f, string actorDirection = "left")
    {
        DialogIsPlaying = true; 

        string actorName = speaker.actorName; 

        // If it's the first time dialog, don't animate out and in, just in
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
            AnimatingDialogBox = true;
            StartCoroutine(AnimateUIToSize());

            while (AnimatingDialogBox)
                yield return null;

            UpdatingDialogBox = false;
        }
        //If it's not the same speaker, animate out and in to new speaker
        else if (!sameSpeaker)
        {
            UpdatingDialogBox = true; 

            AnimatingDialogBox = true;
            StartCoroutine(AnimateUIToSize(grow:false));

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
            AnimatingDialogBox = true;
            StartCoroutine(AnimateUIToSize());

            while (AnimatingDialogBox)
                yield return null;

            UpdatingDialogBox = false;
        }
        else 
        { 
            AnimatingDialogBox = false;
            UpdatingDialogBox = false;
        }

        SetDialogAnimator();
        displayNameText.text = actorName;
        SpeakNextLine(nextLine, speaker);
    }

    private Coroutine lineRoutine = null;
    private void SpeakNextLine(string nextLine, BaseActor speaker)
    {
        // Stop the Coroutine
        this.EnsureCoroutineStopped(ref lineRoutine);
        dialogTextAnimations.isTextAnimating = false;

        List<DialogCommand> commands = DialogUtility.ProcessMessage(nextLine, out string processedMessage);
        lineRoutine = StartCoroutine(dialogTextAnimations.AnimateTextIn(commands, processedMessage, speaker));
    }

    public void HandlePreTextTags(string[] tags, ref string speakerDirection, ref float pBefore, ref List<int> actorsToAnimate,
    ref string animToPlay, ref bool waitForAnim, ref string vcType, ref int vcRate, ref int boxNumber)
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
                    {
                        actorsToAnimate.Add(thisID);
                    }
                    else
                    {
                        Debug.LogError("Animation actor ID is not an integer!");
                    }
                }
            }
            else if (tagKey == "vc")
            {
                // VC Type is 0, VC Rate is 1 
                List<string> allValues = tagValue.Split(",").ToList();
                vcType = allValues[0];

                int rate = 0;
                if (int.TryParse(allValues[1], out rate))
                {
                    vcRate = rate;
                }
                else
                {
                    Debug.LogError("VC Rate is not an integer - going with default rate!");
                }
            }
            else if (tagKey == "bub")
            {
                if (int.TryParse(tagValue, out int parsedNumber))
                {
                    boxNumber = parsedNumber;
                }
            }
            else
            {
                Debug.Log("No additional Pre-Text tag found!");
            }
        }

        DialogManager.Instance.SetDialogBoxNumber(boxNumber);
    }

    public IEnumerator EndDialog()
    {
        if (boxIsActive)
        {
            AnimatingDialogBox = true;
            StartCoroutine(AnimateUIToSize(grow:false));
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
    public void SetFading(bool state)
    {
        fading = state; 
    }
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
    public bool CheckIfFading()
    {
        return fading; 
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
