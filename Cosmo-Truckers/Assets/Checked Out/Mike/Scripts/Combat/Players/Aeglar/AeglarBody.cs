using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AeglarBody : MonoBehaviour
{
    AeglarINA aeglar;

    private void Start()
    {
        aeglar = GetComponentInParent<AeglarINA>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "EnemyDamaging" && !aeglar.iFrames)
        {
            aeglar.TakeDamage();
        }
    }
}
