using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TripleTetherHitbox : MonoBehaviour
{
    [SerializeField] float disabledTime;
    [SerializeField] Material enabledMaterial;

    TripleTetherEnemy parent;
    Collider2D myCollider;
    SpriteRenderer myRenderer;
    Material disabledMaterial; 

    private void Start()
    {
        parent = GetComponentInParent<TripleTetherEnemy>();
        myCollider = GetComponent<Collider2D>();
        myRenderer = GetComponent<SpriteRenderer>();
        disabledMaterial = myRenderer.material;
        myRenderer.material = enabledMaterial; 
    }

    IEnumerator DisableMe()
    {
        myCollider.enabled = false;
        myRenderer.material = disabledMaterial;

        yield return new WaitForSeconds(disabledTime);

        myCollider.enabled = true;
        myRenderer.material = enabledMaterial;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("PlayerAttack"))
        {
            parent.TakeDamage();
            StartCoroutine(DisableMe());
        }
    }
}
