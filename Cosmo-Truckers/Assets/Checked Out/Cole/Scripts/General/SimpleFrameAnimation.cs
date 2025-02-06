using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//This will go through the sprites at all the same speeds
public class SimpleFrameAnimation : MonoBehaviour
{
    [Header("Simple Frame Variables")]
    [SerializeField] protected Sprite[] sprites;
    [SerializeField] protected float timeBetweenEachSprite = 0.5f;

    protected SpriteRenderer mySpriteRenderer;
    protected Image myImage;
    protected int currentSprite = 0;
    protected Material defaultMaterial;

    protected Sprite[] animationSprites;
    protected float animationTime;
    protected float timer = 0;

    private bool canPlayAnimation = true; 

    void Start()
    {
        mySpriteRenderer = GetComponent<SpriteRenderer>();
        
        if (mySpriteRenderer != null)
        {
            defaultMaterial = mySpriteRenderer.material;
        }
        else
        {
            myImage = GetComponent<Image>();
        }

        animationTime = timeBetweenEachSprite;
        animationSprites = sprites; 
    }

    private void Update()
    {
        if (canPlayAnimation)
        {
            timer += Time.deltaTime;

            if (mySpriteRenderer != null)
                Animate();
            else if (myImage != null)
                AnimateImage();
        }
    }

    protected virtual void Animate()
    {
        if (timer > animationTime)
        {
            mySpriteRenderer.sprite = animationSprites[currentSprite];
            currentSprite++;
            timer = 0;

            if (currentSprite >= animationSprites.Length)
                currentSprite = 0;
        }
    }

    protected virtual void AnimateImage()
    {
        if (timer > animationTime)
        {
            myImage.sprite = animationSprites[currentSprite];
            currentSprite++;
            timer = 0;

            if (currentSprite >= animationSprites.Length)
                currentSprite = 0;
        }
    }

    /// <summary>
    /// Zero out the animation values
    /// </summary>
    protected void ResetValues()
    {
        currentSprite = 0; 
        timer = 0;
    }

    public void StopAnimation()
    {
        canPlayAnimation = false;
    }

    public void StartAnimation()
    {
        canPlayAnimation = true;
    }
}
