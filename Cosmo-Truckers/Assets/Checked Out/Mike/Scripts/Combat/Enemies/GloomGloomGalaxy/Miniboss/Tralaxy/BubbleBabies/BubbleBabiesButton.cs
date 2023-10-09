using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleBabiesButton : MonoBehaviour
{
    [SerializeField] bool left = false;
    [SerializeField] BubbleBabiesNeedle myNeedle;
    [SerializeField] AnimationClip hurtAnimationClip;
    [SerializeField] float waitBetweenHits = 0.5f;
    [SerializeField] Material defaultMaterial, toggledMaterial; 

    private Animator myAnimator; 
    private bool canHit = true; 
    private float timer;

    private void Start()
    {
        myAnimator = GetComponent<Animator>(); 
    }

    private void Update()
    {
        if (!canHit)
        {
            timer += Time.deltaTime; 

            if (timer >= waitBetweenHits)
            {
                canHit = true;
                timer = 0f;
                GetComponent<SpriteRenderer>().material = defaultMaterial;
            }
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "PlayerAttack")
        {
            if (canHit)
            {
                myNeedle.MoveMe(left);
                canHit = false;
                myAnimator.Play(hurtAnimationClip.name);
                GetComponent<SpriteRenderer>().material = toggledMaterial; 
            }
        }
    }
}
