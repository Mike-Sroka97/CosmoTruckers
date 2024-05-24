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

    public void SetupINATalker(List<TextAsset> dialogs, List<Transform> textBoxPositions)
    {
        textManager = FindObjectOfType<RegularTextManager>();
        inaDialogCounter = -1;
        TalkingCommandsRunning = 0;
        inaDialogs = dialogs;
        textPositions = textBoxPositions;

        if (inaDialogs.Count != textPositions.Count)
            Debug.LogError("These need to be even to work correctly"); 
    }

    public void INAStartNextDialog()
    {
        CheckingDialog = true;
        StartCoroutine(INATrackNextDialog());
    }

    private IEnumerator INATrackNextDialog()
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
            textManager.StartRegularTextMode(inaDialogs[inaDialogCounter], textPositions[inaDialogCounter]);
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
