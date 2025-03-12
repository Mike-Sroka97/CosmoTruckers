using System;
using System.Collections.Generic;

[Serializable]
public class PlayerData
{
    public Dictionary<int, bool> UnlockedPlayerIDs;
    public Dictionary<Tuple<int, int>, bool> UnlockedSpells;
    public int CommonSpellTokens = 5;
    public int RareSpellTokens = 1;
    public int LegendarySpellTokens;

    /// <summary>
    /// Unlock / reset a character
    /// </summary>
    /// <param name="unlockID"></param>
    public void SaveCharacterUnlock(int unlockID, bool unlock = true)
    {
        PlayerData loadData = SaveManager.LoadPlayerData();
        loadData.UnlockedPlayerIDs[unlockID] = unlock;
        SaveManager.SavePlayerData(loadData);
    }

    /// <summary>
    /// Unlock / reset a spell
    /// </summary>
    /// <param name="spell"></param>
    /// <param name="unlock"></param>
    public void SaveSpellUnlock(Tuple<int, int> spell, bool unlock = true)
    {
        PlayerData loadData = SaveManager.LoadPlayerData();
        UnlockedSpells[spell] = unlock;
        SaveManager.SavePlayerData(loadData);
    }

    public void TempSetDick()
    {
        const int totalCharacters = 8;
        const int totalSpells = 15;

        UnlockedSpells = new Dictionary<Tuple<int, int>, bool>();
        for (int i = 0; i < totalCharacters; i++)
            for (int j = 0; j < totalSpells; j++)
                UnlockedSpells.Add(new Tuple<int, int>(i, j), false);
    }

    /// <summary>
    /// Gain / lose tokens
    /// </summary>
    /// <param name="type"></param>
    /// <param name="amount"></param>
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

    /// <summary>
    /// Load player data
    /// </summary>
    public void LoadPlayerData()
    {
        PlayerData loadData = SaveManager.LoadPlayerData();
        UnlockedPlayerIDs = loadData.UnlockedPlayerIDs;
        UnlockedSpells = loadData.UnlockedSpells;
        CommonSpellTokens = loadData.CommonSpellTokens;
        RareSpellTokens = loadData.RareSpellTokens;
        LegendarySpellTokens = loadData.LegendarySpellTokens;
    }

    /// <summary>
    /// Setup default values for player data
    /// </summary>
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

        //unlocked spells
        const int totalCharacters = 8;
        const int totalSpells = 15;

        UnlockedSpells = new Dictionary<Tuple<int, int>, bool>();
        for (int i = 0; i < totalCharacters; i++)
            for (int j = 0; j < totalSpells; j++)
                UnlockedSpells.Add(new Tuple<int, int>(i, j), false);

        //spell tokens
        CommonSpellTokens = 1;
        RareSpellTokens = 0;
        LegendarySpellTokens = 0;
    }
}
