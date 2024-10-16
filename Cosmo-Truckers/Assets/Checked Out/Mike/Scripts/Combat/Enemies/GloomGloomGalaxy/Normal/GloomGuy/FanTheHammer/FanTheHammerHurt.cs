using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FanTheHammerHurt : MonoBehaviour
{
    FanTheHammer minigame;
    Player player;

    private void Start()
    {
        minigame = FindObjectOfType<FanTheHammer>();
        player = FindObjectOfType<Player>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            if (player.iFrames)
                return;

            if(minigame.Score > 0)
            {
                minigame.Score--;
            }

            minigame.CheckSuccess();
            Debug.Log(minigame.Score);
        }
    }
}
