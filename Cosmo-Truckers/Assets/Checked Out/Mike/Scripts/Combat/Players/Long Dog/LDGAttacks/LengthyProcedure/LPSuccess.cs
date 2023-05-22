using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LPSuccess : MonoBehaviour
{
    LPPlatformMovement[] damagingPlatforms;
    LengthyProcedure minigame;

    private void Start()
    {
        damagingPlatforms = FindObjectsOfType<LPPlatformMovement>();
        minigame = FindObjectOfType<LengthyProcedure>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<LongDogHead>())
        {
            foreach (LPPlatformMovement platform in damagingPlatforms)
            {
                platform.Move();
            }
            minigame.NextNode();
            minigame.Score++;
            gameObject.SetActive(false);
        }
    }
}
