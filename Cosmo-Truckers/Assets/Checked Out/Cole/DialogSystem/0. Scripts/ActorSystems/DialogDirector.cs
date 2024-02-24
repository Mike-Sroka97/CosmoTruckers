using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogDirector : MonoBehaviour
{
    // Scene Setup Variables
    private TextAsset textFile;
    private GameObject sceneLayout;

    private BaseActor[] actors;
    private TextParser textParser;
    private ActorList actorList; 

    private string[] dialogs;
    private string baseDialog; 
    private int currentLineIndex = 0; 
    private int currentID = 1;
    private int lastID = -1;
    private int allLinesCount = 0; 

    private static readonly string[] basePlayerNames = new string[] { "AEGLAR", "SAFE-T", "PROTO", "SIX FACE" };

    private void Start()
    {
        StartCoroutine(SetScene()); 
    }

    #region Scene Setup
    public IEnumerator SetScene()
    {
        GetScripts(); 

        List<BaseActor> playerActors = new List<BaseActor>();
        DialogManager.Instance.GetSceneInformation(ref sceneLayout, ref textFile, ref playerActors);

        Instantiate(sceneLayout); 

        dialogs = new string[playerActors.Count];
        baseDialog = textParser.GetAllDialogs(textFile)[0];

        BaseActor[] sortedActors = SortBasePlayerActors(playerActors);
        List<GameObject> actorPrefabs = GetActorPrefabList(sortedActors);
        actors = SpawnActorsIn(actorPrefabs);

        DialogManager.Instance.SetFading(true); 
        StartCoroutine(DialogManager.Instance.FadeFader(fadeIn: false));

        while (DialogManager.Instance.CheckIfFading())
            yield return null; 

        // Delay wait time before starting dialog
        StartCoroutine(StartScene(2f)); 

    }
    IEnumerator StartScene(float startSceneWaitTime)
    {
        yield return new WaitForSeconds(startSceneWaitTime); 
        AdvanceScene();
    }

    /// <summary>
    /// Go through the base player actors.
    /// Sort them into the correct index for Players 1-4 in a BaseActor array. 
    /// If actors passed in aren't base players, they will replace on of the base players in the original sorted list. 
    /// </summary>
    /// <param name="_actors"></param>
    /// <returns></returns>
    private BaseActor[] SortBasePlayerActors(List<BaseActor> _actors)
    {
        // There's always going to be 4 players
        BaseActor[] finalPlayerActors = new BaseActor[4];
        List<BaseActor> playerActors = _actors;

        // List for actors not found in base players. Remove from this when finding base player actor
        List<BaseActor> remainingPlayerActors = new List<BaseActor>();

        // Go through all passed in player actors
        foreach (BaseActor actor in playerActors)
        {
            bool basePlayerFound = false;

            // Go through all base player names
            for (int j = 0; j < basePlayerNames.Length; j++)
            {
                // This is one of the base players. Set them to the correct position of the base player. 
                if (basePlayerNames[j] == actor.actorName)
                {
                    finalPlayerActors[j] = actor;
                    finalPlayerActors[j].actorID = j + 1;
                    //This is a base character, so we will use the first dialog
                    dialogs[j] = textParser.GetAllDialogs(textFile)[0];
                    basePlayerFound = true;
                }

                if (basePlayerFound)
                    break; 
            }

            // If we couldn't find this actor, add them to a list
            if (!basePlayerFound)
                remainingPlayerActors.Add(actor);
        }

        //If there are any missing spots in the final player actors, fill them one at a time
        for (int i = 0; i < remainingPlayerActors.Count; i++)
        {
            // Loop through all of the final player actors
            for (int j = 0; j < finalPlayerActors.Length; j++)
            {
                if (finalPlayerActors[j] == null)
                {
                    // Set the null spots to the remaining player actors
                    finalPlayerActors[j] = playerActors[i];
                    // Set the dialog for this player slot to be a specific actor dialog
                    dialogs[j] = textParser.GetActorDialog(textFile, finalPlayerActors[j].actorName);
                    finalPlayerActors[j].actorID = j + 1;
                    break;
                }
            }
        }

        return finalPlayerActors;
    }

    /// <summary>
    /// Get all of the actor prefabs we will be spawning in. 
    /// Go through a pre-created list of prefabs for players, and compare their name. Set the prefab to the correct position in the list. 
    /// We will use this list to spawn in the actors into the scene. 
    /// </summary>
    /// <param name="_sortedActors"></param>
    /// <returns></returns>
    private List<GameObject> GetActorPrefabList(BaseActor[] _sortedActors)
    {
        List<GameObject> finalPrefabs = new List<GameObject>();
        List<GameObject> playerActorPrefabs = actorList.GetPlayerActorPrefabs(); 
        List<GameObject> otherActorPrefabs = actorList.GetAdditionalActorPrefabs();

        // We're going to use this later to set other actorID's
        int i = 0; 

        // For each of the sorted player actors, compare them to the list of player actor prefabs
        for (i = 0; i < _sortedActors.Length; i++)
        {
            bool foundPrefab = false; 

            foreach (GameObject playerObject in playerActorPrefabs)
            {
                BaseActor currentPrefabActor = playerObject.GetComponent<BaseActor>();
                
                // if the prefab has the same name as this player actor, set the id & add the prefab to the final list
                if (_sortedActors[i].actorName == currentPrefabActor.actorName)
                {
                    currentPrefabActor.actorID = _sortedActors[i].actorID;
                    finalPrefabs.Add(playerObject);
                    foundPrefab = true;
                }

                if (foundPrefab)
                    break; 
            }

            if (!foundPrefab)
                Debug.LogError("Player Actor prefab object does not exist!"); 
        }

        // Add the other actor prefabs to the list
        for (int j = 0; j < otherActorPrefabs.Count; j++)
        {
            BaseActor currentPrefabActor = otherActorPrefabs[j].GetComponent<BaseActor>();
            currentPrefabActor.actorID = i + j + 1;
            finalPrefabs.Add(otherActorPrefabs[j]);
        }
        
        return finalPrefabs;
    }

    // THIS DOESN'T TAKE ORIGINAL PLAYER ORDER INTO ACCOUNT YET. PLEASE FIX. 
    private BaseActor[] SpawnActorsIn(List<GameObject> actorPrefabs)
    {
        // Get all the actor spots
        ActorSpot[] actorSpots = FindObjectsOfType<ActorSpot>();
        // Make a list of actors the size of how many spots
        BaseActor[] actors = new BaseActor[actorSpots.Length];

        for (int i = 0; i < actorSpots.Length; i++)
        {
            GameObject prefabToSpawn = null; 

            foreach (GameObject prefab in actorPrefabs)
            {
                BaseActor currentPrefabActor = prefab.GetComponent<BaseActor>();
                // Compare if the current prefab should be spawned at this spot
                if (currentPrefabActor.actorID == actorSpots[i].GetActorNumber())
                {
                    prefabToSpawn = prefab;
                    break; 
                }
            }

            if (prefabToSpawn != null)
            {
                GameObject spawnedActor = Instantiate(prefabToSpawn, actorSpots[i].transform);
                BaseActor actor = spawnedActor.GetComponent<BaseActor>();
                actor.Initialize(actorSpots[i].GetSortingLayer(), actorSpots[i].GetFacingRight());

                // Set the actor in the correct spot in the array
                actors[actorSpots[i].GetActorNumber() - 1] = actor;
            }
            else
                Debug.LogError("Prefab couldn't find an actor spot to spawn at!");

        }

        return actors; 
    }
    #endregion

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

    #region Values 
    private bool CanAdvance()
    {
        bool canAdvance = true;

        if (currentLineIndex >= allLinesCount)
            canAdvance = false; 

        return canAdvance && !DialogManager.Instance.UpdatingDialogBox; 
    }
    private void GetScripts()
    {
        if (textParser == null)
        {
            textParser = GetComponent<TextParser>();
            actorList = GetComponent<ActorList>();
        }
    }

    #endregion

    private void CheckPlayerInput()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (CanAdvance())
            {
                if (DialogManager.Instance.CheckIfDialogAnimating()) { DialogManager.Instance.StopAnimating(); }
                else { AdvanceScene(); }
            }
        }
    }
    private void AdvanceScene()
    {
        // Increment the current line
        currentLineIndex++;

        allLinesCount = textParser.GetAllLinesInThisDialogCount(dialogs[0]); 

        // End the dialog if we've reached the line count
        if (currentLineIndex >= allLinesCount)
        {
            StartCoroutine(DialogManager.Instance.EndDialog()); 
        }
        // Otherwise, continue the dialog
        else
        {
            // At the current line in the base dialog, get the tags
            string[] tags = textParser.GetTagsAtCurrentLine(baseDialog, currentLineIndex);
            string speakerDialog = null;

            // Get the actor ID for this line and the dialog associated with that actor
            if (int.TryParse(tags[0], out currentID))
            {
                int dialogToGrab = currentID - 1;

                // Non-players don't need to worry about this. Grab the base dialog
                if (dialogToGrab > 3)
                    dialogToGrab = 0; 

                speakerDialog = dialogs[dialogToGrab]; // ID's in text will be on a starting scale of 1
            }
            else
            {
                Debug.LogError("Unable to parse int out of first tag!");
                speakerDialog = baseDialog;
            }

            // Handle the Pre Text Tags, and return any variables needed to pass into actors
            string speakerDirection = string.Empty; 
            HandlePreTextTags(tags, ref speakerDirection); 

            // Get the line associated with this actor and their dialog
            string currentLine = textParser.GetTextAtCurrentLine(speakerDialog, currentLineIndex);

            // Check if it's the first line in the dialog
            bool firstDialog = false;
            if (currentLineIndex == 0)
                firstDialog = true; 

            // Tell the actor to deliver the line
            actors[currentID - 1].DeliverLine(currentLine, lastID, firstDialog, speakerDirection);

            // Set last id after delivering
            lastID = currentID;
        }
    }
    private void HandlePreTextTags(string[] tags, ref string speakerDirection)
    {
        // Start at 1, first tag is actorID
        for (int i = 1; i < tags.Length; i++)
        {
            string[] tagValues = tags[i].Split(":"); 
            string tagKey = tagValues[0];
            string tagValue = tagValues[1];

            switch (tagKey)
            {
                case "animID":
                    int animID; 
                    if(int.TryParse(tagValue, out animID))
                    {
                        // Grab the animator
                    }
                    else
                    {
                        Debug.LogError("Can't parse value out of animID. Using default character!");
                        animID = 1; 
                    }
                    break;
                case "direction":
                    speakerDirection = tagValue;
                    break; 
                default:
                    Debug.Log("No additional Pre-Text tag found!");
                    break; 
            }

        }
    }

    private void EndScene()
    {

    }

    // Update is called once per frame
    void Update()
    {
        CheckPlayerInput();
    }
}
