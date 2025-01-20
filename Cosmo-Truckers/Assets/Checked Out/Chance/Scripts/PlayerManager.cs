using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance;

    [SerializeField] List<CharacterSO> AllCharacters;
    public List<CharacterSO> SelectedCharacters;

    //The current character the player has selected to play
    public int PlayerID;
    CharacterSO Player;
    [SerializeField] SaveData PlayerData;
    public SaveData GetPlayerData { get => PlayerData; }
    public CharacterSO GetPlayer { get => AllCharacters[PlayerID]; }
    private void Awake() => Instance = this;

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
}
