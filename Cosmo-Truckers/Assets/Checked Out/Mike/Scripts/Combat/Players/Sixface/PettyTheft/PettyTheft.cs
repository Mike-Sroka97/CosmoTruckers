using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PettyTheft : MonoBehaviour
{
    [SerializeField] GameObject[] layouts;
    [HideInInspector] public int Score = 0;

    PTMoney[] moneys;

    private void Start()
    {
        int random = UnityEngine.Random.Range(0, layouts.Length);
        Instantiate(layouts[random], transform);
        moneys = FindObjectsOfType<PTMoney>();
    }

    public void ActivateMoney()
    {
        foreach(PTMoney money in moneys)
        {
            money.Activate();
        }
    }
}
