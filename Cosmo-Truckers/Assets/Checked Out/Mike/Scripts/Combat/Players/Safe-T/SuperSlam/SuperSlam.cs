using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuperSlam : MonoBehaviour
{
    [HideInInspector] public int Score;
    [HideInInspector] public bool PlayerDead;
    [SerializeField] GameObject[] layouts;

    private void Start()
    {
        FindObjectOfType<SafeTINA>().SetMoveSpeed(0);

        int random = UnityEngine.Random.Range(0, layouts.Length);
        Instantiate(layouts[random], transform);
    }
}
