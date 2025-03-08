using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunOfTheMawGunTrigger : MonoBehaviour
{
    [SerializeField] int myValue;

    GunOfTheMawGun myGun;

    private void Start()
    {
        myGun = FindObjectOfType<GunOfTheMawGun>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            myGun.CurrentBlackListedSpawn = myValue;
            Debug.Log("The current blacklist is: " + myValue);
        }
    }
}
