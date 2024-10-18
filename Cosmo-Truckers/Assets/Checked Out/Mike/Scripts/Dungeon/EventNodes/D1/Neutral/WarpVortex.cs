using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarpVortex : EventNodeBase
{
    public void AcceptFate()
    {
        //Get healths and shuffle
        List<int> currentHealth = new List<int>();

        foreach(PlayerVessel vessel in PlayerVesselManager.Instance.PlayerVessels)
            currentHealth.Add(vessel.MyCharacter.CurrentHealth);

        MathHelpers.Shuffle(currentHealth);

        //heal or damage when swapping with pierce
        for(int i = 0; i < 4; i++)
        {
            PlayerCharacter currentCharacter = PlayerVesselManager.Instance.PlayerVessels[i].MyCharacter;

            if (currentCharacter.CurrentHealth > currentHealth[i])
                currentCharacter.TakeDamage(currentCharacter.CurrentHealth - currentHealth[i], true);
            else if(currentCharacter.CurrentHealth < currentHealth[i])
                currentCharacter.TakeHealing(currentHealth[i] - currentCharacter.CurrentHealth, true);
        }

        StartCoroutine(SelectionChosen());
    }
}
