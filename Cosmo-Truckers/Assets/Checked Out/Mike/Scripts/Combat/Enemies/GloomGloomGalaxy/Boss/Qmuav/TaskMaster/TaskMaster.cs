using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskMaster : CombatMove
{
    private void Start()
    {
        GenerateLayout();
    }

    public override void StartMove()
    {
        SetupMultiplayer();

        GetComponentInChildren<GravityManager>().Initialize();

        foreach (Graviton graviton in GetComponentsInChildren<Graviton>())
            graviton.enabled = true;

        base.StartMove();
    }

    public void SetScore()
    {
        Score = (int)currentTime;
        EndMove();
    }
}
