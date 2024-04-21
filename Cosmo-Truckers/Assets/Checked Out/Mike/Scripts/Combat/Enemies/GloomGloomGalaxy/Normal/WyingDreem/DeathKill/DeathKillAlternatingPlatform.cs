using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathKillAlternatingPlatform : MonoBehaviour
{
    [SerializeField] bool startsOn;
    [SerializeField] float timeToSwitch;

    Color on = new Color(1, 1, 1, 1);
    Color off = new Color(1, 1, 1, 0.5f);

    Collider2D myCollider;
    SpriteRenderer myRenderer;
    float currentTime = 0;
    bool isOn;

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
    }

    private void Toggle()
    {
        if (isOn)
        {
            isOn = false;
            myRenderer.color = off;
            myCollider.enabled = false;
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
