using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafeTBody : MonoBehaviour
{
    SafeTINA safeT;

    private void Start()
    {
        safeT = GetComponentInParent<SafeTINA>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "EnemyDamaging" && !safeT.iFrames)
        {
            safeT.TakeDamage();
        }
    }
}
