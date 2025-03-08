using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSpawner : MonoBehaviour
{
    [SerializeField] GameObject particleToSpawn;
    [SerializeField] bool isTrigger = true;

    /// <summary>
    /// If nSC is true, OnTrigger/Collision will spawn Particles
    /// </summary>
    [SerializeField] bool noSpecialCase = false;

    [Header("DeathParticle only spawns once")]
    [SerializeField] bool deathParticle = true;
    [SerializeField] bool attackable = false;
    [SerializeField] bool destroyOnContact = false;

    bool hasSpawned = false; 
    CombatMove minigame; 

    // Start is called before the first frame update
    void Start()
    {
        minigame = FindObjectOfType<CombatMove>();
    }

    public void SpawnParticle(Transform spawnPosition, bool deathParticle = false)
    {
        if (!deathParticle || (deathParticle && !hasSpawned))
        {
            hasSpawned = true;
            Instantiate(particleToSpawn, spawnPosition.position, particleToSpawn.transform.rotation, minigame.transform);
        }

        if (destroyOnContact)
            Destroy(gameObject);
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
            if (collision.CompareTag("PlayerAttack"))
            {
                if (deathParticle)
                    SpawnParticle(transform, true);
                else
                {
                    SpawnParticle(transform);
                }
            }
        }
        else
        {
            if (collision.CompareTag("Player"))
            {
                if (deathParticle)
                    SpawnParticle(transform, true);
                else
                {
                    SpawnParticle(transform);
                }
            }
        }
    }
}
