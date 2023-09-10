using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SSSpring : MonoBehaviour
{
    [SerializeField] GameObject enableMe;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            enableMe.SetActive(true);
            transform.parent.gameObject.SetActive(false);
        }
    }
}
