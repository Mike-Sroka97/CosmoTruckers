using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialHUBCutscene : CutsceneController
{
    
    Animator eyeToActivate;
    [HideInInspector] public bool EyeStuffHappening;

    protected override IEnumerator CutsceneCommands()
    {
        eyeToActivate = GetComponent<Animator>();
        eyeToActivate.enabled = true;
        EyeStuffHappening = true;

        while (EyeStuffHappening)
            yield return null;
    }

    public void EyeDone()
    {
        EyeStuffHappening = false;
        eyeToActivate.speed = 0;
    }
}
