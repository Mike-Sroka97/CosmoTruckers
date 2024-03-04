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

        if (isPlayer)
        {
            foreach (PlayerAnimations enumValue in Enum.GetValues(typeof(PlayerAnimations)))
            {
                if (string.Equals(enumValue.ToString(), input, StringComparison.OrdinalIgnoreCase))
                {
                    animationName = actorName + "_" + enumValue.ToString().ToUpper(); 
                }
            }
        }

        foreach (GeneralAnimations enumValue in Enum.GetValues(typeof(GeneralAnimations)))
        {
            if (string.Equals(enumValue.ToString(), input, StringComparison.OrdinalIgnoreCase))
            {
                animationName = "ACT_" + enumValue.ToString().ToUpper();
            }
        }

        AnimationClip[] clips = anim.runtimeAnimatorController.animationClips; 

        foreach (AnimationClip clip in clips)
        {
            if (animationName == clip.name)
            {
                return clip;
            }
        }

        // Handle the case where no match is found
        throw new ArgumentException("No matching enum value found for the given string", nameof(input));
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
