using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
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

    private static readonly string[] basePlayerNames = new string[] {"AEGLAR", "SAFE-T", "PROTO", "SIX FACE"};

    /*

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

        // Delay wait time before starting dialog
        yield return new WaitForSeconds(2f); //TODO AHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHH

        StartCoroutine(AdvanceScene());

    }
    #endregion

    #region Values 

    #endregion

    // Update is called once per frame
    void Update()
    {
        CheckPlayerInput();
    }
    */ 
}
