using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class SixFPText : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    [SerializeField] float fadeInSpeed;
    [SerializeField] float fadeOutSpeed;
    [SerializeField] Color startColor;
    [SerializeField] Sprite[] sprites;
    [SerializeField] float timeBeforeFadeOut = 0.5f; 
    [SerializeField] float varianceCheckIn = 0.01f, varianceCheckOut = 0.1f; 

    SpriteRenderer myRenderer; 
    Color currentColor, endColor; 
    bool fadeIn = true;
    float timer; 

    public void StartText(int score)
    {
        myRenderer = GetComponent<SpriteRenderer>(); 
        currentColor = Color.clear;
        endColor = Color.clear; 

        switch (score)
        {
            //full success
            case 2:
                myRenderer.sprite = sprites[0];
                break;
            //half success
            case 1:
                myRenderer.sprite = sprites[1];
                break;
            //no success
            default:
                myRenderer.sprite = sprites[2];
                break;
        }

    }

    private void Update()
    {
        FadeText(); 

        transform.position += new Vector3(0, moveSpeed * Time.deltaTime, 0);
    }

    void FadeText()
    {
        if (fadeIn)
        {
            if ((startColor.a - currentColor.a) > varianceCheckIn)
            {
                currentColor = Color.Lerp(currentColor, startColor, fadeInSpeed * Time.deltaTime);
                myRenderer.color = currentColor;
            }
            else
            {
                fadeIn = false;
                myRenderer.color = startColor; 
            }
        }
        else
        {
            if (timer < timeBeforeFadeOut)
            {
                timer += Time.deltaTime; 
            }
            else
            {
                if (currentColor.a > varianceCheckOut)
                {
                    currentColor = Color.Lerp(currentColor, endColor, fadeOutSpeed * Time.deltaTime);
                    myRenderer.color = currentColor;
                }
                else
                {
                    Destroy(gameObject);
                }
            }
        }
    }
}
