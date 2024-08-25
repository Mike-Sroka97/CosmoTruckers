using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ActorAnimationFinder
{
    const string PLAYER_ENUM_APPEND = "PLR_";
    const string CLONE_TEXT = "(Clone)"; 

    public static AnimationClip ReturnAnimationName(string input, string actorName, Animator anim, bool isPlayer = true)
    {
        string animationName = null;
        input = input.ToUpper();

        // Get specific animations that are general for players
        if (isPlayer)
        {
            foreach (PlayerAnimations enumValue in Enum.GetValues(typeof(PlayerAnimations)))
            {
                if (string.Equals(enumValue.ToString(), input, StringComparison.OrdinalIgnoreCase))
                {
                    // This will find the animation by using the actor's game object name and the enum value
                    // Ex: ACT_PLR_AEG_CONFUSED would be different than a general version of CONFUSED. It would be specific to that player
                    string enumName = enumValue.ToString().ToUpper();
                    enumName = enumName.Replace($"{PLAYER_ENUM_APPEND}", ""); 
                    animationName = actorName + "_" + enumValue.ToString().ToUpper();
                    break;
                }
            }
        }

        // Get general animations
        foreach (GeneralAnimations enumValue in Enum.GetValues(typeof(GeneralAnimations)))
        {
            if (string.Equals(enumValue.ToString(), input, StringComparison.OrdinalIgnoreCase))
            {
                // Ex: ACT_CONFUSED
                animationName = "ACT_" + enumValue.ToString().ToUpper();
                break; 
            }
        }

        // Get all the clips on this animator
        AnimationClip[] clips = anim.runtimeAnimatorController.animationClips;
        string actorPlusInput = actorName + "_" + input.ToUpper();
        // If the actor's gameObject name had (Clone) in it, replace that text in final
        actorPlusInput = actorPlusInput.Replace($"{CLONE_TEXT}", "");

        if (animationName == null)
            animationName = input.ToUpper();

        foreach (AnimationClip clip in clips)
        {
            if (animationName == clip.name.ToUpper() || actorPlusInput == clip.name.ToUpper())
                return clip;
        }

        // Handle the case where no match is found
        throw new ArgumentException($"No matching enum value found for {input} with the given string {animationName} on the actor {actorPlusInput}");
    }
}

public enum GeneralAnimations
{
    IDLE,
    CONFUSED,
    SHOCKED,
}

// This is how we differentiate general from players, by adding PLR_ to the front in the dialog text
public enum PlayerAnimations
{
    PLR_IDLE, 
    PLR_CONFUSED, 
    PLR_SHOCKED
}
