using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cometkaze : CombatMove
{
    [SerializeField] float oneScoreTime;
    [SerializeField] float maxScoreTime;

    private void Start()
    {
        GenerateLayout();
    }

    public override void StartMove()
    {
        CometkazeBall[] balls = GetComponentsInChildren<CometkazeBall>();

        foreach (CometkazeBall ball in balls)
            ball.Move = true;

        trackTime = true;

        base.StartMove();
    }

    public override void EndMove()
    {
        MoveEnded = true;

        if(currentTime > maxScoreTime)
        {
            currentTime = maxScoreTime;
            Score = 0;
        }
        else if (currentTime > oneScoreTime)
        {
            Score = 1;
        }
        else
        {
            Score = 2;
        }

        int damage = CalculateScore();
        DealDamageOrHealing(CombatManager.Instance.GetCharactersSelected[0], damage);
        CombatManager.Instance.GetCharactersSelected[1].TakeDamage(999, true);
    }
}
