using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Loot : MonoBehaviour
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
}
