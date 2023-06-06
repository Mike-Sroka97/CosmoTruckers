using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class COKOweakSpot : MonoBehaviour
{
    ClockOutKnockOut minigame;

    private void Start()
    {
        minigame = FindObjectOfType<ClockOutKnockOut>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "PlayerAttack")
        {
            minigame.Score++;
            Debug.Log(minigame.Score);
            Destroy(gameObject);
        }
    }
}
