using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdvancedFrameAnimation : SimpleFrameAnimation
{
    [Header("Advanced Frame Variables")]
    [SerializeField] Sprite[] hurtSprites;
    [SerializeField] float timeBetweenHurtSprites = 0.25f; 
    [SerializeField] float hurtTimeBeforeSwapping = 1f;
    [SerializeField] Sprite[] happySprites;
    [SerializeField] float timeBetweenHappySprites = 0.25f;
    [SerializeField] float happyTimeBeforeSwapping = 1f; 

    bool startHurt = false;
    bool startHappy = false; 
    float timer = 0f;
    float frameTimer = 0f;
    int currentFrame = 0; 

    public void SwitchToHurtAnimation()
    {
        StopAllCoroutines();
        startHurt = true;
        startHappy = false;
        ResetValues();
    }

    public void SwitchToHappyAnimation()
    {
        StopAllCoroutines();
        startHappy = true;
        startHurt = false;
        ResetValues(); 
    }

    private void Update()
    {
        if (startHurt)
        {
            HurtAnimation(); 
        }

        if (startHappy)
        {
            HappyAnimation(); 
        }
    }

    void HurtAnimation()
    {
        timer += Time.deltaTime;
        frameTimer += Time.deltaTime;

        if (frameTimer >= timeBetweenHurtSprites)
        {
            if (currentFrame >= hurtSprites.Length)
                currentFrame = 0;

            mySpriteRenderer.sprite = hurtSprites[currentFrame];
            currentFrame++;
            frameTimer = 0f;
        }

        if (timer > hurtTimeBeforeSwapping)
        {
            startHurt = false;
            timer = 0;
            StartCoroutine(ChangeSprites());
        }
    }

    void HappyAnimation()
    {
        timer += Time.deltaTime;
        frameTimer += Time.deltaTime;

        if (frameTimer >= timeBetweenHappySprites)
        {
            if (currentFrame >= happySprites.Length)
                currentFrame = 0;

            mySpriteRenderer.sprite = happySprites[currentFrame];
            currentFrame++;
            frameTimer = 0f;
        }

        if (timer > happyTimeBeforeSwapping)
        {
            startHappy = false;
            timer = 0;
            StartCoroutine(ChangeSprites());
        }
    }

    void ResetValues()
    {
        currentFrame = 0;
        timer = 0;
        frameTimer = 0;
    }
}
