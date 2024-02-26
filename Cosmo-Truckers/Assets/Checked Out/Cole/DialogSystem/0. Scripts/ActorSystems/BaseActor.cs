using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class BaseActor : MonoBehaviour
{
    //Variables
    [SerializeField] Material actorTextMaterial;
    [SerializeField] Animation[] actorStates;
    [SerializeField] Transform textBoxPosition;
    [SerializeField] SpeakingDirection myDirection; 
    public string actorName;
    public int actorID;

    private Animator myAnimator;
    private bool isAnimating = false; 

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
    public void DeliverLine(string actorsLine, int lastID, bool firstDialog, string direction)
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

        if (actorID == lastID)
            sameSpeaker = true; 

        StartCoroutine(DialogManager.Instance.StartNextDialog(actorsLine, actorName, actorTextMaterial, textBoxPosition, 
            sameSpeaker, firstDialog, actorDirection: direction)); 
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

    #region Animation
    public void StartAnimation(string animationName)
    {
        string animationToPlay = ActorAnimationFinder.ReturnAnimationName(animationName);

        isAnimating = true; 
    }
    IEnumerator AnimateActor()
    {
        yield return null; 
    }
    public void SetAnimation(int actorType)
    {

    }
    #endregion
}

public enum SpeakingDirection
{
    left,
    right,
    none
}