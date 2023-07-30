using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleBabiesBubble : MonoBehaviour
{
    BubbleBabies minigame;

    private void Start()
    {
        minigame = FindObjectOfType<BubbleBabies>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        minigame.Score++;
        Destroy(gameObject);
    }
}
