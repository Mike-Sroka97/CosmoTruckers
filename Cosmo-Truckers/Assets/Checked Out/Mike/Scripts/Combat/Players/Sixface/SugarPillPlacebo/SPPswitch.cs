using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SPPswitch : MonoBehaviour
{
    [SerializeField] GameObject myDefaultDeadZone;
    [SerializeField] SPPlayoutGenerator mySpawnZone;

    Collider2D myCollider;
    SpriteRenderer myRenderer;

    private void Start()
    {
        myCollider = GetComponent<Collider2D>();
        myRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "PlayerAttack")
        {
            myDefaultDeadZone.SetActive(false);
            myCollider.enabled = false;
            myRenderer.enabled = false;
            mySpawnZone.GenerateLayout();
        }
    }

    public void ResetMe()
    {
        myDefaultDeadZone.SetActive(true);
        myCollider.enabled = true;
        myRenderer.enabled = true;
    }
}
