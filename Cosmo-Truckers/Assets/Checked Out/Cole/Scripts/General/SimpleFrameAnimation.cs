using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This will go through the sprites at all the same speeds
public class SimpleFrameAnimation : MonoBehaviour
{
    [SerializeField] Sprite[] sprites;
    [SerializeField] float timeBetweenEachSprite = 0.5f;

    SpriteRenderer mySpriteRenderer; 
    int currentSprite = 0; 

    void Start()
    {
        mySpriteRenderer = GetComponent<SpriteRenderer>(); 

        if (mySpriteRenderer != null)
        {
            StartCoroutine(ChangeSprites());
        }
    }
    
    IEnumerator ChangeSprites()
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
