using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectroWhip : CombatMove
{
    [SerializeField] ElectroWhipEnemy[] enemies;

    private void Start()
    {
        Score = enemies.Length;
        StartMove();
        GenerateLayout();
    }

    public override void EndMove()
    {
        throw new System.NotImplementedException();
    }
}
