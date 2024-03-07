using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour
{
    public static DialogManager Instance;

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

    [Header("Dialog Scene Loading")]
    [SerializeField] private Image fader; 
    private const float FadeTime = 2f;
    private bool fading = false; 

    // Dialog Scene Variables
    private GameObject sceneLayout;
    TextAsset textFile;
    List<BaseActor> playerActors;

    // Audio Variables
    private AudioSource audioSource;

    private DialogTextAnimations dialogTextAnimations;
    private string currentLine;
    private List<BaseActor> currentActorsToAnimate; 
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
    #endregion

    #region Dialog Mode
    public IEnumerator StartNextDialog(string nextLine, BaseActor speaker, Material borderMaterial, 
        Transform textBoxPosition, bool sameSpeaker = false, bool firstDialog = false, 
        float waitTimeBetweenDialogs = 0.5f, string actorDirection = "left")
    {
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
        dialogText.text = string.Empty;
        displayNameText.text = string.Empty;

        if (dialogTextAnimations != null)
        {
            dialogTextAnimations.ClearText();
        }
    }
    public bool CheckIfDialogTextAnimating()
    {
        return dialogTextAnimations.isTextAnimating;
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
}
