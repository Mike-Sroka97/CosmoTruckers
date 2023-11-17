using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LongDogVessel : PlayerVessel
{
    [SerializeField] GameObject[] LoadedBullets;
    [SerializeField] GameObject[] ReserveBullets;

    public override void Initialize(PlayerCharacter player)
    {
        base.Initialize(player);
        DisplayBullets();
    }

    public void DisplayBullets()
    {
        ClearBullets();

        for(int i = 0; i < MyMana.GetComponent<LongDogMana>().LoadedBullets.Count; i++)
        {
            LoadedBullets[i].SetActive(true);
            //handle sprite based on type of bullet
        }

        for (int i = 0; i < MyMana.GetComponent<LongDogMana>().ReserveBullets.Count; i++)
        {
            ReserveBullets[i].SetActive(true);
            //handle sprite based on type of bullet
        }
    }

    private void ClearBullets()
    {
        foreach (GameObject bullet in LoadedBullets)
            bullet.SetActive(false);
        foreach (GameObject bullet in ReserveBullets)
            bullet.SetActive(false);   
    }
}
