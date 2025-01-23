using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CounterBopBag : MonoBehaviour
{
    CounterBop minigame;
    bool canBeHit = true;

    private void Start()
    {
        minigame = transform.parent.parent.GetComponent<CounterBop>();
    }

    private void HandleHit()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player" && canBeHit)
        {
            canBeHit = false;
            HandleHit();
        }
    }
}
