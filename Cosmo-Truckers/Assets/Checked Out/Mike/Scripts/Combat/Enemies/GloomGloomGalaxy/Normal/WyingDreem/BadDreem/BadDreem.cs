using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BadDreem : CombatMove
{
    [SerializeField] float maxTime;

    [HideInInspector] public float CurrentScore;

    private void Start()
    {
        GenerateLayout();
        CurrentScore = Score;
        trackTime = false;
        GetComponentInChildren<BadDreemDarkness>().StartWaiting();
    }

    public override void StartMove()
    {
        GetComponentInChildren<BadDreemLight>().enabled = true;
        trackTime = true;

        base.StartMove();
    }

    private void Update()
    {
        if (!trackTime)
            return;

        TrackTime();
        TrackScore();
    }

    private void TrackScore()
    {
        if (MoveEnded)
            return;

        if (CurrentScore > maxScore)
            MoveEnded = true;
    }

    public override void EndMove()
    {
        MoveEnded = true;
        CurrentScore += 0.5f;
        Score = (int)CurrentScore;

        int damage = CalculateScore();

        DealDamageOrHealing(CombatManager.Instance.GetCharactersSelected[0], damage);
        CombatManager.Instance.GetCharactersSelected[0].AddDebuffStack(DebuffToAdd);
    }
}
