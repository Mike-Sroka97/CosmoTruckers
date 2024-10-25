using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonEventNode : MonoBehaviour
{
    public GameObject EventToGenerate;

    public bool Good;
    public bool Neutral;
    public bool Bad;
    public bool Healing;

    [SerializeField] int healingModifier = 2;

    [HideInInspector] public bool Healed;

    public void Heal()
    {
        foreach(PlayerVessel player in PlayerVesselManager.Instance.PlayerVessels)
            player.MyCharacter.TakeHealing(player.MyCharacter.Health / healingModifier, true);

        Healed = true;
    }
}
