using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraggyCoatingEnemy : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] float moveSpeed;

    CraggyCoating minigame;

    private void Start()
    {
        transform.right = target.position - transform.position;
        minigame = FindObjectOfType<CraggyCoating>();
    }

    private void Update()
    {
        MoveMe();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "PlayerAttack")
        {
            Destroy(gameObject);
        }
        else if(collision.name == "GoodGuy")
        {
            minigame.PlayerDead = true;
            Destroy(gameObject);
        }
    }

    private void MoveMe()
    {
        transform.position = Vector2.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
    }
}
