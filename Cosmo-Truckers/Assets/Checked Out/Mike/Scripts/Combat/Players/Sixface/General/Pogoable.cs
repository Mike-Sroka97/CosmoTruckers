using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pogoable : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.name == "DownAttackArea" && collision.transform.parent.name == "Sixface combat")
        {
            collision.transform.parent.GetComponent<SixfaceINA>().Pogo();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.transform.name == "DownAttackArea" && collision.transform.parent.name == "Sixface combat")
        {
            collision.transform.parent.GetComponent<SixfaceINA>().Pogo();
        }
    }
}
