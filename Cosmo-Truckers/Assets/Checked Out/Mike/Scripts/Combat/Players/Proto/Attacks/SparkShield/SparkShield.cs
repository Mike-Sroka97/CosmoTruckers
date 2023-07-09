using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SparkShield : CombatMove
{
    [SerializeField] float xTeleportLimit;
    [SerializeField] float yTeleportLimit;

    [HideInInspector] public bool PlayerBuff = false;

    ProtoINA proto;

    private void Start()
    {
        proto = FindObjectOfType<ProtoINA>();
        proto.SetTelportBoundaries(xTeleportLimit, yTeleportLimit, -xTeleportLimit, -yTeleportLimit);
        GenerateLayout();
        StartMove();
    }

    public override void EndMove()
    {
        proto.ResetTeleportBoundaries();
    }
}
