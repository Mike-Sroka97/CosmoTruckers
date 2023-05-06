using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LongDogNose : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((collision.gameObject.name == "NeckLineRenderer(Clone)" || collision.gameObject.name == "Body") && collision.transform.tag == "Player")
        {
            GetComponentInParent<LongDogINA>().StretchingCollision();
        }
    }
}
