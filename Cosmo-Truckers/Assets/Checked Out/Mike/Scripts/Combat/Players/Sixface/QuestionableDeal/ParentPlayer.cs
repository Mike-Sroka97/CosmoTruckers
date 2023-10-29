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
            Player player = collision.transform.GetComponentInChildren<Player>();
            if (player == null)
                player = collision.transform.GetComponentInParent<Player>();

            if(player != null)
            {
                player.xVelocityAdjuster = myBody.velocity.x;
                player.yVelocityAdjuster = myBody.velocity.y;
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.transform.tag == "Player")
        {
            Player player = collision.transform.GetComponentInChildren<Player>();
            if (player == null)
                player = collision.transform.GetComponentInParent<Player>();

            if (player != null)
            {
                player.xVelocityAdjuster = 0;
                player.yVelocityAdjuster = 0;
            }
        }
    }
}
