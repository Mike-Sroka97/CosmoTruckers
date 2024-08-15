using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DungeonController : MonoBehaviour
{
    [SerializeField] protected bool debugging;

    public OverworldNode CurrentNode;
    public float minCameraX;
    public float maxCameraX;
    public float minCameraY;
    public float maxCameraY;

    protected bool enableLeaderOnFade = true;

    private void Start()
    {
        DungeonInitialize();
    }

    public void CameraFadeFinished()
    {
        if (enableLeaderOnFade)
            CameraController.Instance.Leader.GetComponent<OverworldCharacter>().enabled = true;
    }

    protected abstract void DungeonInitialize();
}
