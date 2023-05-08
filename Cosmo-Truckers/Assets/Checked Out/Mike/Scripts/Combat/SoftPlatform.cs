using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoftPlatform : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.otherCollider.bounds.min.y > GetComponent<BoxCollider2D>().bounds.max.y)
        {
            Physics2D.IgnoreCollision(collision.otherCollider, GetComponent<BoxCollider2D>());
        }
    }
}
