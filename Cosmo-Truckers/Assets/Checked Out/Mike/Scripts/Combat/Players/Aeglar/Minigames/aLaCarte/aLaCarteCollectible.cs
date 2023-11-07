using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class aLaCarteCollectible : MonoBehaviour
{
    [SerializeField] bool damaging = true;

    aLaCarte minigame;

    private void Start()
    {
        minigame = FindObjectOfType<aLaCarte>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if(damaging)
            {
                minigame.PlayerDead = true;
                Debug.Log(minigame.PlayerDead);
            }
            else
            {
                minigame.Score++;
                minigame.GenerateCurrentLayout();
                Debug.Log(minigame.Score);
            }
        }
    }
}
