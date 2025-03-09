using System;
using System.Collections.Generic;

[Serializable]
public class PlayerData
{
    public Dictionary<int, bool> UnlockedPlayerIDs;
    public int CommonSpellTokens = 5;
    public int RareSpellTokens = 1;
    public int LegendarySpellTokens;

    public void SaveCharacterUnlock(int unlockID)
    {
        PlayerData loadData = SaveManager.LoadPlayerData();
        loadData.UnlockedPlayerIDs[unlockID] = true;
        SaveManager.SavePlayerData(loadData);
    }

    public void SaveTokenExchange(int type, int amount = 1)
    {
        PlayerData loadData = SaveManager.LoadPlayerData();
        
        switch(type)
        {
            case 0:
                loadData.CommonSpellTokens += amount;
                break;
            case 1:
                loadData.RareSpellTokens += amount;
                break;
            case 2:
                loadData.LegendarySpellTokens += amount;
                break;
            default:
                break;
        }

        SaveManager.SavePlayerData(loadData);
    }

    public void LoadPlayerData()
    {
        PlayerData loadData = SaveManager.LoadPlayerData();
        UnlockedPlayerIDs = loadData.UnlockedPlayerIDs;
        CommonSpellTokens = loadData.CommonSpellTokens;
        RareSpellTokens = loadData.RareSpellTokens;
        LegendarySpellTokens = loadData.LegendarySpellTokens;
    }

    public void InitialSetup()
    {
        //unlocked characters
        UnlockedPlayerIDs = new Dictionary<int, bool>();
        UnlockedPlayerIDs.Add(0, true);
        UnlockedPlayerIDs.Add(1, true);
        UnlockedPlayerIDs.Add(2, true);
        UnlockedPlayerIDs.Add(3, true);
        UnlockedPlayerIDs.Add(4, false);
        UnlockedPlayerIDs.Add(5, false);
        UnlockedPlayerIDs.Add(6, false);
        UnlockedPlayerIDs.Add(7, false);

        //spell tokens
        CommonSpellTokens = 0;
        RareSpellTokens = 0;
        LegendarySpellTokens = 0;
    }
}
