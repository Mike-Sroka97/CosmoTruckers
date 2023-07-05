using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProtoBody : MonoBehaviour
{
    ProtoINA proto;

    private void Start()
    {
        proto = GetComponentInParent<ProtoINA>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "EnemyDamaging" && !proto.iFrames)
        {
            proto.TakeDamage();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "EnemyDamaging" && !proto.iFrames)
        {
            proto.TakeDamage();
        }
    }
}

