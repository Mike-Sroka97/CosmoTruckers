using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class CutsceneController : MonoBehaviour
{
    [SerializeField] protected float bufferTime = 1.5f;
    [SerializeField] string sceneToLoad; 

    // Dialog Scene Setup Variables
    [SerializeField] TextAsset[] textFiles;

    protected CameraController cameraController;

    private void Start()
    {
        cameraController = FindObjectOfType<CameraController>();
        StartCoroutine(Buffer());
    }

    protected virtual void End()
    {
        SceneManager.LoadScene(sceneToLoad);
    }

    IEnumerator Buffer()
    {
        yield return new WaitForSeconds(bufferTime);
        StartCoroutine(cameraController.FadeVignette(true));

        while (cameraController.CommandsExecuting > 0)
            yield return null;

        StartCoroutine(CutsceneCommands());
    }

    protected abstract IEnumerator CutsceneCommands();


    /// <summary>
    /// Setup the Dialog.
    /// </summary>
    protected void DialogSetup()
    {
        BaseActor[] sortedActors = NextDialogSetup();

        List<GameObject> actorPrefabs = GetActorPrefabList(sortedActors);
        DialogManager.Instance.SetBaseActors(SpawnActorsIn(actorPrefabs));
    }

    private BaseActor[] NextDialogSetup()
    {
        DialogManager.Instance.DialogIsPlaying = true;
        DialogManager.Instance.SetupDialogs(textFiles[DialogManager.Instance.CurrentTextFile]);

        // Sort the player actors
        BaseActor[] sortedActors = SortBasePlayerActors(DialogManager.Instance.PlayerActors);

        return sortedActors; 
    }

    protected void DialogNextSetup()
    {
        NextDialogSetup(); 
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
            for (int j = 0; j < DialogManager.BasePlayerNames.Length; j++)
            {
                // This is one of the base players. Set them to the correct position of the base player. 
                if (DialogManager.BasePlayerNames[j] == actor.actorName)
                {
                    finalPlayerActors[j] = actor;
                    finalPlayerActors[j].actorID = j + 1;
                    //This is a base character, so we will use the first dialog
                    DialogManager.Instance.SetDialog(j, DialogManager.Instance.TextParser.GetAllDialogs(textFiles[DialogManager.Instance.CurrentTextFile])[0]);
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
                    finalPlayerActors[j] = remainingPlayerActors[i];
                    // Set the dialog for this player slot to be a specific actor dialog
                    DialogManager.Instance.SetDialog(j, DialogManager.Instance.TextParser.GetActorDialog(textFiles[DialogManager.Instance.CurrentTextFile], remainingPlayerActors[i].actorName)); 
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
        List<GameObject> playerActorPrefabs = DialogManager.Instance.ActorList.GetPlayerActorPrefabs();
        List<GameObject> otherActorPrefabs = DialogManager.Instance.ActorList.GetAdditionalActorPrefabs();

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

    private BaseActor[] SpawnActorsIn(List<GameObject> actorPrefabs)
    {
        // Get all the actor spots
        ActorSpot[] actorSpots = FindObjectsOfType<ActorSpot>();

        // Make a list of actors the size of how many spots
        BaseActor[] actors = new BaseActor[actorSpots.Length];

        // Go through all of the actor spots found. NPCs should have actor spots on their prefab
        for (int i = 0; i < actorSpots.Length; i++)
        {
            BaseActor npcActor = actorSpots[i].GetComponent<BaseActor>();

            // If it's an NPC actor, initialize and put the actor into this list
            if (npcActor != null)
            {
                npcActor.Initialize(actorSpots[i].GetSortingLayer(), actorSpots[i].GetFacingRight());

                // Set the actor in the correct spot in the array
                actors[actorSpots[i].GetActorNumber() - 1] = npcActor;
            }
            // Otherwise it's a player actor
            else
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
        }

        return actors;
    }

    private void TryToAdvanceDialog()
    {
        if (DialogManager.Instance.CanAdvanceDialog())
        {
            if (DialogManager.Instance.CheckIfDialogTextAnimating()) 
            {
                if (DialogManager.Instance.CanSkipDialogText())
                    DialogManager.Instance.StopAnimating(); 
            }
            else { StartCoroutine(DialogManager.Instance.AdvanceScene()); }
        }
    }

    public void CheckPlayerInput()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TryToAdvanceDialog(); 
        }
    }
}
