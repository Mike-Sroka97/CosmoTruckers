using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShockingShock : CombatMove
{
    [SerializeField] float lightningDelay;
    [SerializeField] float maxMinigameTime = 15f;

    ShockingShockLightning[] lightning;
    float scoreTime = 0;
    int numberOfLightningToAssign;
    [HideInInspector] public int CurrentActivatedLightning = 0;
    bool trackTime = true;

    private void Start()
    {
        StartMove();
        GenerateLayout();
        lightning = GetComponentsInChildren<ShockingShockLightning>();
        numberOfLightningToAssign = lightning.Length - 1; //always activate all lightning minus one
    }

    private void Update()
    {
        TrackScoreTime();
        CheckShockStatus();
        TrackTime();
    }

    private void TrackScoreTime()
    {
        scoreTime += Time.deltaTime;

        if((scoreTime >= maxMinigameTime || PlayerDead) && !MoveEnded)
        {
            if(scoreTime >= maxMinigameTime)
                scoreTime = maxMinigameTime;

            EndMove();
        }
    }

    private void CheckShockStatus()
    {
        if(CurrentActivatedLightning > 0)
        {
            trackTime = false;
        }
        else
        {
            trackTime = true;
        }
    }

    protected override void TrackTime()
    {
        if (!trackTime)
            return;

        currentTime += Time.deltaTime;
        
        if(currentTime >= lightningDelay)
        {
            currentTime = 0;
            HandleLightning();
        }
    }

    private void HandleLightning()
    {
        foreach(ShockingShockLightning shock in lightning)
        {
            if(shock.AlwaysActivate)
            {
                shock.ActivateMe();
                shock.IsShocking = true;
            }
        }

        while(CurrentActivatedLightning < numberOfLightningToAssign)
        {
            int random = UnityEngine.Random.Range(0, lightning.Length);
            if(!lightning[random].IsShocking)
            {
                lightning[random].ActivateMe();
                lightning[random].IsShocking = true;
            }
        }
    }

    public override void EndMove()
    {
        MoveEnded = true;
        Score = (int)scoreTime;
        Debug.Log(scoreTime);
    }
}
