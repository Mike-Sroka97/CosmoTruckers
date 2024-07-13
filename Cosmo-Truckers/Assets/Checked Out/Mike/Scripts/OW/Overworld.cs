using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Overworld : MonoBehaviour
{
    //TODO MAKE INHERITANT CLASS FOR EACH DIMENSION. HAVE THE START FUNCTION UNLOCK NODES BASED ON SAVE DATA

    public OverworldNode[] Nodes;
    public OverworldNode CurrentNode;
    public float minCameraX;
    public float maxCameraX;

    new CameraController camera;

    private void Start()
    {
        camera = FindObjectOfType<CameraController>();
        StartCoroutine(camera.FadeVignette(true));
    }
}
