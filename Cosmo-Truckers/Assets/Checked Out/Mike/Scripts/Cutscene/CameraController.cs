using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraController : MonoBehaviour
{
    [SerializeField] float fadeSpeed;
    [SerializeField] float textFadeSpeed;
    float shakeMinDistance = 0.05f; 

    [HideInInspector] public int CommandsExecuting { get; private set; }

    SpriteRenderer vignette;
    TextMeshProUGUI text;
    Camera myCamera;

    //OW stuff
    float negXclamp;
    float posXclamp;
    float negYclamp;
    float posYclamp;
    Transform leader;

    private void Awake()
    {
        vignette = transform.Find("CameraVignette").GetComponent<SpriteRenderer>();
        text = GetComponentInChildren<TextMeshProUGUI>();
        myCamera = GetComponent<Camera>();
    }

    private void Update()
    {
        OwPan();
    }

    private void OwPan()
    {
        if(leader)
        {
            float xVal = leader.position.x;
            float yVal = leader.position.y;

            if (leader.position.x < negXclamp)
                xVal = negXclamp;
            else if (leader.position.x > posXclamp)
                xVal = posXclamp;
            if (leader.position.y < negYclamp)
                yVal = negYclamp;
            else if (leader.position.y > posYclamp)
                yVal = posYclamp;

            transform.position = new Vector3(xVal, yVal, transform.position.z);
        }
    }

    public void InitializeOwCamera(float minX, float maxX, float minY, float maxY, Transform leaderTransform)
    {
        negXclamp = minX;
        posXclamp = maxX;
        negYclamp = minY;
        posYclamp = maxY;

        leader = leaderTransform;
    }

    public IEnumerator OwCharacterActionSelect(string sceneToLoad)
    {
        CommandsExecuting++;

        yield return null;

        CommandsExecuting--;

        SceneManager.LoadScene(sceneToLoad);
    }

    public IEnumerator FadeVignette(bool FadeIn)
    {
        CommandsExecuting++; 

        if (FadeIn)
            while (vignette.color.a > 0)
            {
                vignette.color -= new Color(0, 0, 0, fadeSpeed * Time.deltaTime);
                vignette.GetComponentsInChildren<SpriteRenderer>()[1].color -= new Color(0, 0, 0, fadeSpeed * Time.deltaTime);
                yield return null;
            }
        else
            while (vignette.color.a < 1)
            {
                vignette.color += new Color(0, 0, 0, fadeSpeed * Time.deltaTime);
                vignette.GetComponentsInChildren<SpriteRenderer>()[1].color += new Color(0, 0, 0, fadeSpeed * Time.deltaTime);
                yield return null;
            }

        if (leader)
            leader.GetComponent<OverworldCharacter>().enabled = true;

        CommandsExecuting--;
    }

    public IEnumerator FadeText(bool FadeIn)
    {
        CommandsExecuting++;

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

        CommandsExecuting--; 
    }

    public IEnumerator MoveTowardsPosition(Vector3 position, float maxSpeed, bool accelerate = false)
    {
        CommandsExecuting++;

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

        CommandsExecuting--; 
    }

    public IEnumerator Shake(float duration, float shakeSpeed, float shakeOffset)
    {
        CommandsExecuting++;

        Vector3 shakeOriginalPosition = transform.position;
        float distance = 1000; 

        while (duration > 0)
        {
            transform.position = new Vector3(Mathf.Sin(Time.time * shakeSpeed) * shakeOffset, (Mathf.Sin(Time.time * shakeSpeed) * shakeOffset), 0) + shakeOriginalPosition;
            duration -= Time.deltaTime;
            yield return null;
        }

        while(transform.position != shakeOriginalPosition)
        {
            distance = Mathf.Abs(Vector3.Distance(transform.position, shakeOriginalPosition));

            if (distance <= shakeMinDistance)
                transform.position = shakeOriginalPosition;
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, shakeOriginalPosition, shakeSpeed * Time.deltaTime);
                yield return null;
            }
        }

        CommandsExecuting--; 
    }

    public IEnumerator Zoom(bool zoomIn, float speed, float size)
    {
        CommandsExecuting++;

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

        CommandsExecuting--; 
    }
}
