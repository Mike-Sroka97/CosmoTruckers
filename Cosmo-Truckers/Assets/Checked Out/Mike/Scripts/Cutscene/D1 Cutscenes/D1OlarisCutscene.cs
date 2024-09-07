using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class D1OlarisCutscene : CutsceneController
{
    [SerializeField] BaseActor[] playerActors;
    [SerializeField] float dialogBoxOffsetMaxSize = 1f; 

    protected override IEnumerator Buffer()
    {
        // Set the dialog box's max size to be 1 instead of 0.75
        DialogManager.Instance.SetDialogBoxOffsetMaxSize(dialogBoxOffsetMaxSize: dialogBoxOffsetMaxSize); 

        // Set the actors at the beginning so when the vignette fades they are there
        // We want the cutscene to be interactable, so set isCutscene to false, even though it is a "cutscene"
        DialogManager.Instance.SetPlayerActors(GetActorsFromCurrentPlayers(), isCutscene: false);

        // Setup Dialog
        DialogSetup();

        // wait until actors are spawned in
        while (!spawnedInActors)
            yield return null;

        yield return new WaitForSeconds(bufferTime);
        StartCoroutine(cameraController.FadeVignette(true));

        while (cameraController.CommandsExecuting > 0)
            yield return null;

        StartCoroutine(CutsceneCommands());
    }

    protected override IEnumerator CutsceneCommands()
    {
        // give a second before starting the dialog
        yield return new WaitForSeconds(1f);

        // START DIALOG
        StartCoroutine(DialogManager.Instance.AdvanceScene());

        while (DialogManager.Instance.DialogIsPlaying)
            yield return null;

        StartCoroutine(EndWithFade()); 
    }

    private void Update()
    {
        CheckPlayerInput();
    }

    /// <summary>
    /// Pass in information of the current players in the party. Return the base actors 
    /// </summary>
    /// <returns></returns>
    private List<BaseActor> GetActorsFromCurrentPlayers()
    {
        // For now, just return the base player list 
        // Later on, delete this variable and use the Dialog Manager's actor list 
        return playerActors.ToList(); 
    }
}
