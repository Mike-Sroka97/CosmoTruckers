using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

public class CombatData : MonoBehaviour
{
    public static CombatData Instance;
    public Vector2 combatLocation = new Vector2(0, 0);
    public int dungeonSeed = 0;
    public GameObject EnemysToSpawn = null;
    public List<PlayerManager> PlayersToSpawn = new();
    public bool lastNode = false;


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
