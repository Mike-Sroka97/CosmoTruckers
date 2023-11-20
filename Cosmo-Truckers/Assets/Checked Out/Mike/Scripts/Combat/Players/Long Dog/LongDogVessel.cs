using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LongDogVessel : PlayerVessel
{
    [SerializeField] GameObject[] LoadedBullets;
    [SerializeField] GameObject[] ReserveBullets;

    LongDogMana mana;

    public override void Initialize(PlayerCharacter player)
    {
        base.Initialize(player);
        mana = MyMana.GetComponent<LongDogMana>();
        DisplayBullets();
    }

    public void DisplayBullets()
    {
        ClearBullets();

        for(int i = 0; i < mana.LoadedBullets.Count; i++)
        {
            LoadedBullets[i].SetActive(true);
            //handle sprite based on type of bullet
        }

        for (int i = 0; i < mana.ReserveBullets.Count; i++)
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
