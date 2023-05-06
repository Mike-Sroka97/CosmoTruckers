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
    private void OnCollisionEnter2D(Collision2D collision)
    {
        dog.StretchingCollision(collision.gameObject.tag);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        dog.StretchingCollision(collision.gameObject.tag);
    }
}
