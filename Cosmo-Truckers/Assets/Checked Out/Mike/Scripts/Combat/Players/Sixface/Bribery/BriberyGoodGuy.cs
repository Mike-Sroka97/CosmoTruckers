using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BriberyGoodGuy : MonoBehaviour
{
    Bribery minigame;
    Collider2D myCollider;

    private void Start()
    {
        minigame = FindObjectOfType<Bribery>();
        myCollider = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        BriberyEnemy briberyEnemy = collision.GetComponent<BriberyEnemy>(); 

        if(briberyEnemy != null)
        {
            briberyEnemy.DoneMoving();

            if (myCollider.enabled)
            {
                myCollider.enabled = false;
                minigame.AugmentScore--;
                Destroy(gameObject);
            }
        }
    }
}
