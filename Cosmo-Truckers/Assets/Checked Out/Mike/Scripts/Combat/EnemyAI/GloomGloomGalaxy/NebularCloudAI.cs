using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NebularCloudAI : Enemy
{
    bool usingSadPrecipitation = true;

    public override void StartTurn()
    {
        if(usingSadPrecipitation)
        {
            usingSadPrecipitation = !usingSadPrecipitation;
            ChosenAttack = attacks[0];
        }
        else
        {
            usingSadPrecipitation = !usingSadPrecipitation;
            ChosenAttack = attacks[1];
        }

        base.StartTurn();
    }
}
