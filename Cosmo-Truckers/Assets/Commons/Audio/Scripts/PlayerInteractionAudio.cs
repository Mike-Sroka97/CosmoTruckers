using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A script that plays the player hit sound when the player hits this
/// </summary>
public class PlayerInteractionAudio : MonoBehaviour
{
    private const string AttackSound = "Attack";
    private const string HitSound = "Hit";
    private const string SuccessSound = "Success";

    [SerializeField] private bool playerHittable = true; 
    [SerializeField] private bool playerSuccess; 

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerAttack"))
        {
            // Default Hittable
            if (playerHittable && !playerSuccess)
            {
                PlayHitSound(collision.transform);
            }
            // Success Hittable
            if (playerHittable && playerSuccess)
            {
                PlayHitSound(collision.transform, SuccessSound);
            }
        }
        if (collision.CompareTag("Player"))
        {
            // Success interactable
            if (!playerHittable && playerSuccess)
            {
                PlayHitSound(collision.transform, SuccessSound);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Success interactable
            if (!playerHittable && playerSuccess)
            {
                PlayHitSound(collision.transform, SuccessSound);
            }
        }
    }

    /// <summary>
    /// Play the hit sound
    /// </summary>
    /// <param name="playerTransform"></param>
    private void PlayHitSound(Transform playerTransform, string sound = HitSound)
    {
        Player player = HelperFunctions.FindNearestParentOfType<Player>(playerTransform);

        if (player.MyAudioDevice.GetSound(sound).isPlaying)
            return;

        player.MyAudioDevice.StopSound(AttackSound);
        player.MyAudioDevice.PlaySound(sound); 
    }
}
