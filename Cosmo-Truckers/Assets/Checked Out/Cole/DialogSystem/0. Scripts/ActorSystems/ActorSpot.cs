using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorSpot : MonoBehaviour
{
    [SerializeField] private int sortingLayerOrder;
    [SerializeField] private bool isFacingRight = true;
    [SerializeField] private int actorNumber; 

    public int GetSortingLayer() { return sortingLayerOrder; }
    public bool GetFacingRight() { return isFacingRight; }
    public int GetActorNumber() { return actorNumber; }
}
