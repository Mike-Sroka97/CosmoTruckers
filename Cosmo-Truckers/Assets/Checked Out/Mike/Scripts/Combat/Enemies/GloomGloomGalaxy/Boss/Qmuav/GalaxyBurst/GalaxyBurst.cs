using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GalaxyBurst : CombatMove
{
    [SerializeField] GameObject ilk;

    private void Start()
    {
        GenerateLayout();
    }

    public override void StartMove()
    {
        GetComponentInChildren<GravityManager>().Initialize();

        foreach (Graviton graviton in GetComponentsInChildren<Graviton>())
            graviton.enabled = true;

        BasicProjectileSpawner[] spawners = FindObjectsOfType<BasicProjectileSpawner>();
        foreach (BasicProjectileSpawner spawner in spawners)
            spawner.StartSpawning(); 

        base.StartMove();
    }

    public override void EndMove()
    {
        if (FindObjectOfType<InaPractice>())
            return;

        MoveEnded = true;

        int currentNumberOfIlkToSpawn = Score;

        for (int i = 8; i <= 11; i++)
        {
            if (EnemyManager.Instance.EnemyCombatSpots[i] == null)
            {
                currentNumberOfIlkToSpawn--;
                EnemyManager.Instance.UpdateEnemySummons(ilk);
            }

            if (currentNumberOfIlkToSpawn <= 0)
                return;
        }
    }
}
