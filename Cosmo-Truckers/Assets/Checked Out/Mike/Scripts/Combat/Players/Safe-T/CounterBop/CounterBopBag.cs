using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CounterBopBag : MonoBehaviour
{
    CounterBop minigame;
    bool canBeHit = true;
    [SerializeField] Animator bagAnimator;
    [SerializeField] AnimationClip[] bagHitAnimations;
    [SerializeField] Transform burstParticleSpawn; 
    [SerializeField] GameObject burstParticle; 

    private void Start()
    {
        minigame = transform.parent.parent.parent.GetComponent<CounterBop>();
    }

    private void HandleHit()
    {
        minigame.HitBall();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "PlayerAttack" && canBeHit)
        {
            canBeHit = false;
            HandleHit();
            bagAnimator.Play(bagHitAnimations[Mathf.Abs(minigame.Score)].name, -1, 0f); 
            if (minigame.CheckSuccess())
                Instantiate(burstParticle, burstParticleSpawn.position, Quaternion.identity, minigame.transform);

            StartCoroutine(CanHitBag()); 
        }
    }

    private IEnumerator CanHitBag()
    {
        yield return new WaitForSeconds(0.25f);
        canBeHit = true; 
    }
}
