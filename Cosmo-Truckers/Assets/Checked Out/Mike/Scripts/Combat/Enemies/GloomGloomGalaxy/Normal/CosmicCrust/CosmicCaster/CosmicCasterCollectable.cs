using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CosmicCasterCollectable : MonoBehaviour
{
    [HideInInspector] public bool Activated;

    CosmicCaster minigame;
    Collider2D myCollider;
    SpriteRenderer myRenderer;
    [SerializeField] GameObject particle; 

    public void ActivateMe()
    {
        minigame = FindObjectOfType<CosmicCaster>();
        myCollider = GetComponent<Collider2D>();
        myRenderer = GetComponent<SpriteRenderer>();

        Activated = true;
        myCollider.enabled = true;
        myRenderer.enabled = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            Instantiate(particle, transform.position, Quaternion.identity, minigame.transform);
            myCollider.enabled = false;
            myRenderer.enabled = false;
            minigame.Score++;
            minigame.NextCollectable();
        }
    }
}
