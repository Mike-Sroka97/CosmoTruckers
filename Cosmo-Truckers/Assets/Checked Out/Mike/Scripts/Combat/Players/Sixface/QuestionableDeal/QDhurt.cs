using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QDhurt : MonoBehaviour
{
    QuestionableDeal minigame;

    private void Start()
    {
        minigame = FindObjectOfType<QuestionableDeal>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            minigame.PlayerDead = true;
            minigame.CalculateSuccess();
        }
    }
}
