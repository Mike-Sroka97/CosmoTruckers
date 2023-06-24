using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SugarPillPlacebo : CombatMove
{
    [SerializeField] int maxScore = 3;
    [HideInInspector] public Vector3 CurrentCheckPointLocation;

    public override void EndMove()
    {
        throw new System.NotImplementedException();
    }

    private void Start()
    {
        StartMove();
    }

    private void Update()
    {
        if (Score == maxScore) //max score
        {
            EndMove();
        }
    }
}
