using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldenFury : CombatMove
{
    [SerializeField] int maxHealthReduction = -10;

    private void Start()
    {
        GenerateLayout();
    }

    public override void StartMove()
    {
        SetupMultiplayer();
        base.StartMove();

        foreach (GoldenFuryBall ball in GetComponentsInChildren<GoldenFuryBall>())
            ball.enabled = true;

        base.StartMove();
    }

    public override void EndMove()
    {
        if (FindObjectOfType<InaPractice>())
            return;

        MoveEnded = true;

        for (int i = 0; i < CombatManager.Instance.GetCharactersSelected.Count; i++)
        {
            CombatManager.Instance.GetCharactersSelected[i].AdjustMaxHealth(maxHealthReduction);

            int damage = CalculateMultiplayerScore(PlayerScores[CombatManager.Instance.GetCharactersSelected[i].GetComponent<PlayerCharacter>()]);
            DealDamageOrHealing(CombatManager.Instance.GetCharactersSelected[i], damage);
        }
    }
}
