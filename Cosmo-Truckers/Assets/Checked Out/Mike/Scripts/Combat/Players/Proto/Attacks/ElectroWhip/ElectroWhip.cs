using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectroWhip : CombatMove
{
    ElectroWhipEnemy[] enemies;

    private void Start()
    {
        StartMove();
        GenerateLayout();
        enemies = FindObjectsOfType<ElectroWhipEnemy>();
        Score = enemies.Length;
    }

    public override void EndMove()
    {
        throw new System.NotImplementedException();
    }
}
