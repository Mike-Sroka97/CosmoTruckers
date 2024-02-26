using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ActorAnimationFinder
{
   public static string ReturnAnimationName(string input, bool isPlayer)
    {
        foreach (GeneralAnimations enumValue in Enum.GetValues(typeof(GeneralAnimations)))
        {
            if (string.Equals(enumValue.ToString(), input, StringComparison.OrdinalIgnoreCase))
            {
                return enumValue.ToString().ToUpper();
            }
        }

        foreach (PlayerAnimations enumValue in Enum.GetValues(typeof(PlayerAnimations)))
        {
            if (string.Equals(enumValue.ToString(), input, StringComparison.OrdinalIgnoreCase))
            {
                return enumValue.ToString().ToUpper();
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
