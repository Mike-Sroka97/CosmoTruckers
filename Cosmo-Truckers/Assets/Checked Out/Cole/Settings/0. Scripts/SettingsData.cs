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
        SetupVolume();
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
    /// Save the gamepad layout being selected
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
    /// Save all of the volume types being selected
    /// </summary>
    /// <param name="updateVolumeSettings"></param>
    /// <returns></returns>
    public SettingsData SaveVolume(SettingsData updateVolumeSettings)
    {
        SettingsData loadData = SettingsManager.LoadSettingsData();

        if (loadData.MasterVolume == -1)
            loadData.SetupVolume();

        if (updateVolumeSettings.MasterVolume == -1)
            return loadData;
        else
        {
            loadData.MasterVolume = updateVolumeSettings.MasterVolume;
            loadData.MusicVolume = updateVolumeSettings.MusicVolume;
            loadData.SfxVolume = updateVolumeSettings.SfxVolume;
            loadData.DialogVolume = updateVolumeSettings.DialogVolume;
        }

        SettingsManager.SaveSettingsData(loadData);
        return loadData; 
    }

    public SettingsData ResetVolume()
    {
        SettingsData loadData = SettingsManager.LoadSettingsData();
        loadData.SetupVolume();

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
    private void SetupVolume()
    {
        MasterVolume = 50;
        MusicVolume = 100;
        SfxVolume = 100;
        DialogVolume = 100;

        AudioManager.Instance.UpdateVolumes(this); 
    }
}
