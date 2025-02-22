using System;
using System.Collections.Generic;

[Serializable]
public class PlayerData
{
    public Dictionary<int, bool> UnlockedPlayerIDs;

    public void SavePlayerData(int unlockID)
    {
        PlayerData loadData = SaveManager.LoadPlayerData();
        loadData.UnlockedPlayerIDs[unlockID] = true;
        SaveManager.SavePlayerData(loadData);
    }

    public void LoadPlayerData()
    {
        PlayerData loadData = SaveManager.LoadPlayerData();
        UnlockedPlayerIDs = loadData.UnlockedPlayerIDs;
    }

    public void InitialSetup()
    {
        UnlockedPlayerIDs = new Dictionary<int, bool>();
        UnlockedPlayerIDs.Add(0, true);
        UnlockedPlayerIDs.Add(1, true);
        UnlockedPlayerIDs.Add(2, true);
        UnlockedPlayerIDs.Add(3, true);
        UnlockedPlayerIDs.Add(4, false);
        UnlockedPlayerIDs.Add(5, false);
        UnlockedPlayerIDs.Add(6, false);
        UnlockedPlayerIDs.Add(7, false);
    }
}
