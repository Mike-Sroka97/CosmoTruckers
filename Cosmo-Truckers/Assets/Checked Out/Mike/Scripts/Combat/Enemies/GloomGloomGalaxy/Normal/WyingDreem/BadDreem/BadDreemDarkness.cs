using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BadDreemDarkness : MonoBehaviour
{
    [SerializeField] float waitBeforeShrinking;
    [SerializeField] float shrinkTime = 2f;
    [SerializeField] Vector3 endScale = new Vector3(1, 1, 1);
    [SerializeField] SpriteRenderer[] lights;
    float[] lightFinalAlphas = new float[2]; 
    float[] currentLightAlphas = new float[2]; 
    Vector3 startScale;

    float currentScale; 
    float waitTimer;
    bool startWaiting = false;
    float shrinkTimer;

    private void Start()
    {
        lightFinalAlphas[0] = lights[0].color.a;
        lightFinalAlphas[1] = lights[1].color.a;
        lights[0].color = new Color(lights[0].color.r, lights[0].color.g, lights[0].color.b, 0f);
        lights[1].color = new Color(lights[1].color.r, lights[1].color.g, lights[1].color.b, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        if (startWaiting)
        {
            if (waitTimer < waitBeforeShrinking)
            {
                waitTimer += Time.deltaTime;
            }
            else
            {
                Shrink(); 
            }
        }
    }

    void Shrink()
    {
        if (transform.localScale.x > 1.05f)
        {
            // Shrink the darkness
            shrinkTimer += Time.deltaTime;
            currentScale = Mathf.Lerp(startScale.x, endScale.x, shrinkTimer / shrinkTime);
            transform.localScale = new Vector3(currentScale, currentScale, 1f);

            // Fade in pixel lights
            currentLightAlphas[0] = Mathf.Lerp(0, lightFinalAlphas[0], shrinkTimer / shrinkTime);
            currentLightAlphas[1] = Mathf.Lerp(0, lightFinalAlphas[1], shrinkTimer / shrinkTime);

            lights[0].color = new Color(lights[0].color.r, lights[0].color.g, lights[0].color.b, currentLightAlphas[0]); 
            lights[1].color = new Color(lights[1].color.r, lights[1].color.g, lights[1].color.b, currentLightAlphas[1]); 
        }
        else
        {
            startWaiting = false;
            transform.localScale = endScale;

            lights[0].color = new Color(lights[0].color.r, lights[0].color.g, lights[0].color.b, lightFinalAlphas[0]);
            lights[1].color = new Color(lights[1].color.r, lights[1].color.g, lights[1].color.b, lightFinalAlphas[1]);
        }
    }

    public void StartWaiting()
    {
        startScale = transform.localScale;
        startWaiting = true;
    }


}
