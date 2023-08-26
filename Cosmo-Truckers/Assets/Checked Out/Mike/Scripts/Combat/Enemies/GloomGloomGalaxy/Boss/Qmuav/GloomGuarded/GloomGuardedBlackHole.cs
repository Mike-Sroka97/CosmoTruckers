using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GloomGuardedBlackHole : MonoBehaviour
{
    CircleCollider2D myBlackHole; //smirking emoji
    Graviton myGraviton;
    SpriteRenderer myRenderer;

    private void Start()
    {
        myBlackHole = GetComponent<CircleCollider2D>();
        myGraviton = GetComponent<Graviton>();
        myRenderer = GetComponent<SpriteRenderer>();
    }

    public void ActivateMe()
    {
        myBlackHole.enabled = true;
        myGraviton.enabled = true;
        myRenderer.color = new Color(myRenderer.color.r, myRenderer.color.g, myRenderer.color.b, .8f);
    }
}
