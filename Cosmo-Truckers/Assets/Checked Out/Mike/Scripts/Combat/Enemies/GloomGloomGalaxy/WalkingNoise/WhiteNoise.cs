using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhiteNoise : MonoBehaviour
{
    [HideInInspector] public int Score;
    [SerializeField] GameObject[] layouts;

    private void Start()
    {
        int random = UnityEngine.Random.Range(0, layouts.Length);
        Instantiate(layouts[random], transform);
    }
}
