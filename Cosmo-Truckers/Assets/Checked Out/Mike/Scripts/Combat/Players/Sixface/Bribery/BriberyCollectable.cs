using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BriberyCollectable : MonoBehaviour
{
    [SerializeField] BriberyEnemy myEnemy;
    [SerializeField] int row;

    Bribery minigame;
    Collider2D myCollider;
    SpriteRenderer myRenderer;

    private void Start()
    {
        minigame = FindObjectOfType<Bribery>();
        myRenderer = GetComponent<SpriteRenderer>();
        myCollider = GetComponent<Collider2D>();
    }

    public void Activate()
    {
        myRenderer.enabled = true;
        myCollider.enabled = true;
        minigame.ActivatedRows[row] = true;
    }

    private void DeactiveMe()
    {
        myRenderer.enabled = false;
        myCollider.enabled = false;
        minigame.ActivatedRows[row] = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            myEnemy.SendBack();
            DeactiveMe();
        }
    }
}
