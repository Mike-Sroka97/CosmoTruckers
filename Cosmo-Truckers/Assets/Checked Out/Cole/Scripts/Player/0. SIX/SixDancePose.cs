using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SixDancePose : MonoBehaviour
{
    [SerializeField] float finalScale;
    [SerializeField] float speedToScale = 1f;

    SpriteRenderer myRenderer;
    bool startScaling = false;
    Vector3 currentScale, endScale;
    Color currentColor, endColor;

    public void StartDancePose(Sprite sprite)
    {
        //Debug.Log("Start Dance Pose"); 
        myRenderer = GetComponent<SpriteRenderer>();
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
