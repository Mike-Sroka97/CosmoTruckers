using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathParticleSpawner : MonoBehaviour
{
    [SerializeField] GameObject deathParticle;
    [SerializeField] bool isTrigger = true;
    [SerializeField] bool noSpecialCase = false;

    bool hasSpawned = false; 
    CombatMove minigame; 

    // Start is called before the first frame update
    void Start()
    {
        minigame = FindObjectOfType<CombatMove>();
    }

    public void SpawnDeathParticle(Transform spawnPosition)
    {
        //Only spawn once
        if (!hasSpawned)
        {
            hasSpawned = true;
            Instantiate(deathParticle, spawnPosition.position, deathParticle.transform.rotation, minigame.transform);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isTrigger && !noSpecialCase)
        {
            if (collision.gameObject.tag == "PlayerAttack")
            {
                SpawnDeathParticle(transform);
            }
        }       
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isTrigger && !noSpecialCase)
        {
            if (collision.gameObject.tag == "PlayerAttack")
            {
                SpawnDeathParticle(transform);
            }
        }
    }

}
