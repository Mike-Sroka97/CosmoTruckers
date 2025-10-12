using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

[System.Serializable]
public static class SaveManager
{
    //Set name of save file here defaults to auto save if no name is set
    static string GameName = "SaveData";

    const string tutorialStatus = "/tutorialFinished.xml";
    const string playerUnlocks = "/unlockedCharacters.data";
    const string dataLogUnlocks = "/unlockedDataFiles.data";
    const string dimensionOne = "/dimensionOne.data";

    public static void SaveTutorialStatus(TutorialData data)
    {
        string json = JsonUtility.ToJson(data, true);
        string path = Application.persistentDataPath + "/TutorialData.json";
        File.WriteAllText(path, json);
    }

    public static TutorialData LoadTutorialStatus()
    {
        string path = Application.persistentDataPath + tutorialStatus + ".json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            TutorialData loadedData = JsonUtility.FromJson<TutorialData>(json);

            return loadedData;
        }
        else
        {
            //Create save data if it doesn't exist
            TutorialData data = new TutorialData();
            SaveTutorialStatus(data);
            return data;
        }
    }

    public static void SaveDimensionOne(DimensionOneLevelData data)
    {
        string json = JsonUtility.ToJson(data, true);
        string path = Application.persistentDataPath + "/DimensionOneLevelData.json";
        File.WriteAllText(path, json);
    }

    public static DimensionOneLevelData LoadDimensionOne()
    {
        string path = Application.persistentDataPath + dimensionOne + ".json";

        if(File.Exists(path))
        {
            string json = File.ReadAllText(path);
            DimensionOneLevelData loadedData = JsonUtility.FromJson<DimensionOneLevelData>(json);

            return loadedData;
        }
        else
        {
            //Create save data if it doesn't exist
            DimensionOneLevelData data = new DimensionOneLevelData();
            SaveDimensionOne(data);
            return data;
        }
    }

    public static void SavePlayerData(PlayerData data)
    {
        string json = JsonUtility.ToJson(data, true);
        string path = Application.persistentDataPath + "/PlayerData.json";
        File.WriteAllText(path, json);
    }

    public static PlayerData LoadPlayerData()
    {
        string path = Application.persistentDataPath + playerUnlocks + ".json";

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            PlayerData loadedData = JsonUtility.FromJson<PlayerData>(json);

            return loadedData;
        }
        else
        {
            //Create save data if it doesn't exist
            PlayerData data = new PlayerData();
            data.InitialSetup();
            SavePlayerData(data);
            return data;
        }
    }

    public static void SaveDataLogData(DataLogData data)
    {
        string json = JsonUtility.ToJson(data, true);
        string path = Application.persistentDataPath + "/DataLogData.json";
        File.WriteAllText(path, json);
    }

    public static DataLogData LoadDataLogData()
    {
        string path = Application.persistentDataPath + dataLogUnlocks;

        if (File.Exists(path))
        {
            //Load save data if it does exist
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            DataLogData data = (DataLogData)formatter.Deserialize(stream);

            stream.Close();

            return data;
        }
        else
        {
            //Create save data if it doesn't exist
            DataLogData data = new DataLogData();
            data.InitialSetup();
            SaveDataLogData(data);
            return data;
        }
    }

    /// <summary>
    /// Delete all save data 
    /// This excludes the tutorial data
    /// </summary>
    /// <returns></returns>
    public static bool DeleteAllSaveData()
    {
        // Reset Player Data
        PlayerData playerData = new PlayerData();
        playerData.InitialSetup();
        SavePlayerData(playerData);
        if (playerData == null) return false;

        // Reset Data Log Data
        DataLogData dataLogData = new DataLogData();
        dataLogData.InitialSetup();
        SaveDataLogData(dataLogData);
        if (dataLogData == null) return false;

        // Reset D1 Level Data
        DimensionOneLevelData d1Data = new DimensionOneLevelData();
        SaveDimensionOne(d1Data);
        if (d1Data == null) return false;

        // If all data has been reset, return true
        return true; 
    }
}
