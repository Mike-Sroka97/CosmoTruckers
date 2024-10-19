using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class CombatStar : MonoBehaviour
{
    [SerializeField] AnimationClip animationToPlay;
    Animator animator;

    /// <summary>
    /// Setup the star and return its animation length in seconds
    /// </summary>
    /// <param name="textToDisplay"></param>
    /// <param name="newMaterial"></param>
    /// <param name="sortingLayer"></param>
    /// <returns></returns>
    public float SetupStar(string textToDisplay, Material newMaterial, int sortingLayer = 0)
    {
        // Makes certain that next combat star sprite is higher than previous combat star text layer
        sortingLayer = sortingLayer * 2;

        SpriteRenderer renderer = GetComponentInChildren<SpriteRenderer>();
        renderer.material = newMaterial;
        renderer.material.SetFloat("_Instance", sortingLayer); 

        // Increment the sorting layer for multi hit combat stars
        renderer.sortingOrder += sortingLayer; 
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
