using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Simple swap to a hurt animation when being attacked
/// </summary>
public class HurtAnimator : MonoBehaviour
{
    [SerializeField] bool playerAttacksHurt = true;
    [SerializeField] bool trackDeathHurts = false; 

    private AdvancedFrameAnimation animator;

    private void Start()
    {
        animator = GetComponent<AdvancedFrameAnimation>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (playerAttacksHurt)
        {
            if (collision.tag == "PlayerAttack")
            {
                animator.SwitchToEmotion(isHurt: true); 
            }
        }
        
        if (trackDeathHurts)
        {
            if (collision.GetComponent<TrackPlayerDeath>())
            {
                animator.SwitchToEmotion(isHurt: true);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (playerAttacksHurt)
        {
            if (collision.gameObject.tag == "PlayerAttack")
            {
                animator.SwitchToEmotion(isHurt: true);
            }
        }

        if (trackDeathHurts)
        {
            if (collision.gameObject.GetComponent<TrackPlayerDeath>())
            {
                animator.SwitchToEmotion(isHurt: true);
            }
        }
    }
}
