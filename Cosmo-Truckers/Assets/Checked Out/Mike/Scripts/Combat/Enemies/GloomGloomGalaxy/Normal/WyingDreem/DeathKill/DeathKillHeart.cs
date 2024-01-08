using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathKillHeart : MonoBehaviour
{
    [SerializeField] Transform[] nodes;
    [SerializeField] float moveSpeed;
    [SerializeField] Material successHitMaterial, iFrameMaterial; 

    int currentIndex;
    Collider2D myCollider;
    DeathKill minigame;

    bool isMoving = false;

    private void Start()
    {
        transform.position = nodes[0].position;
        myCollider = GetComponent<Collider2D>();
        minigame = FindObjectOfType<DeathKill>();
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

            if (currentIndex + 1 == nodes.Length)
            {
                currentIndex = 0;
            }
            else
            {
                currentIndex++;
            }

            isMoving = true;
        }
    }
}
