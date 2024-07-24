using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[AddComponentMenu("Camera Effects/Pixelation")]
public class Pixelation : MonoBehaviour
{
    public float scale = 1;
    [Header("URP pipeline")]
    [SerializeField] UniversalRenderPipelineAsset urpAsset;
    [Header("Speed")]
    [SerializeField] float pixelSpeed = .05f;
    [SerializeField] float fadeToBlackTime = 1.0f;

    bool currentlyLoading = false;
    public bool IsLoading { get => currentlyLoading; }

    private void Awake() => StartCoroutine(LoadingInto());
    public void LoadScene() => StartCoroutine(LoadingOut());

    IEnumerator LoadingInto()
    {
        currentlyLoading = true;
        scale = 0;
        urpAsset.renderScale = scale;

        yield return new WaitForSeconds(fadeToBlackTime);
        while (scale < 1)
        {
            scale += pixelSpeed;
            urpAsset.renderScale = scale;

            yield return null;
        }
        scale = 1;
        urpAsset.renderScale = scale;

        currentlyLoading = false;
    }

    IEnumerator LoadingOut()
    {
        currentlyLoading = true;
        scale = 1;

        while (scale > 0)
        {
            scale -= pixelSpeed;
            urpAsset.renderScale = scale;

            yield return null;
        }
        scale = 0;
        urpAsset.renderScale = scale;

        yield return new WaitForSeconds(fadeToBlackTime);
        currentlyLoading = false;
    }

    private void OnDisable()
    {
        //To ensure that scale is set to one when closing
        scale = 1;
        urpAsset.renderScale = scale;
    }
}
