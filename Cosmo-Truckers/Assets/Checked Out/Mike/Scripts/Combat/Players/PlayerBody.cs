using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBody : MonoBehaviour
{
    Player body;

    private void Start()
    {
        body = GetComponentInParent<Player>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "EnemyDamaging" && !body.iFrames)
        {
            body.TakeDamage();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "EnemyDamaging" && !body.iFrames)
        {
            body.TakeDamage();
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "EnemyDamaging" && !body.iFrames)
        {
            body.TakeDamage();
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.transform.tag == "EnemyDamaging" && !body.iFrames)
        {
            body.TakeDamage();
        }
    }
}
