using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSpawner : MonoBehaviour
{
    [SerializeField] GameObject particleToSpawn;
    [SerializeField] bool isTrigger = true;

    [Header("If nSC is true, OnTrigger/Collision will spawn Particles")]
    [SerializeField] bool noSpecialCase = false;

    [Header("DeathParticle only spawns once")]
    [SerializeField] bool deathParticle = true;

    [Header("Success Attackable vs Success")]
    [SerializeField] bool attackable = false;


    bool hasSpawned = false; 
    CombatMove minigame; 

    // Start is called before the first frame update
    void Start()
    {
        minigame = FindObjectOfType<CombatMove>();
    }

    public void SpawnParticle(Transform spawnPosition)
    {
        Instantiate(particleToSpawn, spawnPosition.position, particleToSpawn.transform.rotation, minigame.transform);
    }

    public void SpawnDeathParticle(Transform spawnPosition)
    {
        //Only spawn once
        if (!hasSpawned)
        {
            hasSpawned = true;
            SpawnParticle(spawnPosition); 
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isTrigger && noSpecialCase)
        {
            ParticleChecks(collision.gameObject);
        }       
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isTrigger && noSpecialCase)
        {
            ParticleChecks(collision.gameObject); 
        }
    }


    private void ParticleChecks(GameObject collision)
    {
        if (attackable)
        {
            if (collision.tag == "PlayerAttack")
            {
                if (deathParticle)
                {
                    SpawnDeathParticle(transform);
                }
                else
                {
                    SpawnParticle(transform);
                }
            }
        }
        else
        {
            if (collision.tag == "Player")
            {
                if (deathParticle)
                {
                    SpawnDeathParticle(transform);
                }
                else
                {
                    SpawnParticle(transform);
                }
            }
        }
    }
}
