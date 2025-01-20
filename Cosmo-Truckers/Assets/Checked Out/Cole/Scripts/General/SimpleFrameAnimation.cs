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

    void Start()
    {
        mySpriteRenderer = GetComponent<SpriteRenderer>();
        defaultMaterial = mySpriteRenderer.material;
        myImage = GetComponent<Image>();

        if (mySpriteRenderer)
            StartCoroutine(ChangeSprites());
        if (myImage)
            StartCoroutine(ChangeImages());
    }
    
    /// <summary>
    /// Default sprite swapping
    /// </summary>
    /// <returns></returns>
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
