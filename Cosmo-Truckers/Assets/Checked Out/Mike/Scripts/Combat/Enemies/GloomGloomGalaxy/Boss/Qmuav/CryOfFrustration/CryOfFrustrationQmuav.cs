using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CryOfFrustrationQmuav : MonoBehaviour
{
    [SerializeField] int hitPoints = 5;
    [SerializeField] float shakeDuration = 1f;
    
    ObjectShaker shaker;
    AdvancedFrameAnimation animator; 

    private void Start()
    {
        shaker = GetComponent<ObjectShaker>();
        animator = GetComponent<AdvancedFrameAnimation>();
    }

    public void SetHealth(int numberOfPlayers)
    {
        hitPoints *= numberOfPlayers;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "PlayerAttack")
        {
            hitPoints--;

            if (hitPoints <= 0)
            {
                GetComponentInParent<CombatMove>().FightWon = true;
                GetComponentInParent<CombatMove>().EndMove();
                enabled = false;
            }
            else
            {
                StartCoroutine(shaker.ShakeForDuration(shakeDuration));
                animator.StartAnimationWithUniqueTime(shakeDuration, isHurt: true); 
            }
        }
    }
}
