using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PorkanatorPig : MonoBehaviour
{
    [SerializeField] int scoreValue;
    [SerializeField] float fadeSpeed;

    AeglarINA player;
    Porkanator minigame;
    SpriteRenderer myRenderer;
    ParticleSpawner myParticleSpawner; 

    private void Start()
    {
        player = FindObjectOfType<AeglarINA>();
        minigame = FindObjectOfType<Porkanator>();
        myRenderer = GetComponent<SpriteRenderer>();
        myParticleSpawner = GetComponent<ParticleSpawner>(); 
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.name == "Saw Pit")
        {
            minigame.Score += scoreValue;
            Debug.Log(minigame.Score);
            myParticleSpawner.SpawnDeathParticle(transform);
            StartCoroutine(Die());
        }
    }

    IEnumerator Die()
    {
        while(myRenderer.color.a > 0)
        {
            myRenderer.color -= new Color(0, 0, 0, fadeSpeed * Time.deltaTime);
            yield return new WaitForSeconds(Time.deltaTime);
        }
        Destroy(gameObject);
    }
}
