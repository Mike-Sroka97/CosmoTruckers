using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotentPattyPatty : MonoBehaviour
{
    PotentPatty minigame;
    bool trackScore = true;

    private void Start()
    {
        minigame = FindObjectOfType<PotentPatty>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.name == "Oil" && trackScore)
        {
            minigame.Score--;
            Debug.Log(minigame.Score);
            trackScore = false;
        }
    }
}
