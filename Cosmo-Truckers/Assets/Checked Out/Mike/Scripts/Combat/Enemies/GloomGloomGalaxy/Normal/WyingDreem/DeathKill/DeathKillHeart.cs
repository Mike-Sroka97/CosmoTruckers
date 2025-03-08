using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathKillHeart : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    [SerializeField] Material successHitMaterial, iFrameMaterial;
    [SerializeField] AnimationClip hurtAnimation;

    DeathKillNode[] nodes;
    DeathKillNode currentNode;
    Collider2D myCollider;
    DeathKill minigame;
    Animator myAnimator; 

    bool isMoving = false;

    private void Start()
    {
        nodes = FindObjectsOfType<DeathKillNode>(); 

        currentNode = nodes[UnityEngine.Random.Range(0, nodes.Length)];
        transform.position = currentNode.transform.position;
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

        transform.position = Vector2.MoveTowards(transform.position, currentNode.transform.position, moveSpeed * Time.deltaTime);

        if(transform.position == currentNode.transform.position)
        {
            isMoving = false;
            myCollider.enabled = true;
            gameObject.GetComponent<SpriteRenderer>().material = successHitMaterial;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("PlayerAttack"))
        {
            minigame.Score--;
            minigame.AugmentScore--;

            myCollider.enabled = false;
            gameObject.GetComponent<SpriteRenderer>().material = iFrameMaterial;
            myAnimator.Play(hurtAnimation.name); 

            if (!minigame.CheckScoreEqualsValue(0))
            {
                DeathKillNode temp = currentNode; 
                currentNode = temp.GetAlternateNode(); 
                isMoving = true;
            }
        }
    }
}
