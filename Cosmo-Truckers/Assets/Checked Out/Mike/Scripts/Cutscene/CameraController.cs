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
    Camera myCamera;

    private void Start()
    {
        vignette = GetComponentInChildren<SpriteRenderer>();
        text = GetComponentInChildren<TextMeshProUGUI>();
        myCamera = GetComponent<Camera>();
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

    public IEnumerator Shake(float duration, float shakeSpeed, float shakeOffset)
    {
        Vector3 shakeOriginalPosition = transform.position;
        ExecutingCommand = true;

        while(duration > 0)
        {
            transform.position = new Vector3(Mathf.Sin(Time.time * shakeSpeed) * shakeOffset, (Mathf.Sin(Time.time * shakeSpeed) * shakeOffset), 0) + shakeOriginalPosition;
            duration -= Time.deltaTime;
            yield return null;
        }

        while(transform.position != shakeOriginalPosition)
        {
            Vector3.MoveTowards(transform.position, shakeOriginalPosition, shakeSpeed * Time.deltaTime);
            yield return null;
        }

        ExecutingCommand = false;
    }

    public IEnumerator Zoom(bool zoomIn, float speed, float size)
    {
        ExecutingCommand = true;

        if (zoomIn)
        {
            while (myCamera.orthographicSize > size)
            {
                myCamera.orthographicSize -= speed * Time.deltaTime;
                yield return null;
            }
        }

        else
        {
            while (myCamera.orthographicSize < size)
            {
                myCamera.orthographicSize += speed * Time.deltaTime;
                yield return null;
            }
        }

        myCamera.orthographicSize = size;

        ExecutingCommand = false;
    }
}
