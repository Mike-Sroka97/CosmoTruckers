using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParentPlayer : MonoBehaviour
{
    [SerializeField] Rigidbody2D myBody;
    [SerializeField] bool permanentVelocityAdjustment;

    private void Start()
    {
        if(myBody == null)
        {
            myBody = FindObjectOfType<CombatMove>().GetComponent<Rigidbody2D>();
        }
    }

    public void AdjustPlayerVelocity(float velocityAdjustmentX, float velocityAdjustmentY, Player player = null)
    {
        if(player == null)
            player = FindObjectOfType<Player>();

        if (player != null)
        {
            player.xVelocityAdjuster = velocityAdjustmentX;
            player.yVelocityAdjuster = velocityAdjustmentY;
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
                AdjustPlayerVelocity(myBody.velocity.x, myBody.velocity.y, player);
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (permanentVelocityAdjustment)
            return;

           if (collision.transform.tag == "Player")
        {
            Player player = collision.transform.GetComponentInChildren<Player>();
            if (player == null)
                player = collision.transform.GetComponentInParent<Player>();

            if (player != null)
            {
                AdjustPlayerVelocity(0, 0, player);
            }
        }
    }
}
