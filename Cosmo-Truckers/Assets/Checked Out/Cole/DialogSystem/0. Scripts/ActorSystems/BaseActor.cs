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
    [SerializeField] Transform textBoxPosition;
    [SerializeField] SpeakingDirection myDirection;
    public string actorName;
    public int actorID;
    public Material actorTextMaterial; 

    [Header("Standard Voice Barks")]
    [SerializeField] private AudioClip[] normal; 
    [SerializeField] private AudioClip[] low; 
    [SerializeField] private AudioClip[] high; 
    [SerializeField] private AudioClip[] nervous;
    [SerializeField] private int defaultVoiceRate = 3;

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
    public void DeliverLine(string actorsLine, int lastID, bool firstDialog, string direction, float waitTime = 0f)
    {
        bool sameSpeaker = false;
        
        // If direction is empty, set it to this direction
        if (direction == string.Empty)
            direction = myDirection.ToString();
        else
        {
            // If direciton isn't one of the base directions, set it to this direction
            if (direction != "left" && direction != "right" && direction != "none")
                direction = myDirection.ToString();
        }

        // If it's the same speaker, we don't want the box to just sit there if waiting 
        if (actorID == lastID && waitTime == 0)
            sameSpeaker = true;

        float timeToWait = 0.5f;
        if (waitTime > timeToWait)
            timeToWait = waitTime; 

        StartCoroutine(DialogManager.Instance.StartNextDialog(actorsLine, this, actorTextMaterial, textBoxPosition,
            sameSpeaker, firstDialog, waitTimeBetweenDialogs: timeToWait, actorDirection: direction)); 
    } 

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
            voiceBarks = low.ToList<AudioClip>();
        else if (vcType == "high")
            voiceBarks = high.ToList<AudioClip>();
        else if (vcType == "nervous")
            voiceBarks = nervous.ToList<AudioClip>();
        else if (vcType == "unique1")
            voiceBarks.Add(unique1); 
        else 
            voiceBarks = normal.ToList<AudioClip>();

        return voiceBarks; 
    }
    public int GetVoiceBarkRate(int vcRate)
    {
        if (vcRate == -1)
            voiceRate = defaultVoiceRate;
        else if (vcRate == -2)
            voiceRate = 200; // Maybe change this, setting it so high that it'll never play again
        else
            voiceRate = vcRate;

        return voiceRate; 
    }
    #endregion

    #region Animation
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

public enum SpeakingDirection
{
    left,
    right,
    none
}