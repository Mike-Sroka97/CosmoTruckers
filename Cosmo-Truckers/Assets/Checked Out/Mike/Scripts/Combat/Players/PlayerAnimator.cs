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

    /// <summary>
    /// Check to see if the animator passed in is playing the clip passed in
    /// </summary>
    /// <param name="animator"></param>
    /// <param name="clipToCheck"></param>
    /// <returns></returns>
    public bool IsCurrentAnimationPlaying(Animator animator, AnimationClip clipToCheck)
    {
        string currentAnimationName = animator.GetCurrentAnimatorClipInfo(0)[0].clip.name;

        if (clipToCheck.name == currentAnimationName)
            return true;

        return false; 
    }
}
