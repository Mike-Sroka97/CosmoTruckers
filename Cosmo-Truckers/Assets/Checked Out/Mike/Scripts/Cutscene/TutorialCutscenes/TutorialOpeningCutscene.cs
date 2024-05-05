using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialOpeningCutscene : CutsceneController
{
    [SerializeField] float textWaitTime;
    [SerializeField] Vector3 metrodonClusterPosition;
    [SerializeField] float cameraMoveSpeed;

    protected override IEnumerator CutsceneCommands()
    {
        //Show text
        StartCoroutine(cameraController.FadeText(true));

        while (cameraController.ExecutingCommand)
            yield return null;

        //Fade text out and move camera simultaneously (can't use a ExecutingCommand check here so we use a camera pos check)
        yield return new WaitForSeconds(textWaitTime);

        StartCoroutine(cameraController.FadeText(false));
        StartCoroutine(cameraController.MoveTowardsPosition(metrodonClusterPosition, cameraMoveSpeed, true));
    }
}
