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

    private bool hurt = false;
    private bool happy = false; 

    /// <summary>
    /// Switch to a new emotion. <br></br>
    /// By default, this is set up for hurt. 
    /// </summary>
    /// <param name="isHurt">Switches to hurt by default, otherwise will switch to happy animation</param>
    /// <param name="loopAnimation">Does not loop by default. Setting this to true will loop it until ReturnToDefault() is called</param>
    /// <param name="timeBeforeSwapping">Time before swapping back to default. If left alone, it will use a default time.</param>
    public void SwitchToEmotion(bool isHurt = true, bool loopAnimation = false, float timeBeforeSwapping = 0f)
    {
        ResetValues();
        hurt = isHurt;
        happy = !isHurt; 

        if (hurt)
        {
            animationTime = timeBetweenHurtSprites; 
            animationSprites = hurtSprites; 
        }
        else if (happy)
        {
            animationTime = timeBetweenHappySprites;
            animationSprites = happySprites;
        }

        if (!loopAnimation)
            StartCoroutine(DelayedReturnToDefault(timeBeforeSwapping)); 
    }

    /// <summary>
    /// Wait for a short period before returning to the default animation
    /// </summary>
    /// <returns></returns>
    private IEnumerator DelayedReturnToDefault(float timeBeforeSwapping = 0f)
    {
        // Either use unique time before swapping to default, or just use the default for the emotion
        float delayTime = timeBeforeSwapping; 

        if (happy && timeBeforeSwapping == 0f)
            delayTime = happyTimeBeforeSwapping;

        if (hurt && timeBeforeSwapping == 0f)
            delayTime = hurtTimeBeforeSwapping; 

        yield return new WaitForSeconds(delayTime);

        ReturnToDefault();
    }

    /// <summary>
    /// Returns to the default animation
    /// </summary>
    public void ReturnToDefault()
    {
        ResetValues();
        animationTime = timeBetweenEachSprite;
        animationSprites = sprites; 
        hurt = false;
        happy = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerAttack") && !hurt && playerHitHurts)
        {
            SwitchToEmotion(); 
        }
    }
}
