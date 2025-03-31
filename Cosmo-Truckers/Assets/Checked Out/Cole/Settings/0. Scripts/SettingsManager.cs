using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

[System.Serializable]
public static class SettingsManager
{
    const string settingsData = "/settings.data";
    public const int MaxGamepadLayouts = 3;

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

            return data;
        }
        else
        {
            //Create save data if it doesn't exist
            SettingsData data = new SettingsData();
            data.InitialSetup();
            SaveSettingsData(data);
            return data;
        }
    }
}
