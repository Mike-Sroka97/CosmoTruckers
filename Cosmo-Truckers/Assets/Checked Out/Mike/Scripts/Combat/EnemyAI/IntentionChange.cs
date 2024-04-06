using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntentionChange : MonoBehaviour
{
    [SerializeField] float destroyTime;
    [SerializeField] float pulseSpeed;
    [SerializeField] float maxScale;
    float minScale;
    [SerializeField] float rotateTime;
    [SerializeField] float minRotation;
    [SerializeField] float maxRotation;
    [SerializeField] float fadeSpeed;

    Transform anger;
    SpriteRenderer[] myRenderers;

    private void Start()
    {
        //Initialization
        minScale = Vector3.one.x;
        anger = transform.Find("anger");
        myRenderers = GetComponentsInChildren<SpriteRenderer>();
        anger.parent = transform.parent;

        //Coroutines
        StartCoroutine(DestroyTimer());
        StartCoroutine(Pulse());
        StartCoroutine(RotateAnger());
        StartCoroutine(Fade(true));
    }

    IEnumerator Fade(bool fadeIn)
    {
        if(fadeIn)
        {
            while(myRenderers[0].color.a < 1)
            {
                foreach (SpriteRenderer renderer in myRenderers)
                    renderer.color += new Color(0, 0, 0, fadeSpeed * Time.deltaTime);
                yield return null;
            }
        }
        else
        {
            while (myRenderers[0].color.a > 0)
            {
                foreach (SpriteRenderer renderer in myRenderers)
                    renderer.color -= new Color(0, 0, 0, fadeSpeed * Time.deltaTime);
                yield return null;
            }

            //Cleanup
            Destroy(anger.gameObject);
            Destroy(gameObject);
        }
    }

    //Sets a timer to fucking demolish this thing
    IEnumerator DestroyTimer()
    {
        yield return new WaitForSeconds(destroyTime);
        StartCoroutine(Fade(false));
    }

    //Recursively pulses brain in and out
    IEnumerator Pulse()
    {
        while(transform.localScale.x < maxScale)
        {
            transform.localScale += new Vector3(pulseSpeed * Time.deltaTime, pulseSpeed * Time.deltaTime, 0);
            yield return null;
        }
        while(transform.localScale.x > minScale)
        {
            transform.localScale -= new Vector3(pulseSpeed * Time.deltaTime, pulseSpeed * Time.deltaTime, 0);
            yield return null;
        }

        //again! :slightsmile:
        StartCoroutine(Pulse());
    }

    //Recursively rotates anger at random angles from a min-max range
    IEnumerator RotateAnger()
    {
        yield return new WaitForSeconds(rotateTime);

        float random = Random.Range(minRotation, maxRotation);

        //rotates back and forth
        minRotation = -minRotation;
        maxRotation = -maxRotation;

        anger.Rotate(new Vector3(0, 0, random));

        //again! :slightsmile:
        StartCoroutine(RotateAnger());
    }
}
