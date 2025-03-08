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

    private Color defaultColor;
    private Color deactivatedColor = new Color(0.85f, 0.85f, 0.85f, 0.5f);

    private void Awake()
    {
        minigame = GetComponentInParent<EnergonJab>();
        myRenderer = GetComponent<SpriteRenderer>();
        defaultColor = myRenderer.color;
        myRenderer.color = deactivatedColor; 

        myAnimation= GetComponent<AdvancedFrameAnimation>();
        Initialize();
        RandomStartVelocity();
    }

    public void ActivateMe()
    {
        myRenderer.material = enabledMaterial;
        myRenderer.color = defaultColor; 
        active = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("PlayerAttack") && active)
        {
            active = false;
            minigame.Score++;
            minigame.CheckSuccess();
            // Start the hit animation and then it will automatically swap back
            myAnimation.SwitchToEmotion();
            myRenderer.material = disabledMaterial;
            myRenderer.color = deactivatedColor;
            minigame.SetNextBall();
        }
    }
}
