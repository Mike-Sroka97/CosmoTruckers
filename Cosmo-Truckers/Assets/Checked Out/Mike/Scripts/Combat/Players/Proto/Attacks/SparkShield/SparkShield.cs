using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SparkShield : CombatMove
{
    [SerializeField] float xTeleportLimit;
    [SerializeField] float yTeleportLimit;

    [HideInInspector] public bool PlayerBuff = false;

    private void Start()
    {
        FindObjectOfType<ProtoINA>().SetTelportBoundaries(xTeleportLimit, yTeleportLimit, -xTeleportLimit, -yTeleportLimit);
        GenerateLayout();
        StartMove();
    }

    public override void EndMove()
    {
        throw new System.NotImplementedException();
    }
}
