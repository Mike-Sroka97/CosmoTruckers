using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPbuttons : MonoBehaviour
{
    [SerializeField] Collider2D myDDRbuttonCollider;
    [SerializeField] Material toggledMaterial;
    [SerializeField] float onTime;
    [SerializeField] SpriteRenderer myDDRbuttonRenderer;
    [SerializeField] SpriteRenderer myRenderer; 

    Material ddrDefaultMaterial; 
    Material defaultMaterial;
    Collider2D myCollider;
    private void Start()
    {
        myCollider = GetComponent<Collider2D>();
        myDDRbuttonRenderer = myDDRbuttonCollider.GetComponent<SpriteRenderer>();
        defaultMaterial = myRenderer.material; 
        ddrDefaultMaterial = myDDRbuttonRenderer.material;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerAttack"))
        {
            myCollider.enabled = false;
            StartCoroutine(ActivateDDRButton());
        }
    }

    IEnumerator ActivateDDRButton()
    {
        myDDRbuttonCollider.enabled = true;
        myDDRbuttonRenderer.material = toggledMaterial;
        myRenderer.material = toggledMaterial;
        yield return new WaitForSeconds(onTime);
        myDDRbuttonCollider.enabled = false;
        myDDRbuttonRenderer.material = ddrDefaultMaterial;
        myRenderer.material = defaultMaterial;
        myCollider.enabled = true;
    }
}
