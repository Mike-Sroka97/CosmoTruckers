using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CBStraightEnemy : MonoBehaviour
{
    [SerializeField] float xVelocity;

    Rigidbody2D myBody;

    private void Start()
    {
        myBody = GetComponent<Rigidbody2D>();
        myBody.velocity = new Vector2(xVelocity, myBody.velocity.y);
    }
}
