using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TripleTether : CombatMove
{
    private void Start()
    {
        GenerateLayout();
    }

    public override void StartMove()
    {
        TripleTetherEnemy[] enemies = FindObjectsOfType<TripleTetherEnemy>();
        foreach (TripleTetherEnemy enemy in enemies)
            enemy.Intialize();
    }

    public override void EndMove()
    {
        base.EndMove();

        foreach(Enemy enemy in CombatManager.Instance.CharactersSelected)
        {
            enemy.TauntedBy = CombatManager.Instance.GetCurrentPlayer;
        }
    }
}
    
