using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClockOutKnockOut : MonoBehaviour
{
    [HideInInspector] public int Score = 0;
    [SerializeField] GameObject[] layouts;

    private void Start()
    {
        int random = UnityEngine.Random.Range(0, layouts.Length);
        Instantiate(layouts[random], transform);
    }
}
