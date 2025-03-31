using System;
using System.Collections.Generic;

[Serializable]
public class SettingsData
{
    // Controls Data
    public int GamepadLayout;
    // Volume Data
    public int MasterVolume; 
    public int MusicVolume; 
    public int SfxVolume; 
    public int DialogVolume; 

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
    /// Setup default control specs
    /// </summary>
    private void SetupControls()
    {
        GamepadLayout = 0; 
    }

    /// <summary>
    /// Setup default sound settings
    /// </summary>
    private void SetupSounds()
    {
        MasterVolume = 50;
        MusicVolume = 100;
        SfxVolume = 100;
        DialogVolume = 100;
    }
}
