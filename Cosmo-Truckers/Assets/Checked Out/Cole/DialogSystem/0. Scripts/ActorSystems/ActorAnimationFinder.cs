using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ActorAnimationFinder
{
   public static AnimationClip ReturnAnimationName(string input, string actorName, Animator anim, bool isPlayer = true)
    {
        string animationName = null;
        input = input.ToUpper(); 

        // Get general animation for players
        if (isPlayer)
        {
            foreach (PlayerAnimations enumValue in Enum.GetValues(typeof(PlayerAnimations)))
            {
                if (string.Equals(enumValue.ToString(), input, StringComparison.OrdinalIgnoreCase))
                {
                    animationName = actorName + "_" + enumValue.ToString().ToUpper();
                    break;
                }
            }
        }

        // Get general animation for NPCs 
        foreach (GeneralAnimations enumValue in Enum.GetValues(typeof(GeneralAnimations)))
        {
            if (string.Equals(enumValue.ToString(), input, StringComparison.OrdinalIgnoreCase))
            {
                animationName = "ACT_" + enumValue.ToString().ToUpper();
                break; 
            }
        }

        // Get all the clips on this animator
        AnimationClip[] clips = anim.runtimeAnimatorController.animationClips;

        if (animationName == null)
            animationName = input.ToUpper(); 

        foreach (AnimationClip clip in clips)
        {
            if (animationName == clip.name.ToUpper())
            {
                return clip;
            }
        }

        // Handle the case where no match is found
        throw new ArgumentException($"No matching enum value found for {input} with the given string {animationName}");
    }
}

public enum GeneralAnimations
{
    CONFUSED,
    SHOCKED,
}

public enum PlayerAnimations
{
    IDLE, 
}
