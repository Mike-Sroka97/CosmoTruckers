using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CosmicCasterCollectable : MonoBehaviour
{
    [HideInInspector] public bool Activated;

    CosmicCaster minigame;
    Collider2D myCollider;
    SpriteRenderer myRenderer;

    private void Start()
    {
        minigame = FindObjectOfType<CosmicCaster>();
        myCollider = GetComponent<Collider2D>();
        myRenderer = GetComponent<SpriteRenderer>();
    }

    public void ActivateMe()
    {
        Activated = true;
        myCollider.enabled = true;
        myRenderer.enabled = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            myCollider.enabled = false;
            myRenderer.enabled = false;
            minigame.Score++;
            Debug.Log(minigame.Score);
            minigame.NextCollectable();
        }
    }
}
