using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtomicImpactWave : MonoBehaviour
{
    AtomicImpact minigame;

    private void Start()
    {
        minigame = FindObjectOfType<AtomicImpact>();
    }

    private void Update()
    {
        if(GetComponentsInChildren<QmuavProjectile>().Length <= 0)
        {
            minigame.GenerateNextWave();
            Destroy(gameObject);
        }
    }
}
