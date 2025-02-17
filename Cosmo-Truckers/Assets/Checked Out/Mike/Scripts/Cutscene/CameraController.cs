using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CameraController : MonoBehaviour
{
    [SerializeField] float fadeSpeed;
    [SerializeField] float textFadeSpeed;
    [SerializeField] float panSpeed;
    float shakeMinDistance = 0.05f;

    [HideInInspector] public int CommandsExecuting;
    [HideInInspector] public bool CurrentlyExecutingCommand => CommandsExecuting > 0;

    Image vignette;
    TextMeshProUGUI text;
    Camera myCamera;

    //OW stuff
    float negXclamp;
    float posXclamp;
    float negYclamp;
    float posYclamp;
    Pixelation myPixelator;

    [HideInInspector] public Transform Leader;
    public string LastNode;
    [HideInInspector] public static CameraController Instance;

    private void Awake()
    {
        CameraController[] objs = FindObjectsOfType<CameraController>();

        if (objs.Length > 1)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);

        if(!Instance)
            Instance = this;

        vignette = transform.Find("CameraVignette").GetComponent<Image>();
        text = GetComponentInChildren<TextMeshProUGUI>();
        myCamera = GetComponent<Camera>();
        myPixelator = GetComponent<Pixelation>();
        
        myPixelator.Unpixelate();
    }

    private void Update()
    {
        OwPan();
    }

    private void OwPan()
    {
        if(Leader)
        {
            float xVal = Leader.position.x;
            float yVal = Leader.position.y;

            if (Leader.position.x < negXclamp)
                xVal = negXclamp;
            else if (Leader.position.x > posXclamp)
                xVal = posXclamp;
            if (Leader.position.y < negYclamp)
                yVal = negYclamp;
            else if (Leader.position.y > posYclamp)
                yVal = posYclamp;

            transform.position = Vector3.MoveTowards(transform.position, new Vector3(xVal, yVal, transform.position.z), panSpeed * Time.deltaTime);
        }
    }

    public void InitializeOwCamera(float minX, float maxX, float minY, float maxY, Transform leaderTransform)
    {
        negXclamp = minX;
        posXclamp = maxX;
        negYclamp = minY;
        posYclamp = maxY;

        Leader = leaderTransform;
    }

    public IEnumerator HubSceneLoad(string sceneToLoad)
    {
        StartCoroutine(FadeVignette(false));

        yield return null;

        while (CommandsExecuting > 0)
            yield return null;

        SceneManager.LoadScene(sceneToLoad);
    }

    public IEnumerator OwCharacterActionSelect(string sceneToLoad, UnityEvent eventToInvoke)
    {
        CommandsExecuting++;

        myPixelator.Pixelate();

        while (CommandsExecuting > 0)
            yield return null;

        StartCoroutine(FadeVignette(false));

        yield return null;

        while (CommandsExecuting > 0)
            yield return null;

        eventToInvoke?.Invoke();

        SceneManager.LoadScene(sceneToLoad);
    }

    public IEnumerator DungeonEnd(string sceneToLoad)
    {
        CommandsExecuting++;

        yield return new WaitForSeconds(2f);

        CommandsExecuting--;
        SceneManager.LoadScene(sceneToLoad);
    }

    public IEnumerator FadeVignette(bool FadeIn)
    {
        CommandsExecuting++;

        int vignetteChildCount = vignette.transform.childCount; 

        if (FadeIn)
            while (vignette.color.a > 0)
            {
                vignette.color -= new Color(0, 0, 0, fadeSpeed * Time.deltaTime);
                if (vignetteChildCount > 0)
                    vignette.GetComponentsInChildren<Image>()[1].color -= new Color(0, 0, 0, fadeSpeed * Time.deltaTime);
                yield return null;
            }
        else
            while (vignette.color.a < 1)
            {
                vignette.color += new Color(0, 0, 0, fadeSpeed * Time.deltaTime);
                if (vignetteChildCount > 0)
                    vignette.GetComponentsInChildren<Image>()[1].color += new Color(0, 0, 0, fadeSpeed * Time.deltaTime);
                yield return null;

                myPixelator.Unpixelate();
            }

        if (Leader)
        {
            if (FindObjectOfType<Overworld>())
            { 
                FindObjectOfType<Overworld>().CameraFadeFinished();
                myPixelator.Unpixelate();
            }
            else
                FindObjectOfType<DungeonController>().CameraFadeFinished();
        }


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

    public void NormalizePositionRotation()
    {
        transform.position = new Vector3(0, 0, -10);
        transform.rotation = Quaternion.identity;
    }
}
