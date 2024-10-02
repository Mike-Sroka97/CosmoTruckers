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

    void Start()
    {
        mySpriteRenderer = GetComponent<SpriteRenderer>();
        myImage = GetComponent<Image>();

        if (mySpriteRenderer)
            StartCoroutine(ChangeSprites());
        if (myImage)
            StartCoroutine(ChangeImages());
    }
    
    protected IEnumerator ChangeSprites()
    {
        if (currentSprite >= sprites.Length)
            currentSprite = 0;

        mySpriteRenderer.sprite = sprites[currentSprite];
        yield return new WaitForSeconds(timeBetweenEachSprite);
        currentSprite++;

        StartCoroutine(ChangeSprites());
    }

    protected IEnumerator ChangeImages()
    {
        if (currentSprite >= sprites.Length)
            currentSprite = 0;

        myImage.sprite = sprites[currentSprite];
        yield return new WaitForSeconds(timeBetweenEachSprite);
        currentSprite++;

        StartCoroutine(ChangeImages());
    }

    public void StopAnimation()
    {
        StopAllCoroutines(); 
    }
}
