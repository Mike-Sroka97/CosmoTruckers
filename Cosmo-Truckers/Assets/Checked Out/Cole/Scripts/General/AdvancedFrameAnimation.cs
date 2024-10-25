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
    [SerializeField] bool playerHitHurts = false; 

    bool hurt = false;
    bool happy = false; 
    float timer = 0f;
    float frameTimer = 0f;
    int currentFrame = -1; 

    public void SwitchToHurtAnimation()
    {
        StopAllCoroutines();
        hurt = true;
        happy = false;
        ResetValues();
    }

    /// <summary>
    /// Pass in a unique amount of time to the animation before swapping back to default
    /// </summary>
    /// <param name="timeBeforeSwapping"></param>
    /// <param name="isHurt"></param>
    public void StartAnimationWithUniqueTime(float timeBeforeSwapping, bool isHurt = true)
    {
        if (isHurt)
        {
            ChangeAnimation(hurtSprites, timeBetweenHurtSprites, timeBeforeSwapping, ref hurt);
        }
        else
        {
            ChangeAnimation(happySprites, timeBetweenHappySprites, timeBeforeSwapping, ref happy);
        }
    }

    public void SwitchToHappyAnimation()
    {
        StopAllCoroutines();
        happy = true;
        hurt = false;
        ResetValues(); 
    }

    private void Update()
    {
        if (hurt)
        {
            ChangeAnimation(hurtSprites, timeBetweenHurtSprites, hurtTimeBeforeSwapping, ref hurt); 
        }

        if (happy)
        {
            ChangeAnimation(happySprites, timeBetweenHappySprites, happyTimeBeforeSwapping, ref happy);
        }
    }

    void ChangeAnimation(Sprite[] sprites, float timeBetweenSprites, float timeBeforeSwapping, ref bool boolToSet)
    {
        if (currentFrame == -1)
        {
            currentFrame = 0; 
            mySpriteRenderer.sprite = sprites[currentFrame];
        }

        timer += Time.deltaTime;
        frameTimer += Time.deltaTime;

        if (frameTimer >= timeBetweenSprites)
        {
            currentFrame++;

            if (currentFrame >= sprites.Length)
                currentFrame = 0;

            mySpriteRenderer.sprite = sprites[currentFrame];
            frameTimer = 0f;
        }

        if (timer > timeBeforeSwapping)
        {
            boolToSet = false;
            StartCoroutine(ChangeSprites());
        }
    }

    void ResetValues()
    {
        currentFrame = -1;
        timer = 0;
        frameTimer = 0;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerAttack") && !hurt)
        {
            SwitchToHurtAnimation(); 
        }
    }
}
