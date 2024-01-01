using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DutifulDrossPassive : EnemyPassiveBase
{
    [SerializeField] Enemy myEnemy;

    private void Start()
    {
        if(myEnemy)
        {
            GetComponent<DutifulDrossAI>().ProtectedEnemy = myEnemy;
        }
        else
        {
            foreach (Enemy enemy in EnemyManager.Instance.Enemies)
                if (enemy.CharacterName != "Dutiful Dross")
                    GetComponent<DutifulDrossAI>().ProtectedEnemy = enemy;
        }

        //TODO Draw line between dross and protected enemy
    }
}
