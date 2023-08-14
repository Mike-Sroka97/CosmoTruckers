using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraggyCoatingPlatform : MonoBehaviour
{
    [SerializeField] bool startOnGoodSide;
    [SerializeField] GameObject goodSide;
    [SerializeField] GameObject badSide;

    [SerializeField] float flipDelay;
    [SerializeField] float colorChangeDelay;
    [SerializeField] float safetyDelay;
    [SerializeField] Color aboutToFlipcolor;

    Color startingGoodColor;
    SpriteRenderer goodSideRenderer;
    float currentTime = 0;
    bool onGoodSide;

    private void Start()
    {
        goodSideRenderer = goodSide.GetComponentInChildren<SpriteRenderer>();
        startingGoodColor = goodSideRenderer.color;

        if(startOnGoodSide)
        {
            goodSide.SetActive(true);
            badSide.SetActive(false);
            onGoodSide = true;
        }
        else
        {
            goodSide.SetActive(false);
            badSide.SetActive(true);
            onGoodSide = false;
        }
    }

    private void Update()
    {
        TrackTime();
    }

    private void TrackTime()
    {
        currentTime += Time.deltaTime;

        if(onGoodSide)
        {
            if(currentTime >= colorChangeDelay)
            {
                goodSideRenderer.color = aboutToFlipcolor;
            }

            if (currentTime >= flipDelay)
            {
                currentTime = 0;
                onGoodSide = false;
            }
        }
        else
        {
            if(currentTime >= safetyDelay)
            {
                goodSide.SetActive(false);
                badSide.SetActive(true);
                goodSideRenderer.color = startingGoodColor;
            }

            if(currentTime >= flipDelay)
            {
                currentTime = 0;
                onGoodSide = true;
                goodSide.SetActive(true);
                badSide.SetActive(false);
            }
        }
    }
}
