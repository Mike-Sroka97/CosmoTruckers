using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpitBall : CombatMove
{
    [SerializeField] float maxTime;


    private void Start()
    {
        GenerateLayout();
    }

    public override void StartMove()
    {
        BallBounce[] balls = GetComponentsInChildren<BallBounce>();

        foreach (BallBounce ball in balls)
            ball.enabled = true;

        base.StartMove();
        SetupMultiplayer();
    }

    public override void EndMove()
    {
        MoveEnded = true;

        for (int i = 0; i < CombatManager.Instance.GetCharactersSelected.Count; i++)
        {
            int tempScore = PlayerScores[CombatManager.Instance.GetCharactersSelected[i].GetComponent<PlayerCharacter>()];
            tempScore = CalculateMultiplayerScore(tempScore);

            DealDamageOrHealing(CombatManager.Instance.GetCharactersSelected[i], tempScore);
            ApplyAugment(CombatManager.Instance.GetCharactersSelected[i], baseAugmentStacks);
        }
    }
}
