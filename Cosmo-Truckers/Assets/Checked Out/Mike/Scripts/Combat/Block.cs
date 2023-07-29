using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        DestroyMe(collision);
    }

    protected virtual void DestroyMe(Collider2D collision)
    {
        if (collision.tag == "PlayerAttack")
        {
            Destroy(gameObject);
        }
    }
}
