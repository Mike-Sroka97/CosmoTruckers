using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BadDreemScoreTracker : MonoBehaviour
{
    bool trackTime = false;
    BadDreem minigame;

    private void Start()
    {
        minigame = FindObjectOfType<BadDreem>();
    }

    private void Update()
    {
        TrackTime();
    }

    private void TrackTime()
    {
        if(trackTime)
        {
            minigame.CurrentScore -= Time.deltaTime;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            trackTime = false;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            trackTime = true;
        }
    }
}
