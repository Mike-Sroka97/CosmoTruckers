using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance;

    public List<CharacterSO> AllCharacters;
    public List<int> ActivePlayerIDs = new List<int>() { 0, 1, 2, 3 };

    /// <summary>
    /// Sets non destroyable manager
    /// </summary>
    private void Awake()
    {
        PlayerManager[] objs = FindObjectsOfType<PlayerManager>();

        if (objs.Length > 1)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);

        if (!Instance)
            Instance = this;
    }

    /// <summary>
    /// Set players for dungeon to load
    /// </summary>
    /// <param name="newPlayers"></param>
    public void SetActivePlayers(List<int> newPlayers)
    {
        ActivePlayerIDs = newPlayers;
    }
}
