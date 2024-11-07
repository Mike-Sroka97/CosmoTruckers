using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EncrustableBombBlock : Block
{
    [SerializeField] Collider2D blastZone;
    [SerializeField] float explosionDuration;
    [SerializeField] SpriteRenderer blockSprite;

    protected override void DestroyMe(Collider2D collision)
    {
        if (collision.tag == "PlayerAttack")
        {
            StartCoroutine(Blast());
        }
    }

    IEnumerator Blast()
    {
        blockSprite.enabled = false; 
        blastZone.enabled = true;
        SpawnParticle(); 
        yield return new WaitForSeconds(explosionDuration);
        Destroy(gameObject);
    }
}
