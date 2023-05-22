using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FunGun : MonoBehaviour
{
    [HideInInspector] public int Score;
    [SerializeField] GameObject[] layouts;
    [SerializeField] Transform layout;

    FGGun[] guns;
    int currentActiveGun;

    private void Start()
    {
        Instantiate(layouts[UnityEngine.Random.Range(0, layouts.Length)], layout);
        guns = FindObjectsOfType<FGGun>();
        currentActiveGun = UnityEngine.Random.Range(0, guns.Length);
        guns[currentActiveGun].TrackingTime = true;
    }

    public void NextGun()
    {
        if(currentActiveGun + 1 < guns.Length)
        {
            currentActiveGun++;
        }
        else
        {
            currentActiveGun = 0;
        }

        guns[currentActiveGun].TrackingTime = true;
    }
}
