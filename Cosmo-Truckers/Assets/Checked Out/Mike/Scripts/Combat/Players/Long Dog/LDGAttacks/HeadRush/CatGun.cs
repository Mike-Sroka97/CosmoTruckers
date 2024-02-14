using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatGun : MonoBehaviour
{
    [SerializeField] float fireOffset;
    [SerializeField] float fireCD;
    [SerializeField] GameObject projectile;
    [SerializeField] GameObject projectileSpawn;
    [SerializeField] Sprite fireMouth;
    [SerializeField] float spriteWaitTime = 1f;   

    SpriteRenderer myRenderer;
    Sprite startingSprite; 

    float currentTime = 0;

    private void Start()
    {
        myRenderer = GetComponent<SpriteRenderer>();
        startingSprite = myRenderer.sprite; 
        float randomOffset = Random.Range(0.0f, fireOffset);
        currentTime += randomOffset;
    }
    private void Update()
    {
        if(currentTime >= fireCD)
        {
            StartCoroutine(Fire()); 
            currentTime = 0;

        }
        currentTime += Time.deltaTime;
    }

    IEnumerator Fire()
    {
        myRenderer.sprite = fireMouth;
        Instantiate(projectile, projectileSpawn.transform, transform.parent);
        yield return new WaitForSeconds(spriteWaitTime);
        myRenderer.sprite = startingSprite; 
    }  
}
