using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[AddComponentMenu("Camera Effects/Pixelation")]
public class Pixelation : MonoBehaviour
{
    [Header("Scale")]
    [SerializeField] float scale = 1;
    [SerializeField] float minScale = -1;
    [Header("URP pipeline")]
    [SerializeField] UniversalRenderPipelineAsset urpAsset;
    [Header("Speed")]
    [SerializeField] float pixelSpeed = .05f;

    private float localScale;
    [ContextMenu("Test")]

    public void Unpixelate()
    {
        StopAllCoroutines();
        StartCoroutine(LoadingInto());
    }
    public void Pixelate()
    {
        StopAllCoroutines();
        StartCoroutine(LoadingOut());
    }

    public IEnumerator LoadingInto()
    {
        localScale = 0;
        urpAsset.renderScale = scale;

        while (localScale < 1)
        {
            localScale += pixelSpeed * Time.deltaTime;
            urpAsset.renderScale = localScale;

            yield return null;
        }

        localScale = scale;
        urpAsset.renderScale = localScale;
        urpAsset.supportsHDR = true;
    }

    public IEnumerator LoadingOut()
    {
        localScale = scale;

        while (localScale > minScale)
        {
            localScale -= pixelSpeed * Time.deltaTime;
            urpAsset.renderScale = localScale;

            yield return null;
        }

        localScale = minScale;
        urpAsset.renderScale = localScale;
        urpAsset.supportsHDR = false;
        CameraController.Instance.CommandsExecuting--;
    }

    private void OnDisable()
    {
        //To ensure that scale is set to one when closing
        urpAsset.renderScale = scale;
        urpAsset.supportsHDR = true;
    }
}
