using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;

public class BaseActor : MonoBehaviour
{
    //Variables
    [Header("Main Actor Variables")]
    [SerializeField] Animation[] actorStates;
    public Transform textBoxPosition; 
    bool hasDirection;
    public string actorName;
    public int actorID;

    [Header("Visual Variables")]
    public Material actorTextMaterial;
    public Sprite actorNextIndicator; 

    [Header("Standard Voice Barks")]
    [SerializeField] private AudioClip[] normal; 
    [SerializeField] private AudioClip[] low; 
    [SerializeField] private AudioClip[] high; 
    [SerializeField] private AudioClip[] nervous;
    [SerializeField] private int defaultNormalVoiceRate = 3;
    [SerializeField] private int defaultLowVoiceRate = 3;
    [SerializeField] private int defaultHighVoiceRate = 3;

    private int currentVoiceRate = 3;
    private const float defaultWaitTimeBetweenDialogs = 1.35f; 

    [Header("Unique Voice Barks")]
    [SerializeField] private AudioClip unique1;

    private int voiceRate = 3; // 3 is the fallback as the default
    private List<AudioClip> voiceBarks = new List<AudioClip>(); 

    private Animator myAnimator;
    private AnimationClip animationToPlay;

    public void Initialize(int sortingLayerOrder, bool isFacingRight = true)
    {
        SetSpriteSorting(sortingLayerOrder); 

        if (!isFacingRight)
        {
            transform.eulerAngles = new Vector3(transform.rotation.x, 180f, transform.rotation.z); 
        }

        myAnimator = GetComponentInChildren<Animator>();
    }

    //set text box active and material 
    public void DeliverLine(string actorsLine, int lastID, bool firstDialog, bool direction, float waitTime = 0f)
    {
        bool sameSpeaker = false;

        // If it's the same speaker, we don't want the box to just sit there if waiting 
        if (actorID == lastID && waitTime == 0)
            sameSpeaker = true;

        // Use the default wait time between dialogs unless the specified wait time is longer
        float timeToWait = defaultWaitTimeBetweenDialogs;
        if (waitTime > timeToWait)
            timeToWait = waitTime;

        StartCoroutine(DialogManager.Instance.StartNextDialog(actorsLine, this, actorTextMaterial, textBoxPosition,
            sameSpeaker, firstDialog, waitTimeBetweenDialogs: timeToWait, actorDirection: direction)); 
    } 

    /// <summary>
    /// This is for sorting the spawned actors based on which spot they are in, so no overlap happens.
    /// </summary>
    /// <param name="sortingLayer"></param>
    private void SetSpriteSorting(int sortingLayer)
    {
        SortingGroup sortingGroup = GetComponentInChildren<SortingGroup>();
        if (sortingGroup != null )
        {
            sortingGroup.sortingOrder = sortingLayer; 
        }

        else
        {
            SpriteRenderer[] sprites = GetComponentsInChildren<SpriteRenderer>();

            foreach (SpriteRenderer sprite in sprites)
            {
                sprite.sortingOrder = sortingLayer; 
            }
        }
    }

    #region Voice
    public List<AudioClip> GetVoiceBarkType(string vcType)
    {
        // Swap between the types depending on what is passed in. Normal will be chosen last
        if (vcType == "low")
        {
            voiceBarks = low.ToList<AudioClip>();
            currentVoiceRate = defaultLowVoiceRate;
        }
        else if (vcType == "high")
        {
            voiceBarks = high.ToList<AudioClip>();
            currentVoiceRate = defaultHighVoiceRate;
        }
        else if (vcType == "nervous")
            voiceBarks = nervous.ToList<AudioClip>();
        else if (vcType == "unique1")
            voiceBarks.Add(unique1);
        else
        {
            voiceBarks = normal.ToList<AudioClip>();
            currentVoiceRate = defaultNormalVoiceRate;
        }

        return voiceBarks; 
    }
    public int GetVoiceBarkRate(int vcRate)
    {
        // If no rate is entered, choose a default rate based on voice bark type
        if (vcRate == -1)
            voiceRate = currentVoiceRate;
        // Maybe change this, setting it so high that it'll never play again
        else if (vcRate == -2)
            voiceRate = 200;
        else
            voiceRate = vcRate;

        return voiceRate; 
    }
    #endregion

    #region Animation
    /// <summary>
    /// Pass in an animation name for a character and get the time of the animation
    /// </summary>
    /// <param name="animationName"></param>
    /// <param name="animationTime"></param>
    public void GetAnimationInfo(string animationName, ref float animationTime)
    {
        // Players will always be ID #'s 1 - 4
        bool isPlayer = false; 

        if (actorID < 5)
            isPlayer = true;

        // Pass in the gameobject name for player animations
        animationToPlay = ActorAnimationFinder.ReturnAnimationName(animationName, gameObject.name, myAnimator, isPlayer);
        animationTime = animationToPlay.length; 
    }
    public void ClearAnimationInfo()
    {
        animationToPlay = null; 
    }
    public void StartAnimation()
    {
        myAnimator.Play(animationToPlay.name);
    }
    #endregion
}