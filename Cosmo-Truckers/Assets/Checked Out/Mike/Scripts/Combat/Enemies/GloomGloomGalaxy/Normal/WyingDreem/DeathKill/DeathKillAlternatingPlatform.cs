using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathKillAlternatingPlatform : MonoBehaviour
{
    [SerializeField] bool startsOn;
    [SerializeField] float timeToSwitch;
    [SerializeField] float flashWaitTime = 0.25f;

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
        on = new Color(myRenderer.color.r, myRenderer.color.g, myRenderer.color.b, 1f); 
        off = new Color(myRenderer.color.r, myRenderer.color.g, myRenderer.color.b, 0.25f); 

        isOn = startsOn;

        Toggle();
    }

    private void Update()
    {
        TrackTime();
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
