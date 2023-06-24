using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PapConveyorPartNode : MonoBehaviour
{
    bool badZone;
    PunchAPretender minigame;

    private void Start()
    {
        minigame = FindObjectOfType<PunchAPretender>();

        if(transform.name == "BadZone")
        {
            badZone = true;
        }
        else
        {
            badZone = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(badZone && collision.tag == "Player")
        {
            minigame.PlayerDead = true;
        }
        else if(!badZone && collision.tag == "PlayerAttack")
        {
            minigame.Score++;
            if(minigame.Score >= minigame.MaxScore)
            {
                minigame.EndMove();
            }
            Debug.Log(minigame.Score);
            gameObject.SetActive(false);
        }
    }
}
