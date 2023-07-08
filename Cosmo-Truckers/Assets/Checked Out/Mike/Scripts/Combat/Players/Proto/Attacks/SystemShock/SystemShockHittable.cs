using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemShockHittable : MonoBehaviour
{
    [SerializeField] Color disabledColor;
    [SerializeField] Color enabledColor;
    [SerializeField] Color hitColor;
    [HideInInspector] public bool Hit { get; private set; }

    SpriteRenderer myRenderer;
    Collider2D myCollider;
    SystemShock minigame;

    private void Start()
    {
        myRenderer = GetComponentsInChildren<SpriteRenderer>()[1];
        myCollider = GetComponent<Collider2D>();
        minigame = FindObjectOfType<SystemShock>();
    }

    public void DeactivateMe()
    {
        myRenderer.color = disabledColor;
        Hit = false;
    }

    public void ActivateMe()
    {
        myRenderer.color = enabledColor;
        myCollider.enabled = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "PlayerAttack")
        {
            minigame.Score++;
            Hit = true;
            myRenderer.color = hitColor;
            myCollider.enabled = false;
        }
    }
}
