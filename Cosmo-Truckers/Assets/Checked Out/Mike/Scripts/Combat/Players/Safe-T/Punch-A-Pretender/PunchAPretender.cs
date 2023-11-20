using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PunchAPretender : CombatMove
{
    public int MaxScore;

    [SerializeField] PaPConveyorPart[] papNodes;
    [SerializeField] float timeBetweenBadNodes;
    [SerializeField] float timeBetweenHittableNodes;

    const int twoScoreValue = 4;
    const int oneScoreValue = 2;
    const int baseNumberOfAttacks = 2;

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

    public override void EndMove()
    {
        MoveEnded = true;

        if (CombatManager.Instance != null) //In the combat screen
        {
            SafeTCharacter character = CombatManager.Instance.GetCurrentPlayer.GetComponent<SafeTCharacter>();
            int numberOfHits;

            //Calculate Damage 
            if (Score >= twoScoreValue)
            {
                Score = 2;
                numberOfHits = twoScoreValue + baseNumberOfAttacks;
            }
            else if (Score >= oneScoreValue)
            {
                Score = 1;
                numberOfHits = oneScoreValue + baseNumberOfAttacks;
            }
            else
            {
                Score = 0;
                numberOfHits = 2;
            }

            //adjust number of hits and damage so that damage is static no matter the number of hits
            character.GetComponent<Character>().TakeMultiHitDamage(baseDamage / numberOfHits, numberOfHits);
        }
    }
}
