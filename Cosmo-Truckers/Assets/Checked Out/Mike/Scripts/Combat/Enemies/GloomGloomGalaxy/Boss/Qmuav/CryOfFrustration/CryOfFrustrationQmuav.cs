using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CryOfFrustrationQmuav : MonoBehaviour
{
    [SerializeField] int hitPoints = 5;
    [SerializeField] float shakeDuration = 1f;
    [SerializeField] AnimationClip deathAnimation;
    [SerializeField] float deathTime = 5f; 

    ObjectShaker shaker;
    AdvancedFrameAnimation frameAnimator;
    Animator myAnimator;
    CombatMove minigame;
    bool startedDeath = false; 

    private void Start()
    {
        shaker = GetComponent<ObjectShaker>();
        frameAnimator = GetComponent<AdvancedFrameAnimation>();
        myAnimator = GetComponent<Animator>();
        minigame = GetComponentInParent<CombatMove>(); 
    }

    public void SetHealth(int numberOfPlayers)
    {
        hitPoints *= numberOfPlayers;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerAttack"))
        {
            hitPoints--;

            if (hitPoints <= 0 && !startedDeath)
            {
                startedDeath = true;
                StartCoroutine(QmuavDead()); 
            }
            else
            {
                StartCoroutine(shaker.ShakeForDuration(shakeDuration));
                frameAnimator.SwitchToEmotion(isHurt: true, timeBeforeSwapping: shakeDuration); 
            }
        }
    }

    /// This is for when Qmuav dies and we do a little animation
    private IEnumerator QmuavDead()
    {
        frameAnimator.StopAnimation();
        frameAnimator.enabled = false;
        myAnimator.enabled = true;
        myAnimator.Play(deathAnimation.name); 

        yield return new WaitForSeconds(deathTime);

        minigame.EndMove();
        minigame.FightWon = true;
        shaker.enabled = false;
        enabled = false;
    }
}
