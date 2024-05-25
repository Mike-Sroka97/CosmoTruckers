using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class INATalker : MonoBehaviour
{
    int TalkingCommandsRunning = 0; 

    List<TextAsset> inaDialogs;
    List<Transform> textPositions;
    int inaDialogCounter = -1;
    [HideInInspector]
    public RegularTextManager textManager;
    private bool CheckingDialog = false;

    private int textBoxPosition = 0; 

    public void SetupINATalker(List<TextAsset> dialogs, List<Transform> textBoxPositions)
    {
        if (textManager == null)
            textManager = FindObjectOfType<RegularTextManager>();

        inaDialogCounter = -1;
        TalkingCommandsRunning = 0;
        inaDialogs = dialogs;
        textPositions = textBoxPositions;
    }

    // Run dialog with the first text box position set at the center of the screen
    public void INAStartNextDialog()
    {
        CheckingDialog = true;
        StartCoroutine(INATrackNextDialog(0));
    }

    // Call this when you want dialog with the next unique text box position
    public void INAStartNextDialogWithNewTextPosition()
    {
        CheckingDialog = true;
        textBoxPosition++; 
        StartCoroutine(INATrackNextDialog(textBoxPosition));
    }

    private IEnumerator INATrackNextDialog(int textPos)
    {
        while (TalkingCommandsRunning > 0)
            yield return null;

        TalkingCommandsRunning++;
        inaDialogCounter++; 

        if (inaDialogCounter > (inaDialogs.Count - 1) || inaDialogs.Count <= 0)
        {
            Debug.LogError("You are trying to proceed at the end of the INA Dialogs list, or the list is empty!");
            CheckingDialog = false; 
        }
        else
        {
            textManager.StartRegularTextMode(inaDialogs[inaDialogCounter], textPositions[textPos]);
        }

        while (textManager.DialogIsPlaying)
            yield return null;

        CheckingDialog = false; 

        if (TalkingCommandsRunning > 0)
            TalkingCommandsRunning--; 
        else
            Debug.LogError("Talking Commands Running is at 0 but Track Next Dialog is running!");
    }

    public bool DialogPlaying()
    {
        return textManager.DialogIsPlaying || CheckingDialog; 
    }
}
