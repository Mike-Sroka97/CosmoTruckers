using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DutifulDrossPassive : EnemyPassiveBase
{
    [SerializeField] Enemy myEnemy;

    private void Start()
    {
        GetComponent<DutifulDrossAI>().ProtectedEnemy = myEnemy;
    }
}
