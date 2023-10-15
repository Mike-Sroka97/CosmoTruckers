using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVesselManager : MonoBehaviour
{
    [SerializeField] RectTransform[] vesselSpawns;

    [HideInInspector] public static PlayerVesselManager Instance;
    PlayerVessel[] playerVessels;

    private void Awake() => Instance = this;

    public void Initialize()
    {
        //playerVessels = GetComponentsInChildren<PlayerVessel>();
        //foreach(PlayerVessel vessel in playerVessels)
        //    vessel.gameObject.SetActive(false);

        for (int i = 0; i < EnemyManager.Instance.Players.Count; i++)
        {
            PlayerVessel currentVessel = Instantiate(EnemyManager.Instance.Players[i].PlayerVessel, vesselSpawns[i]).GetComponent<PlayerVessel>();
            currentVessel.Initialize(EnemyManager.Instance.Players[i]);
        }
    }
}
