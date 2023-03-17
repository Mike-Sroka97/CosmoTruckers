using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Loot", menuName = "ScriptableObjects/Loot")]
public class Loot : ScriptableObject
{
    [SerializeField] int percentChanceToDrop;
    [SerializeField] string itemName;
    [SerializeField] Sprite itemImage;

    int lootCount = 1;
    public int GetDropChance() { return percentChanceToDrop; }
    public string GetName() { return itemName; }
    public void SetName(string newName) { itemName = newName; }
    public Sprite GetImage() { return itemImage; }

    public void IncrementLootCount() { lootCount++; }
    public int GetLootCount() { return lootCount; }
    //add saving to inventory
    /// <summary>
    /// Will save the new instance of loot to a list stored on the player manager that will be in every scene
    /// </summary>
    /// <param name="pm">The player manager that controlls the network logic for the current player obtaining the loot</param>
    public void SaveLoot(PlayerManager pm)
    {
        pm.GetPlayerData.PlayersLoot.Add(this);
        SaveManager.Save(pm.GetPlayerData, pm.GetPlayer.PlayerID);
    }
}
