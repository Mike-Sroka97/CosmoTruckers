using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System;
using UnityEngine;
using System.Runtime.CompilerServices;

//This script handles the utility of the dialogue, like speed, size, effects, etc.
public class DialogUtility : MonoBehaviour
{
    #region REGEX VARIABLES
    // -- Regular Expressions are for matching strings within certain boundaries --
    // RemainderRegex finds any character and will look ahead for the position in the string where > is. It will also look for / or $ (end of string)
    private const string REMAINDER_REGEX = "(.*?((?=>)|(/|$)))";

    // Each of the Regexes finds the defined type (ex: <p: _text here_>) and gets the text inside of the brackets for said type
    private const string PAUSE_REGEX_STRING = "<p:(?<pause>" + REMAINDER_REGEX + ")>";
    private static readonly Regex pauseRegex = new Regex(PAUSE_REGEX_STRING);
    private const string SPEED_REGEX_STRING = "<sp:(?<speed>" + REMAINDER_REGEX + ")>";
    private static readonly Regex speedRegex = new Regex(SPEED_REGEX_STRING);
    private const string VOICE_REGEX_STRING = "<vc:(?<voice>" + REMAINDER_REGEX + ")>";
    private static readonly Regex voiceRegex = new Regex(VOICE_REGEX_STRING);
    private const string ANIM_START_REGEX_STRING = "<anim:(?<anim>" + REMAINDER_REGEX + ")>";
    private static readonly Regex animStartRegex = new Regex(ANIM_START_REGEX_STRING);
    private const string ANIM_END_REGEX_STRING = "</anim>";
    private static readonly Regex animEndRegex = new Regex(ANIM_END_REGEX_STRING);
    private const string COLOR_START_REGEX_STRING = "<color:(?<color>" + REMAINDER_REGEX + ")>";
    private static readonly Regex colorStartRegex = new Regex(COLOR_START_REGEX_STRING);
    private const string COLOR_END_REGEX_STRING = "</color>";
    private static readonly Regex colorEndRegex = new Regex(COLOR_END_REGEX_STRING);
    #endregion

    private static readonly Dictionary<string, float> pauseDictionary = new Dictionary<string, float>
    {
        {"tiny", 0.1f},
        {"short", 0.25f},
        {"normal", 0.5f},
        {"long", 1f},
        {"longest", 2f},
    };

    /// <summary>
    /// Process the incoming message for all of its tags.
    /// Remove all tag text from the message that is going to be displayed to the user
    /// </summary>
    /// <param name="message"></param>
    /// <param name="processedMessage"></param>
    /// <returns></returns>
    public static List<DialogCommand> ProcessMessage(string message, out string processedMessage)
    {
        List<DialogCommand> result = new List<DialogCommand>();
        processedMessage = message;

        processedMessage = HandlePauseTags(processedMessage, result);
        processedMessage = HandleSpeedTags(processedMessage, result);
        processedMessage = HandleVoiceTags(processedMessage, result);
        processedMessage = HandleAnimStartTags(processedMessage, result);
        processedMessage = HandleAnimEndTags(processedMessage, result); 
        processedMessage = HandleColorStartTags(processedMessage, result);
        processedMessage = HandleColorEndTags(processedMessage, result);

        return result; 
    }

    /// <summary>
    /// Go through all of the Pause Tags and add them to the list of Dialog Commands
    /// After doing so, remove the Pause Tags text from the message
    /// </summary>
    /// <param name="proccessedMessage"></param>
    /// <param name="result"></param>
    /// <returns></returns>
    private static string HandlePauseTags(string proccessedMessage, List<DialogCommand> result)
    {
        //Get a collection of matches for the pauseRegex
        MatchCollection pauseMatches = pauseRegex.Matches(proccessedMessage);

        // For each pause, define where it is in the message and how long it is
        foreach (Match match in pauseMatches)
        {
            string value = match.Groups["pause"].Value;

            result.Add(new DialogCommand
            {
                position = VisibleCharactersUpToIndex(proccessedMessage, match.Index), 
                type = TextCommandType.Pause, 
                floatValue = pauseDictionary[value]
            }); 
        }

        //Remove the Regex string from the original message
        proccessedMessage = Regex.Replace(proccessedMessage, PAUSE_REGEX_STRING, "");
        return proccessedMessage; 
    }

    /// <summary>
    /// Go through all of the Speed Tags and add them to the list of Dialog Commands
    /// After doing so, remove the Speed Tags text from the message
    /// </summary>
    /// <param name="processedMessage"></param>
    /// <param name="result"></param>
    /// <returns></returns>
    private static string HandleSpeedTags(string processedMessage, List<DialogCommand> result)
    {
         MatchCollection speedMatches = speedRegex.Matches(processedMessage);

        foreach (Match match in speedMatches)
        {
            string stringValue = match.Groups["speed"].Value; 

            //If you can't get a float from the speed value, just default to 25f
            if (!float.TryParse(stringValue, out float val))
                val = 25f;

            result.Add(new DialogCommand
            {
                position = VisibleCharactersUpToIndex(processedMessage, match.Index), 
                type = TextCommandType.TextSpeedChange, 
                floatValue = val
            }); 
        }

        processedMessage = Regex.Replace(processedMessage, SPEED_REGEX_STRING, ""); 
        return processedMessage;
    }

    /// <summary>
    /// Go through all of the Voice Bark Tags and add them to the list of Dialog Commands
    /// After doing so, remove the Voice Bark Tags text from the message
    /// </summary>
    /// <param name="processedMessage"></param>
    /// <param name="result"></param>
    /// <returns></returns>
    private static string HandleVoiceTags(string processedMessage, List<DialogCommand> result)
    {
        MatchCollection voiceMatches = voiceRegex.Matches(processedMessage);

        foreach (Match match in voiceMatches)
        {
            string stringValue = match.Groups["voice"].Value;

            result.Add(new DialogCommand
            {
                position = VisibleCharactersUpToIndex(processedMessage, match.Index),
                type = TextCommandType.VoiceBark,
                stringValue = GetVoiceType(stringValue), 
                floatValue = GetVoiceRate(stringValue),
                boolValue = GetVoiceFluctuate(stringValue)
            });
        }

        processedMessage = Regex.Replace(processedMessage, VOICE_REGEX_STRING, "");
        return processedMessage;
    }

    /// <summary>
    /// Go through all of the Anim Tags (the start of them) and add them to the list of Dialog Commands
    /// After doing so, remove the Anim Tags text from the message
    /// </summary>
    /// <param name="processedMessage"></param>
    /// <param name="result"></param>
    /// <returns></returns>
    private static string HandleAnimStartTags(string processedMessage, List<DialogCommand> result)
    {
        MatchCollection animStartMatches = animStartRegex.Matches(processedMessage);

        foreach (Match match in animStartMatches)
        {
            string stringValue = match.Groups["anim"].Value;
            result.Add(new DialogCommand
            {
                position = VisibleCharactersUpToIndex(processedMessage, match.Index), 
                type = TextCommandType.AnimStart, 
                textAnimValue = GetTextAnimationType(stringValue)
            }); 
        }

        processedMessage = Regex.Replace(processedMessage, ANIM_START_REGEX_STRING, ""); 
        return processedMessage;
    }

    /// <summary>
    /// Go through all of the Anim Tags (the end of them) and add them to the list of Dialog Commands
    /// After doing so, remove the Anim Tags text from the message
    /// </summary>
    /// <param name="processedMessage"></param>
    /// <param name="result"></param>
    /// <returns></returns>
    private static string HandleAnimEndTags(string processedMessage, List<DialogCommand> result)
    {
        MatchCollection animEndMatches = animEndRegex.Matches(processedMessage);

        foreach (Match match in animEndMatches)
        {
            result.Add(new DialogCommand
            {
                position = VisibleCharactersUpToIndex(processedMessage, match.Index), 
                type = TextCommandType.AnimEnd,
            });
        }

        processedMessage = Regex.Replace(processedMessage, ANIM_END_REGEX_STRING, ""); 
        return processedMessage;
    }
    
    /// <summary>
    /// Go through all of the Color Tags (the start of them) and add them to the list of Dialog Commands
    /// After doing so, remove the Color Tags text from the message
    /// </summary>
    /// <param name="processedMessage"></param>
    /// <param name="result"></param>
    /// <returns></returns>
    private static string HandleColorStartTags(string processedMessage, List<DialogCommand> result)
    {
        MatchCollection colorStartMatches = colorStartRegex.Matches(processedMessage);

        foreach (Match match in colorStartMatches)
        {
            string stringValue = match.Groups["color"].Value;
            result.Add(new DialogCommand
            {
                position = VisibleCharactersUpToIndex(processedMessage, match.Index),
                type = TextCommandType.ColorStart,
                color = GetColor(stringValue)
            });
        }

        processedMessage = Regex.Replace(processedMessage, COLOR_START_REGEX_STRING, "");
        return processedMessage;
    }

    /// <summary>
    /// Go through all of the Color Tags (the end of them) and add them to the list of Dialog Commands
    /// After doing so, remove the Color Tags text from the message
    /// </summary>
    /// <param name="processedMessage"></param>
    /// <param name="result"></param>
    /// <returns></returns>
    private static string HandleColorEndTags(string processedMessage, List<DialogCommand> result)
    {
        MatchCollection colorEndMatches = colorEndRegex.Matches(processedMessage);

        foreach (Match match in colorEndMatches)
        {
            result.Add(new DialogCommand
            {
                position = VisibleCharactersUpToIndex(processedMessage, match.Index),
                type = TextCommandType.ColorEnd,
            });
        }

        processedMessage = Regex.Replace(processedMessage, COLOR_END_REGEX_STRING, "");
        return processedMessage;
    }

    /// <summary>
    /// Goes through each character, and if it's not part of a <> tag, it will count that character
    /// Gives the position of where this tag starts in the actual shown message
    /// </summary>
    /// <param name="message"></param>
    /// <param name="index"></param>
    /// <returns></returns>
    private static int VisibleCharactersUpToIndex(string message, int index)
    {
        int result = 0;
        bool insideBrackets = false;

        for (int i = 0; i < index; i++)
        {
            if (message[i] == '<')
            {
                insideBrackets = true;
            }
            else if (message[i] == '>')
            {
                insideBrackets = false;
                result--;
            }
            if (!insideBrackets)
            {
                result++;
            }
            else if (i + 6 < index && message.Substring(i, 6) == "sprite")
            {
                result++;
            }
        }
        return result;
    }

    /// <summary>
    /// Try to get the TextAnimationType from the string
    /// Otherwise set it to none
    /// </summary>
    /// <param name="stringVal"></param>
    /// <returns></returns>
    private static TextAnimationType GetTextAnimationType(string stringVal)
    {
        TextAnimationType result; 

        try
        {
            result = (TextAnimationType)Enum.Parse(typeof(TextAnimationType), stringVal, true);
        }
        catch (ArgumentException)
        {
            Debug.LogError("Invalid Text Animation Type: " + stringVal);
            result = TextAnimationType.none; 
        }

        return result; 
    }
    private static Color32 GetColor(string stringVal)
    {
        // Set to black by default
        Color32 newColor = EnumManager.ColorsRGB.GetValueOrDefault(EnumManager.ColorPalette.Black); 
        EnumManager.ColorPalette currentColor = (EnumManager.ColorPalette)Enum.Parse(typeof(EnumManager.ColorPalette), stringVal, true);
        
        if ((currentColor == EnumManager.ColorPalette.White && stringVal == "white") || (currentColor != EnumManager.ColorPalette.White && stringVal != "white"))
            newColor = EnumManager.ColorsRGB.GetValueOrDefault(currentColor);

        return newColor;
    }
    private static float GetVoiceRate(string stringVal)
    {
        const int defaultVoiceRate = 3; 

        // split the string up based on commas
        string[] valueList = stringVal.Split(',');

        if (float.TryParse(valueList[1], out float currentValue))
            return currentValue; 

        else { return (float)defaultVoiceRate;}
    }
    private static string GetVoiceType(string stringVal)
    {
        // split the string up based on commas
        string[] valueList = stringVal.Split(',');

        return valueList[0]; 
    }
    private static bool GetVoiceFluctuate(string stringVal)
    {
        string[] valueList = stringVal.Split(',');

        // In case I add more to VoiceBarks, but first two are type and rate. Third would be fluctuate bool
        if (valueList.Length > 2)
        {
            if (bool.TryParse(valueList[2], out bool result))
            {
                return result;
            }
            else
            {
                return true;
            }
        }
        else
        {
            return true;
        }
    }
}

public struct DialogCommand
{
    public int position;
    public float floatValue;
    public string stringValue;
    public bool boolValue; 
    public Color32 color;  
    public TextCommandType type;
    public TextAnimationType textAnimValue;
    public TextEmotionType textEmotionType;
}
public enum TextCommandType
{
    Pause, 
    TextSpeedChange,
    VoiceBark, 
    AnimStart,
    AnimEnd,
    SizeStart,
    SizeEnd, 
    ColorStart, 
    ColorEnd
}
public enum TextAnimationType
{
    none,
    shake,
    wave,
    pulse
}
public enum TextEmotionType
{
    none, 
    happy, 
    sad, 
    questioning, 
    angry, 
    confused,
    dumbfounded
}
