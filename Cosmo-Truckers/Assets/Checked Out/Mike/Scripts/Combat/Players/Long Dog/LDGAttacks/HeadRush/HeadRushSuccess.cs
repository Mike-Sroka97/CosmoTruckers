using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadRushSuccess : MonoBehaviour
{
    [SerializeField] GameObject gateToDisable;
    [SerializeField] GameObject gateToEnable;
    [SerializeField] int successToGive = 3;
    [SerializeField] bool lowerSuccessRate = false;

    bool hasInteracted = false; 

    HeadRush myMinigame;

    private void Start()
    {
        myMinigame = GetComponentInParent<HeadRush>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!hasInteracted)
        {
            hasInteracted = true; 
            myMinigame.Score += successToGive;
            Destroy(gameObject);
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" && !hasInteracted)
        {
            hasInteracted = true;
            myMinigame.Score += successToGive;
            Destroy(gameObject);
        }
    }
}
