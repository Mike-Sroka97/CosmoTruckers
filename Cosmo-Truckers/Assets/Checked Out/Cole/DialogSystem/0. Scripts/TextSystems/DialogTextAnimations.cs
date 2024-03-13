using System; 
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogTextAnimations
{
    public bool isTextAnimating = false; 
    private bool stopAnimating = false;

    private readonly TMP_Text textBox;
    private readonly float textAnimationScale;
    private Image nextLineIndicator;
    private int previousCharacterCount;

    private int countSinceLastBark = 0;
    private int previousBarkPosition = -1;
    private AudioSource audioSource;
    private BaseActor speaker;
    int vcRate = 3;
    List<AudioClip> vcBarks = new List<AudioClip>(); 

    // Initializer
    public DialogTextAnimations(TMP_Text _textBox, Image _nextLineIndicator, AudioSource _audioSource)
    {
        textBox = _textBox;
        nextLineIndicator = _nextLineIndicator;
        textAnimationScale = textBox.fontSize; 
        audioSource = _audioSource;
    }

    #region DEFAULT ANIMATION VARIABLES
    private static readonly Color32 clear = new Color32(0, 0, 0, 0);
    private const float CHAR_ANIM_TIME = 0.07f;
    private float secondsPerCharStartValue = 150f;
    private float secondsPerCharacterValue_1 = 1f;
    private float secondsPerCharacterValue_2 = 150f;
    private float secondsPerCharacter;

    private static readonly Vector3 vectorZero = Vector3.zero;
    #endregion

    #region AUDIO VARIABLES
    private List<AudioClip> dialogAudioClips = new List<AudioClip>();
    private int currentRateCount = 0; 
    private int previousClipValue;
    #endregion

    // Begin to animate the text in using a coroutine
    public IEnumerator AnimateTextIn(List<DialogCommand> commands, string processedMessage, BaseActor _speaker)
    {
        speaker = _speaker; 

        secondsPerCharacterValue_1 = 1f;
        secondsPerCharacterValue_2 = secondsPerCharStartValue;

        // set the isTextAnimating to true and get Seconds Per Character
        isTextAnimating = true; 
        secondsPerCharacter = secondsPerCharacterValue_1 / secondsPerCharacterValue_2;
        float timeOfLastCharacter = 0;

        // Get the start and ending index for text anims, colors, and sizes
        TextAnimInfo[] textAnimInfo = SeparateOutTextAnimInfo(commands);
        TextColorInfo[] textColorInfo = SeparateOutTextColorInfo(commands);

        // Get the text info
        TMP_TextInfo textInfo = textBox.textInfo;

        // Clear the mesh
        for (int i = 0; i < textInfo.meshInfo.Length; i++) 
        {
            TMP_MeshInfo meshInform = textInfo.meshInfo[i];
            if (meshInform.vertices != null)
            {
                for (int j = 0; j < meshInform.vertices.Length; j++)
                {
                    meshInform.vertices[j] = vectorZero; 
                }
            }
        }

        // Set the text of the textBox to the processed message and update the meshes
        textBox.text = processedMessage; 
        textBox.ForceMeshUpdate();

        // Get the original colors of the meshes
        TMP_MeshInfo[] cachedMeshInfo = textInfo.CopyMeshInfoVertexData();

        // Get the number of characters and create an array for the start times of each
        int characterCount = textInfo.characterCount;
        previousCharacterCount = characterCount; 

        float[] characterAnimStartTimes = new float[characterCount];

        for (int i = 0; i < characterCount; i++)
        {
            // this indicates the character hasn't started animating yet
            characterAnimStartTimes[i] = -1; 
        }

        int visibleCharacterIndex = 0;

        // Get the colors for this dialog
        Color32[] destinationColors = RetrieveDestinationColors(characterCount, textInfo, textColorInfo, cachedMeshInfo);

        // Begin to actually animate
        while (true)
        {
            if (stopAnimating)
            {
                // If stopAnimating is true, set all character anim start times to right now and finish animating
                for (int i = visibleCharacterIndex; i < characterCount; i++)
                {
                    characterAnimStartTimes[i] = Time.unscaledTime; 
                }
                // set the visible character count to the text info character count
                visibleCharacterIndex = characterCount;
                FinishAnimating(); 
            }
            if (CanShowNextCharacter(secondsPerCharacter, timeOfLastCharacter))
            {
                // If we're not at the end of the characterCount 
                if (visibleCharacterIndex <= characterCount)
                {
                    // Make sure every character talks
                    if (visibleCharacterIndex == 0)
                        UpdateDialogSound("normal", -1); 

                    ExecuteRemainingCommandsAtIndex(commands, visibleCharacterIndex, ref secondsPerCharacter, ref timeOfLastCharacter);
                    
                    // Check again because we've updated the secondsPerCharacter and timeOfLastCharacter
                    if (visibleCharacterIndex < characterCount && CanShowNextCharacter(secondsPerCharacter, timeOfLastCharacter))
                    {
                        // set the animation start time to now
                        characterAnimStartTimes[visibleCharacterIndex] = Time.unscaledTime;

                        // progress visible character index
                        visibleCharacterIndex++; 
                        timeOfLastCharacter = Time.unscaledTime; // set time of last character to now

                        // progress last dialog bark count
                        countSinceLastBark++; 

                        // Play Audio
                        PlayDialogSound(); 

                        // If we're at the characterCount, finish animating
                        if (visibleCharacterIndex == characterCount)
                        {
                            FinishAnimating(); 
                        }
                    }
                }
            }

            // Go through all of the characters and set the colors and the animation adjustments
            for (int j = 0; j < characterCount; j++)
            {
                // Character Info has a lot of stuff, but we'll mostly be using vertex info from here
                TMP_CharacterInfo characterInfo = textInfo.characterInfo[j];
                
                //Invisible characters have a vertexIndex of 0 because they have no vertices
                //They should be ignored to avoid messing up the first character in the string which also has a vertexIndex of 0
                if (characterInfo.isVisible)
                {
                    int vertexIndex = characterInfo.vertexIndex;
                    int materialIndex = characterInfo.materialReferenceIndex;
                    destinationColors = textInfo.meshInfo[materialIndex].colors32;

                    textBox.mesh.colors32 = destinationColors;

                    // Get the stored verticies and the current verticies
                    Vector3[] sourceVertices = cachedMeshInfo[materialIndex].vertices;
                    Vector3[] destinationVertices = textInfo.meshInfo[materialIndex].vertices;

                    float characterTime = 0;
                    float characterAnimStartTime = characterAnimStartTimes[j]; 

                    // Wait between each character. Max 1 second
                    if (characterAnimStartTime >= 0)
                    {
                        float timeSinceAnimStart = Time.unscaledTime - characterAnimStartTime;
                        characterTime = Mathf.Min(1, timeSinceAnimStart / CHAR_ANIM_TIME); 
                    }

                    // May need to get text font size of each character here, check back later
                    Vector3 animPositionAdjust = GetAnimPositionAdjustment(textAnimInfo, j, textBox.fontSize, Time.unscaledTime);
                    Vector3 offset = (sourceVertices[vertexIndex] + sourceVertices[vertexIndex + 2]) / 2;

                    for (int i = 0; i < 4; i++)
                        destinationVertices[vertexIndex + i] = ((sourceVertices[vertexIndex + i] - offset) * characterTime) + offset + animPositionAdjust; 
                }
            }

            // Update the vertex data
            textBox.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32); 

            for (int i = 0; i < textInfo.meshInfo.Length; i++)
            {
                // Get index mesh and update its Geometry
                TMP_MeshInfo info = textInfo.meshInfo[i];
                info.mesh.vertices = info.vertices;
                textBox.UpdateGeometry(info.mesh, i); 
            }
            yield return null; 
        }
    }
    public void ClearText()
    {
        for (int i = 0; i < previousCharacterCount; i++)
        {
            TMP_CharacterInfo characterInfo = textBox.textInfo.characterInfo[i];
            TMP_TextInfo textInfo = textBox.textInfo;

            if (characterInfo.isVisible)
            {
                int vertexIndex = characterInfo.vertexIndex;
                int materialIndex = characterInfo.materialReferenceIndex;
                Color32[] currentColors = textInfo.meshInfo[materialIndex].colors32;

                for (int j = 0; j < currentColors.Length; j++)
                {
                    currentColors[j] = (Color32)Color.clear;
                }

                textBox.mesh.colors32 = currentColors;
            }
        }
    }
    private static bool CanShowNextCharacter(float secondsPerCharacter, float timeOfLastCharacter)
    {
        // If the difference in the unscaled time and the time the last character played is greater than seconds per character, show next character
        return (Time.unscaledTime - timeOfLastCharacter) > secondsPerCharacter;         
    }

    private void ExecuteRemainingCommandsAtIndex(List<DialogCommand> commands, int visibleCharacterIndex, ref float secondsPerCharacter, ref float timeOfLastCharacter)
    {
        // At the current index, go through the remaining DialogCommands and apply the effects if they are at the current index
        for (int i = 0; i < commands.Count; i++)
        {
            DialogCommand currentCommand = commands[i];
            if (currentCommand.position == visibleCharacterIndex)
            {
                switch (currentCommand.type)
                {
                    // Add a pause between characters using timeOfLastCharacter
                    case TextCommandType.Pause:
                        timeOfLastCharacter = Time.unscaledTime + currentCommand.floatValue;
                        break;
                    // Set the text speed with seconds per character
                    case TextCommandType.TextSpeedChange: 
                        secondsPerCharacter = 1f / currentCommand.floatValue;
                        break;
                    case TextCommandType.VoiceBark:
                        UpdateDialogSound(currentCommand.stringValue, currentCommand.floatValue);
                        break; 
                }
                // Remove the DialogCommand from this list once it is executed
                commands.RemoveAt(i);
                i--; 
            }
        }
    }

    private Color32[] RetrieveDestinationColors(int characterCount, TMP_TextInfo textInfo, TextColorInfo[] textColorInfo, TMP_MeshInfo[] cachedMeshInfo)
    {
        Color32[][] originalColors = new Color32[textInfo.meshInfo.Length][];
        Color32[] newDestinationColors = null; 

        // Get all the original colors
        for (int i = 0; i < originalColors.Length; i++)
        {
            // Get colors of current mesh, create an array the length of the colors we just got, and copy them over to keep track before modifying. 
            Color32[] theColors = textInfo.meshInfo[i].colors32;
            originalColors[i] = new Color32[theColors.Length];
            Array.Copy(theColors, originalColors[i], theColors.Length);
        }

        int vertexIndex = 0; 
        List<Color32> vertexColors = new List<Color32>();

        // Go through all of the characters and set the colors and the animation adjustments
        for (int i = 0; i < characterCount; i++)
        {
            // Character Info has a lot of stuff, but we'll mostly be using vertex info from here
            TMP_CharacterInfo characterInfo = textInfo.characterInfo[i];

            int materialIndex = characterInfo.materialReferenceIndex;
            newDestinationColors = textInfo.meshInfo[materialIndex].colors32;

            // Set the text to clear by default so we can show one character at a time
            Color32 thisColor = clear;

            //Invisible characters have a vertexIndex of 0 because they have no vertices
            //They should be ignored to avoid messing up the first character in the string which also has a vertexIndex of 0
            if (characterInfo.isVisible)
            {
                bool insideBrackets = false;

                if (textColorInfo.Length > 0)
                {
                    for (int c = 0; c < textColorInfo.Length; c++)
                    {
                        if ((i >= textColorInfo[c].startIndex) && (i < textColorInfo[c].endIndex))
                        {
                            insideBrackets = true;
                            //thisColor = textColorInfo[c].color;
                            vertexColors.Add(textColorInfo[c].color);
                            break;
                        }
                        else if (i >= textColorInfo[c].endIndex)
                        {
                            insideBrackets = false;
                        }
                    }
                    if (!insideBrackets)
                    {
                        //thisColor = originalColors[materialIndex][vertexIndex]; 
                        vertexColors.Add(originalColors[materialIndex][vertexIndex]);
                    }
                }
                else
                {
                    //thisColor = originalColors[materialIndex][vertexIndex];
                    vertexColors.Add(originalColors[materialIndex][vertexIndex]);
                }
            }
        }

        for (int j = 0; j < vertexColors.Count; j++)
        {
            for (int k = 0; k < 4; k++)
            {
                newDestinationColors[vertexIndex] = vertexColors[j];
                vertexIndex++;
            }
        }

        return newDestinationColors;
    }

    #region AUDIO 
    const float pitchVariability = 0.01f; 

    private void UpdateDialogSound(string _vcType, float _vcRate)
    {
        vcRate = speaker.UpdateVoiceBarkRate((int)_vcRate);
        vcBarks = speaker.GetVoiceBarkType(_vcType);

        // Set first character to equal vcRate (minus 1 because count will be added +1 after this call) so that it always plays on first character
        countSinceLastBark = vcRate - 1; 
    }
    private void PlayDialogSound()
    {
        if (vcBarks.Count > 0)
        {
            if (countSinceLastBark == vcRate)
            {
                // We don't want sounds to overlap
                //audioSource.Stop();

                int randomClip = UnityEngine.Random.Range(0, vcBarks.Count);

                while (randomClip == previousBarkPosition)
                {
                    randomClip = UnityEngine.Random.Range(0, vcBarks.Count);
                }

                // Random pitch
                audioSource.pitch = UnityEngine.Random.Range(1 - pitchVariability, 1 + pitchVariability); 
                audioSource.PlayOneShot(vcBarks[randomClip]);

                // Reset count
                countSinceLastBark = 0;
            }
        }
    }
    #endregion

    #region ANIMATION ADJUSTMENT VARIABLES
    const float SHAKE_MAGNITUDE = 0.06f;
    const float SHAKE_FREQUENCY = 15f;
    const float WAVE_MAGNITUDE = 0.06f;
    const float shakeAdjuster = 0.5f; //Makes it feel better
    const float waveAdjuster = 1.5f;
    const float crestCharCount = 6; 
    #endregion

    /// <summary>
    /// Animate all of the different text anim infos
    /// </summary>
    /// <param name="textAnimInfo"></param>
    /// <param name="characterIndex"></param>
    /// <param name="fontSize"></param>
    /// <param name="time"></param>
    /// <returns></returns>
    private Vector3 GetAnimPositionAdjustment(TextAnimInfo[] textAnimInfo, int characterIndex, float fontSize, float time)
    {
        float x = 0, y = 0; 

        // Loop through all of the text anim infos and apply time to their animations
        for (int i = 0; i < textAnimInfo.Length; i++)
        {
            TextAnimInfo currentInfo = textAnimInfo[i];

            // Check if this character is inside of the indexes for the animation type
            if (characterIndex >= currentInfo.startIndex && characterIndex < currentInfo.endIndex)
            {
                // Shake
                if (currentInfo.type == TextAnimationType.shake)
                {
                    float scaleAdjustment = fontSize * SHAKE_MAGNITUDE;
                    x += (Mathf.PerlinNoise((characterIndex + time) * SHAKE_FREQUENCY, 0) - shakeAdjuster) * scaleAdjustment; 
                    y += (Mathf.PerlinNoise((characterIndex + time) * SHAKE_FREQUENCY, 1000) - shakeAdjuster) * scaleAdjustment;
                }
                // Wave
                else if (currentInfo.type == TextAnimationType.wave)
                {
                    y += Mathf.Sin((characterIndex * waveAdjuster) + (time * crestCharCount)) * fontSize * WAVE_MAGNITUDE; 
                }
            }
        }
        return new Vector3(x, y, 0);
    }

    private void FinishAnimating()
    {
        // Call this when the animation is finished. 
        isTextAnimating = false;
        stopAnimating = false;
        DialogManager.Instance.SetNextLineIndicatorState(true); 
    }
    public void FinishCurrentAnimation()
    {
        secondsPerCharacter = 0;

        if (isTextAnimating)
            stopAnimating = true;
    }

    #region SEPARATE OUT INFO 
    private TextAnimInfo[] SeparateOutTextAnimInfo(List<DialogCommand> commands)
    {
        List<TextAnimInfo> tempResult = new List<TextAnimInfo>();
        List<DialogCommand> animStartCommands = new List<DialogCommand>();
        List<DialogCommand> animEndCommands = new List<DialogCommand>();

        // Go through all of the commands being sent in (this list comes from DialogueUtility)
        // Remove anims from the commands list as you go and add them to a local list
        for (int i = 0; i < commands.Count; i++)
        {
            DialogCommand currentCommand = commands[i];

            bool isValidType = true; 
            switch (currentCommand.type)
            {
                case TextCommandType.AnimStart:
                    animStartCommands.Add(currentCommand);
                    break; 
                case TextCommandType.AnimEnd:
                    animEndCommands.Add(currentCommand);
                    break; 
                default:
                    isValidType = false; 
                    break;
            }
            if (isValidType)
            {
                commands.RemoveAt(i);
                i--;
            }
        }

        if (animStartCommands.Count != animEndCommands.Count)
            Debug.LogError("Unequal number of start and end animation commands. Start Commands: " + animStartCommands.Count + " End Commands: " + animEndCommands.Count);
        // Create a textAnimInfo struct for the animations 
        else
        {
            for (int i = 0; i < animStartCommands.Count; i++)
            {
                DialogCommand startCommand = animStartCommands[i];
                DialogCommand endCommand = animEndCommands[i];

                tempResult.Add(new TextAnimInfo
                {
                    startIndex = startCommand.position,
                    endIndex = endCommand.position,
                    type = startCommand.textAnimValue
                });
            }
        }

        return tempResult.ToArray(); 
    }
    private TextColorInfo[] SeparateOutTextColorInfo(List<DialogCommand> commands)
    {
        List<TextColorInfo> tempResult = new List<TextColorInfo>();
        List<DialogCommand> colorStartCommands = new List<DialogCommand>();
        List<DialogCommand> colorEndCommands = new List<DialogCommand>();

        // Go through all of the commands being sent in (this list comes from DialogueUtility)
        // Remove colors from the commands list as you go and add them to a local list
        for (int i = 0; i < commands.Count; i++)
        {
            DialogCommand currentCommand = commands[i];

            bool isValidType = true;
            switch (currentCommand.type)
            {
                case TextCommandType.ColorStart:
                    colorStartCommands.Add(currentCommand);
                    break;
                case TextCommandType.ColorEnd:
                    colorEndCommands.Add(currentCommand);
                    break;
                default:
                    isValidType = false;
                    break;
            }
            if (isValidType)
            {
                commands.RemoveAt(i);
                i--;
            }
        }

        if (colorStartCommands.Count != colorEndCommands.Count)
            Debug.LogError("Unequal number of start and end color commands. Start Commands: " + colorStartCommands.Count + " End Commands: " + colorEndCommands.Count);
        // Create a textAnimInfo struct for the animations 
        else
        {
            for (int i = 0; i < colorStartCommands.Count; i++)
            {
                DialogCommand startCommand = colorStartCommands[i];
                DialogCommand endCommand = colorEndCommands[i];

                tempResult.Add(new TextColorInfo
                {
                    startIndex = startCommand.position,
                    endIndex = endCommand.position,
                    color = startCommand.color
                });
            }
        }

        return tempResult.ToArray();
    }
    #endregion
}

public struct TextAnimInfo
{
    public int startIndex;
    public int endIndex;
    public TextAnimationType type;
}

public struct TextColorInfo
{
    public int startIndex;
    public int endIndex;
    public Color32 color;
}
