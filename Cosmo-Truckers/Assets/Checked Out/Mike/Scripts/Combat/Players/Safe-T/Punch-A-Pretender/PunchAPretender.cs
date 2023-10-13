using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PunchAPretender : CombatMove
{
    public int MaxScore;

    [SerializeField] PaPConveyorPart[] papNodes;
    [SerializeField] float timeBetweenBadNodes;
    [SerializeField] float timeBetweenHittableNodes;

    float currentTimeBadNodes = 0;
    float currentTimeHittableNodes = 0;
    bool trackTime = false;

    private void Start()
    {
        GenerateLayout();
    }

    public override void StartMove()
    {
        trackTime = true;
    }

    private void Update()
    {
        if (!trackTime)
            return;
        TrackTime();
    }

    protected override void TrackTime()
    {
        currentTimeBadNodes += Time.deltaTime;
        currentTimeHittableNodes += Time.deltaTime;

        bool isPossible = false;
        foreach(PaPConveyorPart part in papNodes)
        {
            if(!part.NodeActive)
            {
                isPossible = true;
                break;
            }
        }

        if(currentTimeBadNodes > timeBetweenBadNodes)
        {
            currentTimeBadNodes = 0;
            int random = UnityEngine.Random.Range(0, papNodes.Length);
            while(papNodes[random].NodeActive && isPossible)
            {
                random = UnityEngine.Random.Range(0, papNodes.Length);
            }
            papNodes[random].StartFlash(true);
        }
        else if(currentTimeHittableNodes > timeBetweenHittableNodes)
        {
            currentTimeHittableNodes = 0;
            int random = UnityEngine.Random.Range(0, papNodes.Length);
            while (papNodes[random].NodeActive && isPossible)
            {
                random = UnityEngine.Random.Range(0, papNodes.Length);
            }
            papNodes[random].StartFlash(false);
        }
    }
}
