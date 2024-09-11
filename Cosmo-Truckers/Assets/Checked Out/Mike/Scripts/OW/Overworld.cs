using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Overworld : MonoBehaviour
{
    [SerializeField] protected bool debugging;

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
    }

    public void CameraFadeFinished()
    {
        if (enableLeaderOnFade && !LoadingScene)
            CameraController.Instance.Leader.GetComponent<OverworldCharacter>().enabled = true;
    }

    protected abstract void OverworldInitialize();

    protected abstract void SetupStartingNode(); 

    protected abstract void DebugInput();
}
