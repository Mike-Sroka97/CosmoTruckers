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

    private void Start()
    {
        OverworldInitialize();
    }

    protected abstract void OverworldInitialize();

    protected abstract void DebugInput();
}
