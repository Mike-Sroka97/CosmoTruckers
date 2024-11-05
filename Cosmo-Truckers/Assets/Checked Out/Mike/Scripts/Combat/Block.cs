using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    [SerializeField] GameObject particle; 

    private void OnTriggerEnter2D(Collider2D collision)
    {
        DestroyMe(collision);
    }

    protected virtual void DestroyMe(Collider2D collision)
    {
        if (collision.tag == "PlayerAttack")
        {
            SpawnParticle(); 
            Destroy(gameObject);
        }
    }

    protected void SpawnParticle()
    {
        if (particle != null)
        {
            Instantiate(particle, transform.position, Quaternion.identity, FindObjectOfType<CombatMove>().transform);
        }
    }
}
