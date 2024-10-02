using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class CombatStar : MonoBehaviour
{
    [SerializeField] AnimationClip animationToPlay;
    Animator animator;

    public float SetupStar(string textToDisplay, Material newMaterial, int sortingLayer = 0)
    {
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
