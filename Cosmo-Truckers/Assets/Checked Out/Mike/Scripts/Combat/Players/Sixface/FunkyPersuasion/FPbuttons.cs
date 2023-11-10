using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPbuttons : MonoBehaviour
{
    [SerializeField] Collider2D myDDRbuttonCollider;
    [SerializeField] Color onColor;
    [SerializeField] float onTime;

    SpriteRenderer myDDRbuttonRenderer;
    Color offColor;
    Collider2D myCollider;
    private void Start()
    {
        myCollider = GetComponent<Collider2D>();
        myDDRbuttonRenderer = myDDRbuttonCollider.GetComponent<SpriteRenderer>();
        offColor = myDDRbuttonRenderer.color;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "PlayerAttack")
        {
            myCollider.enabled = false;
            StartCoroutine(ActivateDDRButton());
        }
    }

    IEnumerator ActivateDDRButton()
    {
        myDDRbuttonCollider.enabled = true;
        myDDRbuttonRenderer.color = onColor;
        yield return new WaitForSeconds(onTime);
        myDDRbuttonCollider.enabled = false;
        myDDRbuttonRenderer.color = offColor;
        myCollider.enabled = true;
    }
}
