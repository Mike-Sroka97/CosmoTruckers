using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergonJabDVD : DVDlogoMovement
{
    [SerializeField] Material disabledMaterial;
    [SerializeField] Material enabledMaterial;

    SpriteRenderer myRenderer;
    EnergonJab minigame;
    AdvancedFrameAnimation myAnimation;
    private bool active;

    private void Awake()
    {
        minigame = GetComponentInParent<EnergonJab>();
        myRenderer = GetComponent<SpriteRenderer>();
        myAnimation= GetComponent<AdvancedFrameAnimation>();
        Initialize();
        RandomStartVelocity();
    }

    public void ActivateMe()
    {
        myRenderer.material = enabledMaterial;
        myAnimation.SwitchToHappyAnimation();
        active = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "PlayerAttack" && active)
        {
            active = false;
            minigame.Score++;
            minigame.CheckSuccess();
            // Start the hit animation and then it will automatically swap back
            myAnimation.SwitchToHurtAnimation();
            myRenderer.material = disabledMaterial;
            minigame.SetNextBall();
        }
    }
}
