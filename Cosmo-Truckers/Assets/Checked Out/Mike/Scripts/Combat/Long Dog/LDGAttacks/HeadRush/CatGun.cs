using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatGun : MonoBehaviour
{
    [SerializeField] float fireOffset;
    [SerializeField] float fireCD;
    [SerializeField] GameObject projectile;
    [SerializeField] GameObject projectileSpawn;

    float currentTime = 0;

    private void Start()
    {
        float randomOffset = Random.Range(0.0f, fireOffset);
        currentTime += randomOffset;
    }
    private void Update()
    {
        if(currentTime >= fireCD)
        {
            Instantiate(projectile, projectileSpawn.transform);
            currentTime = 0;
        }
        currentTime += Time.deltaTime;
    }
}
