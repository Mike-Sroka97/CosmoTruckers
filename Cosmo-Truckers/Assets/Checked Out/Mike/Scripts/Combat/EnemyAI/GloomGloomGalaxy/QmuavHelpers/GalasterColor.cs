using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GalasterColor : MonoBehaviour
{
    [SerializeField] Color[] galasterColors;

    private void Start()
    {
        GetComponent<SpriteRenderer>().color = galasterColors[Random.Range(0, galasterColors.Length)];
    }
}
