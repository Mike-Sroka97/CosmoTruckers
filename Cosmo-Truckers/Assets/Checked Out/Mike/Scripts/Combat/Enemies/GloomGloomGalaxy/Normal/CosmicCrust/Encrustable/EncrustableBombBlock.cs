using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EncrustableBombBlock : Block
{
    [SerializeField] Collider2D blastZone;
    [SerializeField] float explosionDuration;

    protected override void DestroyMe(Collider2D collision)
    {
        if (collision.tag == "PlayerAttack")
        {
            StartCoroutine(Blast());
        }
    }

    IEnumerator Blast()
    {
        blastZone.enabled = true;
        blastZone.GetComponent<SpriteRenderer>().enabled = true;
        yield return new WaitForSeconds(explosionDuration);
        Destroy(gameObject);
    }
}
