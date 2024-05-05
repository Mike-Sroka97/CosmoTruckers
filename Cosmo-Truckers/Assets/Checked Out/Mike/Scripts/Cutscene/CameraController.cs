using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] float fadeSpeed;
    [SerializeField] float textFadeSpeed;

    [HideInInspector] public bool ExecutingCommand { get; private set; }

    SpriteRenderer vignette;
    TextMeshProUGUI text;
    CutsceneController cutscene;

    private void Start()
    {
        vignette = GetComponentInChildren<SpriteRenderer>();
        text = GetComponentInChildren<TextMeshProUGUI>();
        cutscene = FindObjectOfType<CutsceneController>();
    }

    public IEnumerator FadeVignette(bool FadeIn)
    {
        ExecutingCommand = true;

        if (FadeIn)
            while (vignette.color.a > 0)
            {
                vignette.color -= new Color(0, 0, 0, fadeSpeed * Time.deltaTime);
                yield return null;
            }
        else
            while (vignette.color.a < 1)
            {
                vignette.color += new Color(0, 0, 0, fadeSpeed * Time.deltaTime);
                yield return null;
            }

        ExecutingCommand = false;
    }

    public IEnumerator FadeText(bool FadeIn)
    {
        ExecutingCommand = true;

        if (FadeIn)
            while (text.color.a < 1)
            {
                text.color += new Color(0, 0, 0, fadeSpeed * Time.deltaTime);
                yield return null;
            }
        else
            while (text.color.a > 0)
            {
                text.color -= new Color(0, 0, 0, fadeSpeed * Time.deltaTime);
                yield return null;
            }

        ExecutingCommand = false;
    }

    public IEnumerator MoveTowardsPosition(Vector3 position, float maxSpeed, bool accelerate = false)
    {
        ExecutingCommand = true;

        float speed = maxSpeed;
        float startDistance = Vector3.Distance(transform.position, position);

        while (transform.position != position)
        {
            transform.position = Vector3.MoveTowards(transform.position, position, speed * Time.deltaTime);

            if (accelerate)
            {
                float currentDistance = Vector3.Distance(transform.position, position);

                if(currentDistance > startDistance / 2)
                    speed = maxSpeed - maxSpeed * (currentDistance / startDistance) + (maxSpeed / 4);

                else
                    speed = maxSpeed * (currentDistance / startDistance) + (maxSpeed / 4);
            }

            yield return null;
        }

        ExecutingCommand = false;
    }
}
