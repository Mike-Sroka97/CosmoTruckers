using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathKillHeart : MonoBehaviour
{
    [SerializeField] Transform[] nodes;
    [SerializeField] float moveSpeed;
    [SerializeField] Material successHitMaterial, iFrameMaterial;
    [SerializeField] AnimationClip hurtAnimation; 

    int currentIndex;
    Collider2D myCollider;
    DeathKill minigame;
    Animator myAnimator; 

    bool isMoving = false;

    private void Start()
    {
        currentIndex = UnityEngine.Random.Range(0, 2); 
        transform.position = nodes[currentIndex].position;
        myCollider = GetComponent<Collider2D>();
        minigame = FindObjectOfType<DeathKill>();
        myAnimator = GetComponent<Animator>();
    }

    private void Update()
    {
        MoveMe();
    }

    private void MoveMe()
    {
        if (!isMoving)
            return;

        transform.position = Vector2.MoveTowards(transform.position, nodes[currentIndex].position, moveSpeed * Time.deltaTime);

        if(transform.position == nodes[currentIndex].position)
        {
            isMoving = false;
            myCollider.enabled = true;
            gameObject.GetComponent<SpriteRenderer>().material = successHitMaterial;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "PlayerAttack")
        {
            minigame.Score--;
            minigame.AugmentScore--;

            myCollider.enabled = false;
            gameObject.GetComponent<SpriteRenderer>().material = iFrameMaterial;
            myAnimator.Play(hurtAnimation.name); 

            if (minigame.Score == 0)
            {
                minigame.EndMove(); 
            }
            else
            {
                currentIndex += 2;
                isMoving = true;
            }
        }
    }
}
