using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathKillAlternatingPlatform : MonoBehaviour
{
    [SerializeField] bool startsOn;
    [SerializeField] float timeToSwitch;
    [SerializeField] float flashWaitTime = 0.25f;
    [SerializeField] Color flash = new Color(0.8f, 0.8f, 0.8f, 1);

    Color on = new Color(1, 1, 1, 1);
    Color off = new Color(1, 1, 1, 0.25f);

    Collider2D myCollider;
    SpriteRenderer myRenderer;
    float currentTime = 0;
    bool isOn;

    bool flashOn; 
    float flashTime = 0;

    private void Start()
    {
        myCollider = GetComponent<Collider2D>();
        myRenderer = GetComponent<SpriteRenderer>();
        isOn = startsOn;

        Toggle();
    }

    private void Update()
    {
        TrackTime();

        /*
        if (isOn)
        {
            flashTime += Time.deltaTime; 
            if (flashTime > flashWaitTime)
            {
                if (!flashOn)
                {
                    myRenderer.color = flash;
                    flashOn = true; 
                }
                else
                {
                    myRenderer.color = on;
                    flashOn = false;
                }

                flashTime = 0;
            }

        }
        */ 
    }

    private void Toggle()
    {
        if (isOn)
        {
            isOn = false;
            myRenderer.color = off;
            myCollider.enabled = false;
            flashTime = 0; 
            flashOn = false;
        }
        else
        {
            isOn = true;
            myRenderer.color = on;
            myCollider.enabled = true;
        }
    }

    private void TrackTime()
    {
        currentTime += Time.deltaTime;

        if(currentTime >= timeToSwitch)
        {
            currentTime = 0;
            Toggle();
        }
    }
}
