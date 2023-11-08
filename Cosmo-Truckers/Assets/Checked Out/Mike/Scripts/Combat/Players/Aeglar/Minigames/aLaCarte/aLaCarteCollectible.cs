using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class aLaCarteCollectible : MonoBehaviour
{
    aLaCarte minigame;

    private void Start()
    {
        minigame = FindObjectOfType<aLaCarte>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            minigame.Score++;
            minigame.GenerateCurrentLayout();
            Debug.Log(minigame.Score);
        }
    }
}
