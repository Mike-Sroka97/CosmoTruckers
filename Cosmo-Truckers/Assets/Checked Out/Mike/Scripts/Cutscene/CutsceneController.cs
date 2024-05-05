using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CutsceneController : MonoBehaviour
{
    [SerializeField] protected float bufferTime = 1.5f;

    protected CameraController cameraController;

    private void Start()
    {
        cameraController = FindObjectOfType<CameraController>();
        StartCoroutine(Buffer());
    }

    IEnumerator Buffer()
    {
        yield return new WaitForSeconds(bufferTime);
        StartCoroutine(cameraController.FadeVignette(true));

        while (cameraController.ExecutingCommand)
            yield return null;

        StartCoroutine(CutsceneCommands());
    }

    protected abstract IEnumerator CutsceneCommands();
}
