using System;
using System.Collections.Generic;
using static SpellCraftingButton;

[Serializable]
public class PlayerData
{
    public Dictionary<int, bool> UnlockedPlayerIDs;
    public Dictionary<Tuple<int, int>, bool> UnlockedSpells;
    public Dictionary<int, int> SelectedSpecs;
    public int CommonSpellTokens = 0;
    public int RareSpellTokens = 0;
    public int LegendarySpellTokens = 0;

    const int totalCharacters = 8;

    /// <summary>
    /// Unlock / reset a character
    /// </summary>
    /// <param name="unlockID"></param>
    public void SaveCharacterUnlock(int unlockID, bool unlock = true)
    {
        PlayerData loadData = SaveManager.LoadPlayerData();

        if (loadData.UnlockedPlayerIDs == null)
            loadData.SetupUnlockedCharacters();

        loadData.UnlockedPlayerIDs[unlockID] = unlock;
        SaveManager.SavePlayerData(loadData);
    }

    /// <summary>
    /// Unlock / reset a spell
    /// </summary>
    /// <param name="spell"></param>
    /// <param name="unlock"></param>
    public void SaveSpellUnlock(Tuple<int, int> spell, bool unlock = true, Rarity rarity = Rarity.Common)
    {
        if (unlock)
            SaveTokenExchange(rarity, -1);
        PlayerData loadData = SaveManager.LoadPlayerData();

        if (loadData.UnlockedSpells == null)
            loadData.SetupSpells();

        loadData.UnlockedSpells[spell] = unlock;
        SaveManager.SavePlayerData(loadData);
    }

    /// <summary>
    /// Gain / lose tokens
    /// </summary>
    /// <param name="type"></param>
    /// <param name="amount"></param>
    public void SaveTokenExchange(Rarity rarity, int amount = 1)
    {
        PlayerData loadData = SaveManager.LoadPlayerData();

        switch (rarity)
        {
            case Rarity.Common:
                loadData.CommonSpellTokens += amount;
                break;
            case Rarity.Rare:
                loadData.RareSpellTokens += amount;
                break;
            case Rarity.Legendary:
                loadData.LegendarySpellTokens += amount;
                break;
            default:
                break;
        }

        SaveManager.SavePlayerData(loadData);
    }

    public PlayerData SaveSpecSelection(int characterId, int spec)
    {
        PlayerData loadData = SaveManager.LoadPlayerData();

        if (loadData.SelectedSpecs == null)
            loadData.SetupSpecs();

        loadData.SelectedSpecs[characterId] = spec;
        SaveManager.SavePlayerData(loadData);
        return loadData;
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
        SetupUnlockedCharacters();
        SetupSpells();
        SetupTokens();
        SetupSpecs();
    }

    /// <summary>
    /// Default Unlocked Characters
    /// </summary>
    private void SetupUnlockedCharacters()
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

    /// <summary>
    /// Default Spell Setup
    /// </summary>
    private void SetupSpells()
    {
        const int totalSpells = 15;

        UnlockedSpells = new Dictionary<Tuple<int, int>, bool>();
        for (int i = 0; i < totalCharacters; i++)
            for (int j = 0; j < totalSpells; j++)
                UnlockedSpells.Add(new Tuple<int, int>(i, j), false);
    }

    /// <summary>
    /// Default Token Setup
    /// </summary>
    private void SetupTokens()
    {
        CommonSpellTokens = 0;
        RareSpellTokens = 0;
        LegendarySpellTokens = 0;
    }

    /// <summary>
    /// Default Spec Setup
    /// </summary>
    private void SetupSpecs()
    {
        SelectedSpecs = new Dictionary<int, int>();

        for (int i = 0; i < totalCharacters; i++)
            SelectedSpecs.Add(i, 0); //select first spec
    }
}
