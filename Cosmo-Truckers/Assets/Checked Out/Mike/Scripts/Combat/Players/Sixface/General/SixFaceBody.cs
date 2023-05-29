using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SixFaceBody : MonoBehaviour
{
    SixfaceINA sixFace;

    private void Start()
    {
        sixFace = GetComponentInParent<SixfaceINA>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "EnemyDamaging" && !sixFace.iFrames)
        {
            sixFace.TakeDamage();
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "EnemyDamaging" && !sixFace.iFrames)
        {
            sixFace.TakeDamage();
        }
    }
}
