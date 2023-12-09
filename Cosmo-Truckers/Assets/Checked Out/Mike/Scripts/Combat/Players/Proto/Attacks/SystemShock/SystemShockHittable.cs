using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemShockHittable : MonoBehaviour
{
    [SerializeField] Color disabledColor;
    [SerializeField] Color enabledColor;
    [SerializeField] Color hitColor;
    [SerializeField] SpriteRenderer myRenderer;
    [HideInInspector] public bool Hit { get; private set; }


    Collider2D myCollider;
    SystemShock minigame;

    private void Start()
    {
        minigame = FindObjectOfType<SystemShock>();
    }

    public void DeactivateMe()
    {
        myRenderer.color = disabledColor;
        Hit = false;
    }

    public void ActivateMe()
    {
        myCollider = GetComponent<Collider2D>();
        myRenderer.color = enabledColor;
        myCollider.enabled = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision);

        if (collision.tag == "PlayerAttack" && !Hit)
        {
            minigame.AugmentScore++;
            minigame.GetComponent<SystemShock>().HittablesHit++;
            Hit = true;
            myRenderer.color = hitColor;
            myCollider.enabled = false;
        }
    }
}
