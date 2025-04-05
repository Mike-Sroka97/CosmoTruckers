using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Overworld : MonoBehaviour
{
    [SerializeField] protected bool debugging;
    [SerializeField] string dataFileKey;

    public OverworldNode CurrentNode;
    public float minCameraX;
    public float maxCameraX;
    public float minCameraY;
    public float maxCameraY;

    [HideInInspector] public bool LoadingScene;

    protected bool enableLeaderOnFade = true;

    private void Start()
    {
        OverworldInitialize();

        DataLogData dataLogData = SaveManager.LoadDataLogData();
        dataLogData.SaveDataFileUnlock(dataFileKey);

    }

    public void CameraFadeFinished()
    {
        if (enableLeaderOnFade && !LoadingScene)
            CameraController.Instance.Leader.GetComponent<OverworldCharacter>().enabled = true;
    }

    protected abstract void OverworldInitialize();

    protected virtual void SetupStartingNode()
    {
        string lastNodeName = CameraController.Instance.LastNode;
        GameObject startNode = GameObject.Find(lastNodeName);

        // Only call this portion when loading into the scene
        CurrentNode = startNode != null ? startNode.GetComponent<OverworldNode>() : CurrentNode;

        // Set the player to the position of the previous node
        OverworldCharacter mapPlayer = GameObject.Find("OW_ControllerCharacter").GetComponent<OverworldCharacter>();
        mapPlayer.transform.position = CurrentNode.transform.position;

        // Setup the current node 
        CurrentNode.SetupNode();
    }

    protected abstract void DebugInput();
}
