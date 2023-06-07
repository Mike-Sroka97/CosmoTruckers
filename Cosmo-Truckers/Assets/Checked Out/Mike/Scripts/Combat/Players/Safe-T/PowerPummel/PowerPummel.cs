using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerPummel : MonoBehaviour
{
    [HideInInspector] public int Score;
    [HideInInspector] public bool PlayerDead;
    [SerializeField] GameObject[] layouts;

    private void Start()
    {
        int random = UnityEngine.Random.Range(0, layouts.Length);
        Instantiate(layouts[random], transform);
    }
}
