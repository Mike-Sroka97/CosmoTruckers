using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafeTSpring : MonoBehaviour
{
    [SerializeField] float springForce;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponentInParent<SafeTINA>())
        {
            //play animation
            collision.GetComponentInParent<Rigidbody2D>().velocity = new Vector2(collision.GetComponentInParent<Rigidbody2D>().velocity.x, springForce);
        }
    }
}
