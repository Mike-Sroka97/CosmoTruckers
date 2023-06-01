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
        if(collision.GetComponent<BriberyEnemy>())
        {
            if(myCollider.enabled)
            {
                myCollider.enabled = false;
                collision.GetComponent<BriberyEnemy>().DoneMoving();
                minigame.Score--;
                Debug.Log(minigame.Score);
                Destroy(gameObject);
            }
        }
    }
}
