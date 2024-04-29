using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaliteBonkArm : MonoBehaviour
{
    [SerializeField] GameObject shockwave;
    
    public void SpawnShockWave()
    {
        Transform shockSpawn = transform.parent.Find("shockSpawn");
        Instantiate(shockwave, shockSpawn);
    }
}
