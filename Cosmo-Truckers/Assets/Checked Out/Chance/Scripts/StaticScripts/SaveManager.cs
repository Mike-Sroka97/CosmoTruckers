using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

[System.Serializable]
public static class SaveManager
{
    static bool TutorialFinished;

    //Set name of save file here defaults to auto save if no name is set
    static string GameName = "SaveData";

    const string tutorialStatus = "/tutorialFinished.data";
    const string playerUnlocks = "/unlockedCharacters.data";
    const string dimensionOne = "/dimensionOne.data";

    public static void SaveTutorialStatus(bool finished)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + tutorialStatus;
        FileStream stream = new FileStream(path, FileMode.Create);

        formatter.Serialize(stream, finished);
        stream.Close();
    }

    public static TutorialData LoadTutorialStatus()
    {
        string path = Application.persistentDataPath + tutorialStatus;

        if (File.Exists(path))
        {
            //Load save data if it does exist
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            TutorialData data = (TutorialData)formatter.Deserialize(stream);

            stream.Close();

            return data;
        }
        else
        {
            //Create save data if it doesn't exist
            TutorialData data = new TutorialData();
            SaveTutorialStatus(data.TutorialFinished);
            return data;
        }
    }

    public static void SaveDimensionOne(DimensionOneLevelData data)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + dimensionOne;
        FileStream stream = new FileStream(path, FileMode.Create);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static DimensionOneLevelData LoadDimensionOne()
    {
        string path = Application.persistentDataPath + dimensionOne;

        if(File.Exists(path))
        {
            //Load save data if it does exist
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            DimensionOneLevelData data = (DimensionOneLevelData)formatter.Deserialize(stream);

            stream.Close();

            return data;
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
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + playerUnlocks;
        FileStream stream = new FileStream(path, FileMode.Create);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static PlayerData LoadPlayerData()
    {
        string path = Application.persistentDataPath + playerUnlocks;

        if (File.Exists(path))
        {
            //Load save data if it does exist
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            PlayerData data = (PlayerData)formatter.Deserialize(stream);

            stream.Close();

            return data;
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

    //public static void Save(SaveData data, int character)
    //{
    //    BinaryFormatter formatter = new BinaryFormatter();
    //    string path = Path.Combine(Application.persistentDataPath, $"SaveData{Application.version}{character}");
    //    Directory.CreateDirectory(path);
    //    path = Path.Combine(path, GameName);

    //    using (FileStream stream = new FileStream(path, FileMode.Create))
    //        formatter.Serialize(stream, JsonUtility.ToJson(data));
    //}

    //public static SaveData Load(int character)
    //{
    //    string path = Path.Combine(Application.persistentDataPath, $"SaveData{Application.version}{character}");
    //    path = Path.Combine(path, GameName);

    //    if (!File.Exists(path)) return null;

    //    BinaryFormatter formatter = new BinaryFormatter();
    //    string data;

    //    using (FileStream stream = new FileStream(path, FileMode.Open))
    //        data = formatter.Deserialize(stream) as string;

    //    return JsonUtility.FromJson<SaveData>(data);
    //}

    public static bool DeleteSave(int character)
    {
        string path = Path.Combine(Application.persistentDataPath, $"SaveData{Application.version}{character}");
        path = Path.Combine(path, GameName);

        if (!File.Exists(path)) return false;

        File.Delete(path);

        return true;
    }
}
