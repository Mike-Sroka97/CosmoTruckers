using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TripleTetherHitbox : MonoBehaviour
{
    [SerializeField] float disabledTime;
    [SerializeField] Color disabledColor;

    TripleTetherEnemy parent;
    Collider2D myCollider;
    SpriteRenderer myRenderer;
    Color startingColor;

    private void Start()
    {
        parent = GetComponentInParent<TripleTetherEnemy>();
        myCollider = GetComponent<Collider2D>();
        myRenderer = GetComponent<SpriteRenderer>();
        startingColor = myRenderer.color;
    }

    IEnumerator DisableMe()
    {
        myCollider.enabled = false;
        myRenderer.color = disabledColor;

        yield return new WaitForSeconds(disabledTime);

        myCollider.enabled = true;
        myRenderer.color = startingColor;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "PlayerAttack")
        {
            parent.TakeDamage();
            StartCoroutine(DisableMe());
        }
    }
}
