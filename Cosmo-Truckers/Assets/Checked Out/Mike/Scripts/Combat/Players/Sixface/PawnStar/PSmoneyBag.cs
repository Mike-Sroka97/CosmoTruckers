using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PSmoneyBag : MonoBehaviour
{
    [SerializeField] Color hurtColor;
    [SerializeField] float deactiveTime;

    SpriteRenderer myRenderer;
    Color startingColor;
    PawnStar minigame;
    Collider2D myCollider;

    private void Start()
    {
        minigame = FindObjectOfType<PawnStar>();
        myRenderer = GetComponent<SpriteRenderer>();
        myCollider = GetComponent<Collider2D>();
        startingColor = myRenderer.color;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "PlayerAttack")
        {
            minigame.Score++;
            Debug.Log(minigame.Score);
            StartCoroutine(ResetHitbox());
        }
    }

    IEnumerator ResetHitbox()
    {
        myCollider.enabled = false;
        myRenderer.color = hurtColor;
        yield return new WaitForSeconds(deactiveTime);
        myCollider.enabled = true;
        myRenderer.color = startingColor;
    }
}
