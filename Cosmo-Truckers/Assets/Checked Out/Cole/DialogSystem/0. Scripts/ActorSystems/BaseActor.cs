using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;

public class BaseActor : MonoBehaviour
{
    //Variables
    [SerializeField] Material actorTextMaterial;
    [SerializeField] Animation[] actorStates;
    [SerializeField] Transform textBoxPosition; 
    public string actorName;
    public int actorID;

    private DialogManager myDialogManager; 

    public void Initialize(DialogManager dialogManager, int sortingLayerOrder, bool isFacingRight = true)
    {
        myDialogManager = dialogManager;
        SetSpriteSorting(sortingLayerOrder); 

        if (!isFacingRight)
        {
            transform.eulerAngles = new Vector3(transform.rotation.x, 180f, transform.rotation.z); 
        }
    }

    //Methods
    public void SetAnimation(int actorType)
    {

    }

    //set text box active and material 
    public void DeliverLine(string actorsLine, int lastID, bool firstDialog)
    {
        bool sameSpeaker = false;

        if (actorID == lastID)
            sameSpeaker = true; 

        StartCoroutine(myDialogManager.StartNextDialog(actorsLine, actorName, actorTextMaterial, textBoxPosition, sameSpeaker, firstDialog)); 
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
}
