using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergonJabDVD : DVDlogoMovement
{
    [SerializeField] float disableTime;
    [SerializeField] float alpha;

    float currentTime = 0;
    bool trackTime = false;
    SpriteRenderer[] myRenderers;

    private void Start()
    {
        myRenderers = GetComponentsInChildren<SpriteRenderer>();
        Initialize();
        RandomStartVelocity();
    }

    private void Update()
    {
        Movement();
        TrackTime();
    }
    
    private void TrackTime()
    {
        if (!trackTime)
            return;

        currentTime += Time.deltaTime;

        if(currentTime >= disableTime)
        {
            currentTime = 0;
            trackTime = false;
            foreach (SpriteRenderer sprite in myRenderers)
            {
                sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, 1);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "PlayerAttack")
        {
            trackTime = true;
            foreach(SpriteRenderer sprite in myRenderers)
            {
                sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, alpha);
            }
        }
    }
}
