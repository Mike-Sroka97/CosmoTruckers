using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnFadeObject : MonoBehaviour
{
    [SerializeField] float finalScale;
    [SerializeField] float speedToScale = 1f;

    SpriteRenderer myRenderer;
    bool startScaling = false;
    Vector3 currentScale, endScale;
    Color currentColor, endColor;

    /// <summary>
    /// Start Fading. Enter a renderer number if you want to get a child object 
    /// Getting a child object sprite renderer is for when sprites have an offset
    /// </summary>
    /// <param name="sprite"></param>
    /// <param name="renderer"></param>
    public void StartFading(Sprite sprite, int renderer = -1)
    {
        if (renderer != -1)
        {
            transform.GetComponent<SpriteRenderer>().enabled = false; 
            myRenderer = transform.GetChild(renderer).GetComponent<SpriteRenderer>();
            myRenderer.enabled = true; 
        }
        else
        {
            //Debug.Log("Start Fading"); 
            myRenderer = GetComponent<SpriteRenderer>();
        }

        myRenderer.sprite = sprite;
        currentScale = transform.localScale;
        endScale = new Vector3(finalScale, finalScale, finalScale);
        currentColor = myRenderer.color;
        endColor = new Color(currentColor.r, currentColor.g, currentColor.b, 0f);

        startScaling = true;
    }

    private void Update()
    {
        if (startScaling)
        {
            if (currentScale.x < finalScale)
            {
                currentScale = Vector3.Lerp(currentScale, endScale, speedToScale * Time.deltaTime);
                currentColor = Color.Lerp(currentColor, endColor, speedToScale * Time.deltaTime);

                transform.localScale = currentScale;
                myRenderer.color = currentColor;
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}
