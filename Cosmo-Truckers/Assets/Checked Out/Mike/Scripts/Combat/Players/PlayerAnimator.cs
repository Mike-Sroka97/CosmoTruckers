using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    public void ChangeAnimation(Animator animator, AnimationClip animation, float speed = 1)
    {
        animator.speed = speed;
        animator.Play(animation.name);
    }

    public void SetSprite(SpriteRenderer spriteRenderer, Animator animator, Sprite sprite)
    {
        animator.speed = 0;
        spriteRenderer.sprite = sprite;
    }
}
