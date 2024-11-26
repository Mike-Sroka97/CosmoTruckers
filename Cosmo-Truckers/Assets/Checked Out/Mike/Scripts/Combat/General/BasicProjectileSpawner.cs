using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// Spawn projectiles, typically falling or rising ones, randomly out of an array of positions
/// </summary>
public class BasicProjectileSpawner : MonoBehaviour
{
    [SerializeField] float timeBetweenSpawn = 0.75f;
    [SerializeField] Transform[] spawnPositions;
    [SerializeField] GameObject projectile; 

    private float timer;
    private int previousSpawn = -1;
    private bool startSpawning = false;

    CombatMove minigame; 

    // Start is called before the first frame update
    void Start()
    {
        minigame = FindObjectOfType<CombatMove>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!startSpawning)
            return;

        timer += Time.deltaTime; 

        if (timer > timeBetweenSpawn)
        {
            timer = 0;
            SpawnProjectile();
        }
    }

    public void StartSpawning()
    {
        startSpawning = true;
    }

    public void SpawnProjectile()
    {
        int spawnPos = previousSpawn;

        while (spawnPos == previousSpawn)
        {
            spawnPos = UnityEngine.Random.Range(0, spawnPositions.Length);
        }

        previousSpawn = spawnPos;

        Instantiate(projectile, spawnPositions[spawnPos].position, Quaternion.identity, minigame.transform);
    }
}
