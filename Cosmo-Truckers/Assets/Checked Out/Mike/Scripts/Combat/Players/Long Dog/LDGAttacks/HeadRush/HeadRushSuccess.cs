using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadRushSuccess : MonoBehaviour
{
    [SerializeField] int successToGive = 3;

    bool hasInteracted = false; 

    HeadRush myMinigame;

    private void Start()
    {
        myMinigame = GetComponentInParent<HeadRush>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((collision.gameObject.CompareTag("Player") || collision.CompareTag("Attack Zone")) && !hasInteracted)
        {
            hasInteracted = true;
            myMinigame.Score += successToGive;
            myMinigame.CheckSuccess();
            Destroy(gameObject);
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !hasInteracted)
        {
            hasInteracted = true;
            myMinigame.Score += successToGive;
            myMinigame.CheckSuccess(); 
            Destroy(gameObject);
        }
    }
}
