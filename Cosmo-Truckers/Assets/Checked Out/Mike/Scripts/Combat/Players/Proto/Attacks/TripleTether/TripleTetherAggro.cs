using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TripleTetherAggro : MonoBehaviour
{
    TripleTetherEnemy parent;

    private void Start()
    {
        parent = GetComponentInParent<TripleTetherEnemy>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            parent.Aggro = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            parent.Aggro = false;
        }
    }
}
