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
    }

    public override void EndMove()
    {
        base.EndMove();
        CombatManager.Instance.CharactersSelected[0].GetComponent<Enemy>().TauntedBy = CombatManager.Instance.GetCurrentPlayer;
    }
}
