using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClockOutKnockOut : CombatMove
{
    float timeToEndMove = 1f; 

    private void Start()
    {
        GenerateLayout();
        StartMoveTest(); 
    }

    public override void StartMove()
    {
        COKOhand[] hands = FindObjectsOfType<COKOhand>();

        foreach(COKOhand hand in hands)
        {
            hand.SetVelocity();
        }
    }

    public void CheckScore()
    {
        if (Score >= maxScore)
        {
            float timeRemaining = MinigameDuration - currentTime; 

            if (timeRemaining > timeToEndMove)
            {
                StartCoroutine(CallEndMove()); 
            }
        }
    }

    private IEnumerator CallEndMove()
    {
        yield return new WaitForSeconds(timeToEndMove);
        EndMove(); 
    }

    public override void EndMove()
    {
        base.EndMove();
        FindObjectOfType<SafeTMana>().SetCurrentAnger(1);
    }
}
