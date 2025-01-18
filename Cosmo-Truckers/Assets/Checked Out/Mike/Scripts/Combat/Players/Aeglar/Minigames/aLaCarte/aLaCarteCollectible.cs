using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class aLaCarteCollectible : MonoBehaviour
{
    [SerializeField] float layoutDelay = .15f;

    aLaCarte minigame;
    ParticleSpawner myParticleSpawner; 

    private void Start()
    {
        minigame = FindObjectOfType<aLaCarte>();
        myParticleSpawner = GetComponent<ParticleSpawner>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            myParticleSpawner.SpawnParticle(transform); 
            minigame.Score++;
            minigame.CheckSuccess();
            StartCoroutine(LayoutDelay());
        }
    }

    IEnumerator LayoutDelay()
    {
        yield return new WaitForSeconds(layoutDelay);
        minigame.GenerateCurrentLayout();
    }
}
