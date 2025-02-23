using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FGGoal : MonoBehaviour
{
    [SerializeField] Transform particleSpawn; 
    [SerializeField] GameObject successParticle;
    [SerializeField] Animator gunAnimator;
    [SerializeField] AnimationClip fireAnimation;
    
    [SerializeField] Transform bulletSpawn;
    [SerializeField] GameObject bullet;
    [SerializeField] ParticleSystem smoke;

    private FunGun minigame;
    private bool isFiring = false;  

    private void Start()
    {
        minigame = FindObjectOfType<FunGun>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<FGBullet>() != null)
        {
            Instantiate(successParticle, particleSpawn.position, Quaternion.identity, minigame.transform);
            Destroy(collision.gameObject);
            StartCoroutine(FireGun()); 
        }
    }

    IEnumerator FireGun()
    {
        while (isFiring)
            yield return null;

        isFiring = true; 

        // Play sound: loading magazine 
        yield return new WaitForSeconds(0.5f);

        Instantiate(bullet, bulletSpawn.position, Quaternion.identity, minigame.transform);
        smoke.Play();
        gunAnimator.Play(fireAnimation.name);
        minigame.Score++;
        minigame.CheckSuccess(); 

        isFiring = false;
    }
}
