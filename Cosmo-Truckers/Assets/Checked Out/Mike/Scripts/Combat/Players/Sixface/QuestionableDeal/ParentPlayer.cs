using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParentPlayer : MonoBehaviour
{
    [SerializeField] Rigidbody2D myBody;

    private void Start()
    {
        if(myBody == null)
        {
            myBody = FindObjectOfType<CombatMove>().GetComponent<Rigidbody2D>();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "Player")
        {
            if(collision.transform.GetComponentInParent<Player>())
            {
                collision.transform.GetComponentInParent<Player>().xVelocityAdjuster = myBody.velocity.x;
                collision.transform.GetComponentInParent<Player>().yVelocityAdjuster = myBody.velocity.y;
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.transform.tag == "Player")
        {
            if (collision.transform.GetComponentInParent<Player>())
            {
                collision.transform.GetComponentInParent<Player>().xVelocityAdjuster = 0;
                collision.transform.GetComponentInParent<Player>().yVelocityAdjuster = 0;
            }
        }
    }
}
