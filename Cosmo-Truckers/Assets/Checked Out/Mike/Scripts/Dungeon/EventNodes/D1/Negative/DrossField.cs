using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrossField : EventNodeBase
{
    [SerializeField] string responseText;

    public void AcceptOption()
    {        
        descriptionText.text = responseText;
        StartCoroutine(SelectionChosen());
    }
}
