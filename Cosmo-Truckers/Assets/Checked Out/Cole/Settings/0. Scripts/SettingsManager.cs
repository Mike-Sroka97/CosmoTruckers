using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

[System.Serializable]
public static class SettingsManager
{
    const string settingsData = "/settings.data";
    public const int VolumeIncrement = 10;
    public const int MaxVolume = 100;

    public static void SaveSettingsData(SettingsData data)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + settingsData;
        FileStream stream = new FileStream(path, FileMode.Create);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static SettingsData LoadSettingsData()
    {
        string path = Application.persistentDataPath + settingsData;

        if (File.Exists(path))
        {
            //Load save data if it does exist
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            SettingsData data = (SettingsData)formatter.Deserialize(stream);

            stream.Close();

            // If data isn't null return this data, otherwise it will run CreateSettingsData
            if (data != null)
            {
                return data;
            }
        }

        return CreateSettingsData();
    }

    private static SettingsData CreateSettingsData()
    {
        //Create save data if it doesn't exist
        SettingsData data = new SettingsData();
        data.InitialSetup();
        SaveSettingsData(data);
        return data;
    }

    [Serializable]
    public enum AudioTypes
    {
        Master,
        Music,
        Sfx,
        Dialog
    }
}
