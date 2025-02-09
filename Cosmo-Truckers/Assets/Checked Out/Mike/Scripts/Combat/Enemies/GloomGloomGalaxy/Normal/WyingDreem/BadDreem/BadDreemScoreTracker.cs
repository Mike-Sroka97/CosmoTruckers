using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BadDreemScoreTracker : MonoBehaviour
{
    bool trackScoreTime = false;
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
        if(trackScoreTime)
            minigame.CurrentScore += Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
            trackScoreTime = false;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
            trackScoreTime = true;
    }
}
