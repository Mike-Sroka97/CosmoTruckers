using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleBabiesBubble : MonoBehaviour
{
    [SerializeField] GameObject bubblePopParticle;

    BubbleBabies minigame;

    private void Start()
    {
        minigame = FindObjectOfType<BubbleBabies>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        minigame.Score++;

        GameObject particle = Instantiate(bubblePopParticle, transform.position, Quaternion.identity, null); 

        Destroy(gameObject);
    }
}
