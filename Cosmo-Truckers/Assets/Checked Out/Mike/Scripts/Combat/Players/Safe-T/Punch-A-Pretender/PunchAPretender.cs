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

    private void Start()
    {
        GenerateLayout();
    }

    public override void StartMove()
    {
        trackTime = true;

        base.StartMove();
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
        if (FindObjectOfType<InaPractice>())
            return;

        MoveEnded = true;

        if (CombatManager.Instance != null) //In the combat screen
        {
            SafeTCharacter character = CombatManager.Instance.GetCurrentPlayer.GetComponent<SafeTCharacter>();
            int numberOfHits = CalculateNumberOfHits();

            //1 being base damage
            float DamageAdj = 1;

            //Damage on players must be divided by 100 to multiply the final
            DamageAdj = CombatManager.Instance.GetCurrentCharacter.Stats.Damage / 100;

            character.GetComponent<Character>().TakeMultiHitDamage(baseDamage / numberOfHits + character.FlatDamageAdjustment, numberOfHits);
        }
    }

    private int CalculateNumberOfHits()
    {

        //Calculate Damage 
        if (Score >= twoScoreValue)
        {
            Score = 2;
            return twoScoreValue  + baseNumberOfAttacks;
        }
        else if (Score >= oneScoreValue)
        {
            Score = 1;
            return oneScoreValue + baseNumberOfAttacks;
        }
        else
        {
            Score = 0;
            return 2;
        }
    }

    public override string TrainingDisplayText => $"You scored {Score = (Score > maxScore ? maxScore : Score)}/{maxScore} hitting yourself ({CalculateNumberOfHits()}) times and generating ({CalculateNumberOfHits()}) rage.";
}
