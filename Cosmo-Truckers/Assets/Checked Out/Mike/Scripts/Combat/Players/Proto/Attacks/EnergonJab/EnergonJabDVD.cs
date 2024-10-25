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
    AdvancedFrameAnimation myAnimation; 

    private void Start()
    {
        minigame = GetComponentInParent<EnergonJab>();
        myRenderer = GetComponent<SpriteRenderer>();
        myAnimation= GetComponent<AdvancedFrameAnimation>();
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
            myRenderer.material = startMaterial;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "PlayerAttack" && !trackTime)
        {
            trackTime = true;
            minigame.Score++;
            minigame.CheckSuccess();
            // Start the hit animation and then it will automatically swap back
            myAnimation.StartAnimationWithUniqueTime(disableTime);
            myRenderer.material = disabledMaterial;
        }
    }
}
