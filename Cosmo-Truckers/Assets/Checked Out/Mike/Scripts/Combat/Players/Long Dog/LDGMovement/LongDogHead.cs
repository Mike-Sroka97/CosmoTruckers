using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LongDogHead : MonoBehaviour
{
    //Extend this method to ass and neck as well
    LongDogINA dog;

    private void Start()
    {
        dog = GetComponentInParent<LongDogINA>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "EnemyDamaging")
        {
            dog.StretchingCollision(collision.gameObject.tag);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "EnemyDamaging")
        {
            dog.StretchingCollision(collision.gameObject.tag);
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.transform.tag == "EnemyDamaging")
        {
            dog.StretchingCollision(collision.gameObject.tag);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.transform.tag == "EnemyDamaging")
        {
            dog.StretchingCollision(collision.gameObject.tag);
        }
    }
}
