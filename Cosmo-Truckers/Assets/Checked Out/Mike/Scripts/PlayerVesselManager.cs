using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVesselManager : MonoBehaviour
{
    [HideInInspector] public static PlayerVesselManager Instance;
    PlayerVessel[] playerVessels;

    private void Awake() => Instance = this;

    public void Initialize()
    {
        playerVessels = GetComponentsInChildren<PlayerVessel>();
        foreach(PlayerVessel vessel in playerVessels)
            vessel.gameObject.SetActive(false);

        for (int i = 0; i < EnemyManager.Instance.Players.Count; i++)
        {
            playerVessels[i].gameObject.SetActive(true);
            playerVessels[i].Initialize(EnemyManager.Instance.Players[i]);
        }
    }
}
