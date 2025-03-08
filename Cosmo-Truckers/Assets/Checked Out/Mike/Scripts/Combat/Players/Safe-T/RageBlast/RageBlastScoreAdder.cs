using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RageBlastScoreAdder : MonoBehaviour
{
    RageBlastLoop parent;

    private void Start()
    {
        parent = GetComponentInParent<RageBlastLoop>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            StartCoroutine(parent.FadeOut());
        }
    }
}
