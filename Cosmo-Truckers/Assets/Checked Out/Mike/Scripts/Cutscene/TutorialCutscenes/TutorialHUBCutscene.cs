using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialHUBCutscene : CutsceneController
{
    [SerializeField] GameObject gorpToEnable;
    [SerializeField] GameObject roy;
    
    Animator eyeToActivate;
    [HideInInspector] public bool EyeStuffHappening;

    protected override IEnumerator CutsceneCommands()
    {
        eyeToActivate = GetComponent<Animator>();
        eyeToActivate.enabled = true;
        EyeStuffHappening = true;

        while (EyeStuffHappening)
            yield return null;

        yield return new WaitForSeconds(3f);

        StartCoroutine(cameraController.FadeVignette(false));
        while (cameraController.CommandsExecuting > 0)
            yield return null;

        GetComponent<SpriteRenderer>().enabled = false;
        gorpToEnable.SetActive(true);

        StartCoroutine(cameraController.FadeVignette(true));
        while (cameraController.CommandsExecuting > 0)
            yield return null;

        yield return new WaitForSeconds(5f);

        StartCoroutine(cameraController.FadeVignette(false));

        while (cameraController.CommandsExecuting > 0)
            yield return null;

        yield return new WaitForSeconds(3f);

        gorpToEnable.SetActive(false);
        roy.SetActive(true);

        StartCoroutine(cameraController.FadeVignette(true));

        while (cameraController.CommandsExecuting > 0)
            yield return null;

        End();
    }

    public void EyeDone()
    {
        EyeStuffHappening = false;
        eyeToActivate.speed = 0;
    }
}
