using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BriberyCollectable : MonoBehaviour
{
    [SerializeField] BriberyEnemy myEnemy;
    [SerializeField] int row;
    [SerializeField] GameObject moneyParticle; 

    Bribery minigame;
    Collider2D myCollider;
    SpriteRenderer myRenderer;

    private void Start()
    {
        minigame = FindObjectOfType<Bribery>();
        myRenderer = GetComponent<SpriteRenderer>();
        myCollider = GetComponent<Collider2D>();
    }

    private void Update()
    {
        if(minigame.DisabledRows[row])
        {
            DeactiveMe();
        }
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
            Instantiate(moneyParticle, transform.position, Quaternion.identity, minigame.transform); 
            myEnemy.SendBack();
            DeactiveMe();
        }
    }
}
