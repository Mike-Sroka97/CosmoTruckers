using System;
using System.Collections.Generic;

[Serializable]
public class SettingsData
{
    // Controls Data
    public int GamepadLayout = -1;
    // Volume Data
    public int MasterVolume = -1; 
    public int MusicVolume = -1; 
    public int SfxVolume = -1; 
    public int DialogVolume = -1; 

    /// <summary>
    /// Setup default values for settings
    /// </summary>
    public void InitialSetup()
    {
        SetupControls(); 
        SetupSounds();
    }

    /// <summary>
    /// Load player data
    /// </summary>
    public void LoadPlayerData()
    {
        SettingsData loadData = SettingsManager.LoadSettingsData();
        GamepadLayout = loadData.GamepadLayout;
    }

    /// <summary>
    /// Save the gameplad layout being selected
    /// </summary>
    /// <param name="layoutToSave"></param>
    /// <returns></returns>
    public SettingsData SaveGamepadLayoutSelection(int layoutToSave)
    {
        SettingsData loadData = SettingsManager.LoadSettingsData();

        if (loadData.GamepadLayout == -1)
            loadData.SetupControls();

        loadData.GamepadLayout = layoutToSave; 
        SettingsManager.SaveSettingsData(loadData);
        return loadData;
    }

    /// <summary>
    /// Initial setup for default control specs
    /// </summary>
    private void SetupControls()
    {
        GamepadLayout = 0; 
    }

    /// <summary>
    /// Initial setup for default sound settings
    /// </summary>
    private void SetupSounds()
    {
        MasterVolume = 50;
        MusicVolume = 100;
        SfxVolume = 100;
        DialogVolume = 100;
    }
}
