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

    #region DICTIONARIES
    private static readonly Dictionary<string, float> pauseDictionary = new Dictionary<string, float>
    {
        {"tiny", 0.1f},
        {"short", 0.25f},
        {"normal", 0.5f},
        {"long", 1f},
        {"longest", 2f},
    };

    private static readonly Dictionary<string, float> sizeDictionary = new Dictionary<string, float>
    {
        {"mini", 0.25f},
        {"small", 0.5f},
        {"normal", 1f},
        {"large", 2f},
        {"huge", 3f},
    };

    private static readonly Dictionary<string, Color32> colorDictionary = new Dictionary<string, Color32>
    {
        {"yellowlight", new Color32(237,232,197,255)},
        {"yellow", new Color32(240,211,117,255)},
        {"skin", new Color32(240,179,149,255)},
        {"orange", new Color32(235,148,101,255)},
        {"brown", new Color32(189,104,91,255)},
        {"brown-dark", new Color32(48,17,28,255)},
        {"gray", new Color32(189,157,157,255)},
        {"pink", new Color32(250,150,170,255)},
        {"magenta", new Color32(212,76,121,255)},
        {"magentadark", new Color32(138,59,102,255)},
        {"bluelight", new Color32(120,182,204,255)},
        {"blue", new Color32(114,127,181,255)},
        {"royal", new Color32(100,80,148,255)},
        {"purpledark", new Color32(87,51,89,255)},
        {"purplelight", new Color32(182,137,204,255)},
        {"purple", new Color32(145,83,163,255)},
        {"regal", new Color32(130,91,112,255)},
        {"greenlight", new Color32(157,189,92,255)},
        {"green", new Color32(93,153,92,255)},
        {"greendark", new Color32(77,105,99,255)},
        {"neonred", new Color32(191,0,0,255)},
        {"neongreen", new Color32(57,255,20,255)},
        {"neonorange", new Color32(255,95,31,255)},
        {"neonyellow", new Color32(255,234,0,255)},
        {"neonwhite", new Color32(191,191,191,255)},
    };
    #endregion

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

            //If you can't get a float from the speed value, just default to 150f
            if (!float.TryParse(stringValue, out float val))
            {
                val = 150f; 
            }

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
        Color32 newColor;

            if (!colorDictionary.TryGetValue(stringVal, out newColor))
        {
            // split the string up based on commas
            string[] valueList = stringVal.Split(',');
            float[] numbers = new float[4];

            for (int i = 0; i < 4; i++)
            {
                // RGB of color
                if (i < 3)
                {
                    if (float.TryParse(valueList[i], out float currentValue))
                        numbers[i] = (currentValue / 255);
                    else
                        numbers[i] = 1f;
                }
                // Alpha of color
                else
                {
                    if (valueList[i] != null)
                    {
                        if (float.TryParse(valueList[i], out float currentValue) && currentValue <= 1f)
                            numbers[i] = currentValue;
                        else
                            numbers[i] = 1f;
                    }
                }
            }

            Color tempColor = new Color(numbers[0], numbers[1], numbers[2], numbers[3]);
            newColor = (Color32)tempColor;
        }

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
