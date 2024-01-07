using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LPSuccess : MonoBehaviour
{
    LPPlatformMovement[] damagingPlatforms;
    LengthyProcedure minigame;
    bool hasTriggered; 

    private void Start()
    {
        damagingPlatforms = FindObjectsOfType<LPPlatformMovement>();
        minigame = FindObjectOfType<LengthyProcedure>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.transform.tag == "PlayerAttack" || collision.transform.tag == "Player")
        {
            if (!hasTriggered)
            {
                hasTriggered = true;
                GetComponent<Collider2D>().enabled = false;

                foreach (LPPlatformMovement platform in damagingPlatforms)
                {
                    platform.Move();
                }
                minigame.NextNode();
                minigame.Score++;
                //Debug.Log(gameObject.name + ": " + minigame.Score);
                gameObject.SetActive(false);
            }
        }
    }
}
