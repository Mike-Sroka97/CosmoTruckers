using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class COKOweakSpot : MonoBehaviour
{
    ClockOutKnockOut minigame;
    GonzorHurt gonzor; 
    
    private void Start()
    {
        minigame = FindObjectOfType<ClockOutKnockOut>();
        gonzor = FindObjectOfType<GonzorHurt>(); 
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "PlayerAttack")
        {
            minigame.Score++;
            minigame.CheckSuccess(); 
            gonzor.Hurt(); 
            Debug.Log(minigame.Score);
            Destroy(gameObject);
        }
    }
}
