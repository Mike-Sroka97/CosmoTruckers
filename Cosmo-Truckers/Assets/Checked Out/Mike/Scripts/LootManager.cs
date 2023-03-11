using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootManager : MonoBehaviour
{
    List<Loot> thisCombatsLoot;

    private void Start()
    {
        thisCombatsLoot = new List<Loot>();
    }
    public void AddLoot(Loot loot)
    {
        foreach(Loot oldLoot in thisCombatsLoot)
        {
            if(oldLoot.name == loot.name)
            {
                oldLoot.IncrementLootCount();
                return;
            }
        }
        thisCombatsLoot.Add(loot);
    }

    public List<Loot> GetLoot() { return thisCombatsLoot; }
}
