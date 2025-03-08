using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class aLaCarteCollectible : MonoBehaviour
{
    [SerializeField] float layoutDelay = .15f;

    aLaCarte minigame;
    ParticleSpawner myParticleSpawner;
    SpriteRenderer myRenderer; 

    private void Start()
    {
        minigame = FindObjectOfType<aLaCarte>();
        myParticleSpawner = GetComponent<ParticleSpawner>();
        myRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            myRenderer.enabled = false;
            myParticleSpawner.SpawnParticle(transform); 
            minigame.Score++;
            
            if (!minigame.CheckSuccess())
                StartCoroutine(LayoutDelay());
        }
    }

    IEnumerator LayoutDelay()
    {
        yield return new WaitForSeconds(layoutDelay);
        minigame.GenerateCurrentLayout();
        myRenderer.enabled = true;
    }
}
