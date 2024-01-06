using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This will go through the sprites at all the same speeds
public class SimpleFrameAnimation : MonoBehaviour
{
    [Header("Simple Frame Variables")]
    [SerializeField] protected Sprite[] sprites;
    [SerializeField] protected float timeBetweenEachSprite = 0.5f;

    protected SpriteRenderer mySpriteRenderer; 
    protected int currentSprite = 0; 

    void Start()
    {
        mySpriteRenderer = GetComponent<SpriteRenderer>(); 

        if (mySpriteRenderer != null)
        {
            StartCoroutine(ChangeSprites());
        }
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

    public void StopAnimation()
    {
        StopAllCoroutines(); 
    }
}
