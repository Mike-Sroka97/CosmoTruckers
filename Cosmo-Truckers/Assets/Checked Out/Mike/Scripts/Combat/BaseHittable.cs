using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseHittable : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "PlayerAttack")
        {
            ReceiveHit();
        }
    }

    protected virtual void ReceiveHit()
    {
        Destroy(gameObject);
    }
}
