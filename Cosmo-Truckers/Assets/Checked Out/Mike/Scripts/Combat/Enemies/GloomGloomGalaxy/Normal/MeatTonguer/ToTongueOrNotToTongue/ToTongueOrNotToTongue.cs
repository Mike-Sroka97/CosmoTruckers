using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToTongueOrNotToTongue : CombatMove
{
    [SerializeField] float maxTime;

    Rotator myRotator;
    float currentTime = 0;

    private void Start()
    {
        StartMove();
        GenerateLayout();
        myRotator = GetComponentInChildren<Rotator>();
        int random = UnityEngine.Random.Range(0, 2);
        if(random == 1)
        {
             myRotator.RotateSpeed = -myRotator.RotateSpeed;
        }
    }

    private void Update()
    {
        TrackTime();
    }

    private void TrackTime()
    {
        if (MoveEnded)
            return;

        currentTime += Time.deltaTime;

        if (currentTime >= maxTime)
        {
            currentTime = maxTime;
            EndMove();
        }
        else if(PlayerDead)
        {
            EndMove();
        }
    }

    public override void EndMove()
    {
        MoveEnded = true;
        Score = (int)currentTime;
        Debug.Log(Score);
    }
}
