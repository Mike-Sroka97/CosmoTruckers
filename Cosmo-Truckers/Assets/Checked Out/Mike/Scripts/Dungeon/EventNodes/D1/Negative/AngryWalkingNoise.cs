using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AngryWalkingNoise : EventNodeBase
{
    [SerializeField] GameObject Timmy;
    [SerializeField] string responseText;

    public void NoOption()
    {
        EnemyManager.Instance.EnemySummonsToSpawn.Add(Timmy);
        descriptionText.text = responseText;
        StartCoroutine(SelectionChosen());
    }
}
