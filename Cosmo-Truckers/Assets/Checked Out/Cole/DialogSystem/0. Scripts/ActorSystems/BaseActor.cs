using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BaseActor : MonoBehaviour
{
    //Variables
    [SerializeField] Material actorTextMaterial;
    [SerializeField] Animation[] actorStates;
    [SerializeField] Transform textBoxPosition; 
    public string actorName;
    public int actorID;

    //Methods
    public void SetAnimation(int actorType)
    {

    }

    //set text box active and material 
    public void DeliverLine(string actorsLine)
    {

    } 
}
