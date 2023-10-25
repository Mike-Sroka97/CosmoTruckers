using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VeggieVengeance : CombatMove
{

    public override void StartMove()
    {
        GenerateLayout();
        FindObjectOfType<VeggieVengeanceCannon>().StartMove();
    }

    public override void EndMove()
    {
        FindObjectOfType<VeggieVengeanceCannon>().CalculateMove = false;
    }
}
