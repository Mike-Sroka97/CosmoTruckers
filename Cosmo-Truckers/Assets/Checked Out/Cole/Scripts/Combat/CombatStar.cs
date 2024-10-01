using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class CombatStar : MonoBehaviour
{
    [SerializeField] AnimationClip animationToPlay;
    Animator animator;

    public float SpawnStar(string textToDisplay, int sortingLayer = 0)
    {
        // Increment the sorting layer for multi hit combat stars
        GetComponentInChildren<SpriteRenderer>().sortingOrder += sortingLayer; 
        GetComponentInChildren<Canvas>().sortingOrder += sortingLayer;

        animator = GetComponent<Animator>();
        float animationTime = animationToPlay.length;

        TMP_Text displayText = GetComponentInChildren<TMP_Text>();
        displayText.text = textToDisplay;

        StartCoroutine(AnimateAndDestroy(animationTime));

        return animationTime; 
    }

    IEnumerator AnimateAndDestroy(float time)
    {
        animator.Play(animationToPlay.name); 
        yield return new WaitForSeconds(time);

        Destroy(gameObject); 
    }
}
