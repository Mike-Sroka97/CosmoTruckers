using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectroWhip : CombatMove
{
    ElectroWhipEnemy[] enemies;

    private void Start()
    {
        GenerateLayout();
    }

    public override void StartMove()
    {
        enemies = FindObjectsOfType<ElectroWhipEnemy>();
        Score = enemies.Length;

        foreach (ElectroWhipEnemy enemy in enemies)
            enemy.Initialize();

        base.StartMove();
    }

    public override void EndMove()
    {
        if (FindObjectOfType<InaPractice>())
            return;

        base.EndMove();
        CombatManager.Instance.CharactersSelected[0].GetComponent<Enemy>().TauntedBy = CombatManager.Instance.GetCurrentPlayer;
    }

    public override string TrainingDisplayText => $"You scored {Score = (Score > maxScore ? maxScore : Score)}/{maxScore} dealing {Score * Damage + baseDamage}, and you taunted your target.";
}
