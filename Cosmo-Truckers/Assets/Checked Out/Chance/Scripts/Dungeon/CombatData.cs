using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

public class CombatData : MonoBehaviour
{
    public bool TESTING = true;

    public static CombatData Instance;
    public List<PlayerManager> PlayersToSpawn = new();
    public List<GameObject> EnemySummonsToSpawn = new();

    private void Awake()
    {
        Instance = this;

        //If Network is active will get the current players
        //Other wise needs to be set manually
        if(NetworkManager.singleton)
        {
            PlayersToSpawn = FindObjectsOfType<PlayerManager>().ToList();
        }
    }
}
