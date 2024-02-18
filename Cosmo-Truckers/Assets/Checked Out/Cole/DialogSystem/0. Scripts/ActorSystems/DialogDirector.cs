using Ink;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DialogDirector : MonoBehaviour
{
    private TextAsset textFile; 
    private string[] dialogs;
    private string baseDialog; 
    private string[] playerNames = new string[] {"AEGLAR","SAFE-T","PROTO","SIX-FACE"};
    private BaseActor[] actors;
    private int currentLineIndex = -1; 
    private int currentID = 1;
    private TextParser textParser;

    private void Start()
    {
        textParser = GetComponent<TextParser>();
    }

    // Methods
    private void SetScene(TextAsset _textFile, List<BaseActor> _actors)
    {
        textFile = _textFile; 
        dialogs = new string[_actors.Count];
        baseDialog = textParser.GetAllDialogs(textFile)[0];
        List<BaseActor> actors = new List<BaseActor>();

        for (int i = 0; i < 4; i++)
        {
            actors.Add(_actors[i]);
        }

        GetActorsAndDialogs(actors); 
    }

    private void GetActorsAndDialogs(List<BaseActor> _actors)
    {
        List<BaseActor> playerActors = _actors; 
        BaseActor[] finalPlayerActors = new BaseActor[4];

        // Go through the player actors
        for (int i = 0; i < 4; i++)
        {
            foreach (BaseActor actor in playerActors)
            {
                // Base player is in party
                if (playerNames[i] == actor.actorName)
                {
                    finalPlayerActors[i] = actor;
                    finalPlayerActors[i].actorID = i + 1;
                    //This is a base character, so we will use the first dialog
                    dialogs[i] = textParser.GetAllDialogs(textFile)[0]; 
                    playerActors.Remove(actor);
                    break; 
                }
            }
        }

        //If there are any missing spots in the final player actors, fill them one at a time
        for (int i = 0; i < playerActors.Count; i++)
        {
            for (int j = 0; j < finalPlayerActors.Length; j++)
            {
                if (finalPlayerActors[j] == null)
                {
                    finalPlayerActors[j] = playerActors[i];
                    dialogs[j] = textParser.GetActorDialog(textFile, finalPlayerActors[j].actorName); 
                    finalPlayerActors[j].actorID = j + 1;
                    break; 
                }
            }
        }
    }

    #region Actor Commands
    public void MoveActor(BaseActor actor, Vector3 actorDestination, float speed = 1)
    {

    }
    public void CommandActor(BaseActor actor)
    {

    }
    public void CommandAllActors(bool justPlayerActors = true) // call a BaseActor.cs Function on all actors
    {

    }
    #endregion

    private void EndScene()
    {

    }

    // Update is called once per frame
    void Update()
    {
        CheckPlayerInput(); 
    }

    private void CheckPlayerInput()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            bool doneAnimating = DialogManager.instance.CheckDialogCompletion(); 

            if (!doneAnimating) { DialogManager.instance.StopAnimating(); }

            else
            {
                AdvanceScene(); 
            }
        }
    }

    private void AdvanceScene()
    {
        // Increment the current line
        currentLineIndex++;

        // At the current line in the base dialog, get the tags
        string[] tags = textParser.GetTagsAtCurrentLine(baseDialog, currentLineIndex);
        string speakerDialog = null;

        // Get the actor ID for this line and the dialog associated with that actor
        if (int.TryParse(tags[0], out currentID))
        {
            speakerDialog = dialogs[currentID - 1]; // ID's in text will be on a starting scale of 1
        }
        else
        {
            Debug.LogError("Unable to parse int out of first tag!");
            speakerDialog = baseDialog; 
        }

        // Get the line associated with this actor and their dialog
        string currentLine = textParser.GetTextAtCurrentLine(speakerDialog, currentLineIndex);

        // Tell the actor to deliver the line
        actors[currentID - 1].DeliverLine(currentLine); 
    }
}
