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
    [SerializeField] Material hurtMaterial; 
    [SerializeField] Material happyMaterial; 

    bool hurt = false;
    bool happy = false; 
    float timer = 0f;
    float frameTimer = 0f;
    int currentFrame = -1; 
    private float timeBeforeSwapping = 0f;

    public void SwitchToHurtAnimation()
    {
        StopAllCoroutines();
        timeBeforeSwapping = timeBetweenHurtSprites;
        hurt = true;
        happy = false;
        ResetValues();
    }

    public void SwitchToHappyAnimation()
    {
        StopAllCoroutines();
        timeBeforeSwapping = timeBetweenHappySprites;
        happy = true;
        hurt = false;
        ResetValues();
    }

    /// <summary>
    /// Pass in a unique amount of time to the animation before swapping back to default
    /// </summary>
    /// <param name="timeBeforeSwapping"></param>
    /// <param name="isHurt"></param>
    public void StartAnimationWithUniqueTime(float timeBeforeSwapping, bool isHurt = true)
    {
        ResetValues();
        this.timeBeforeSwapping = timeBeforeSwapping;

        if (isHurt)
        {
            if (hurtMaterial != null)
                mySpriteRenderer.material = hurtMaterial;
            
            StopAllCoroutines();
            hurt = true;
            happy = false;
            ResetValues();
        }
        else
        {
            if (happyMaterial != null)
                mySpriteRenderer.material = happyMaterial;

            StopAllCoroutines();
            hurt = false;
            happy = true;
            ResetValues();
        }
    }

    private void Update()
    {
        if (hurt)
            ChangeAnimation(hurtSprites, timeBeforeSwapping, hurtTimeBeforeSwapping, ref hurt);

        if (happy)
            ChangeAnimation(happySprites, timeBeforeSwapping, happyTimeBeforeSwapping, ref happy);
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
        if (collision.CompareTag("PlayerAttack") && !hurt && playerHitHurts)
        {
            SwitchToHurtAnimation(); 
        }
    }
}
