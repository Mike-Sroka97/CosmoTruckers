using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergonJabDVD : DVDlogoMovement
{
    [SerializeField] float disableTime;
    [SerializeField] float alpha;
    [SerializeField] Material disabledMaterial; 

    float currentTime = 0;
    bool trackTime = false;
    SpriteRenderer myRenderer;
    Material startMaterial;  
    EnergonJab minigame;

    private void Start()
    {
        minigame = GetComponentInParent<EnergonJab>();
        myRenderer = GetComponent<SpriteRenderer>();
        startMaterial = myRenderer.material;
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
            myRenderer.color = new Color(myRenderer.color.r, myRenderer.color.g, myRenderer.color.b, 1);
            myRenderer.material = startMaterial;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "PlayerAttack" && !trackTime)
        {
            trackTime = true;
            minigame.Score++;
            myRenderer.color = new Color(myRenderer.color.r, myRenderer.color.g, myRenderer.color.b, alpha);
            myRenderer.material = disabledMaterial;
        }
    }
}
