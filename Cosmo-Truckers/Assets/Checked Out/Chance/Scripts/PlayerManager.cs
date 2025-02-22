using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance;

    public List<CharacterSO> AllCharacters;
    public List<int> ActivePlayerIDs = new List<int>() { 0, 1, 2, 3 };
    [SerializeField] SaveData PlayerData;

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
    /// Load in the player data for this character and set it for online play
    /// </summary>
    /// <param name="id">ID number of current character</param>
    public void SetPlayerCharacter(int id)
    {
        PlayerData = SaveManager.Load(id);
        if (PlayerData == null)
            PlayerData = new SaveData();
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
