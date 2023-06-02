using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SPPswitch : MonoBehaviour
{
    [SerializeField] GameObject myDefaultDeadZone;
    [SerializeField] SPPlayoutGenerator mySpawnZone;

    Collider2D myCollider;

    private void Start()
    {
        myCollider = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "PlayerAttack")
        {
            myDefaultDeadZone.SetActive(false);
            myCollider.enabled = false;
            mySpawnZone.GenerateLayout();
        }
    }

    public void ResetMe()
    {
        myDefaultDeadZone.SetActive(true);
        myCollider.enabled = true;
    }
}
