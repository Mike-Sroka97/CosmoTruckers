using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StretchySpineProjectile : MonoBehaviour
{
    //TODO Add Scoring

    [SerializeField] float moveSpeed;
    [SerializeField] float deadForceBoost;

    bool movingRight = true;
    Rigidbody2D myBody;
    private void Start()
    {
        myBody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if(movingRight)
        {
            transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            movingRight = false;
            myBody.velocity = Vector2.zero;
            myBody.gravityScale = 1;
            myBody.AddForce(new Vector2(-deadForceBoost, deadForceBoost), ForceMode2D.Impulse);
        }
    }
}
