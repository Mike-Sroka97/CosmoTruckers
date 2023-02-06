using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

[System.Serializable]
public static class SaveManager
{
    //Set name of save file here defaults to auto save if no name is set
    static string GameName = "SaveData";

    public static void Save(SaveData data)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Path.Combine(Application.persistentDataPath, $"SaveData{Application.version}");
        Directory.CreateDirectory(path);
        path = Path.Combine(path, GameName);

        using (FileStream stream = new FileStream(path, FileMode.Create))
            formatter.Serialize(stream, JsonUtility.ToJson(data));
    }

    public static SaveData Load()
    {
        string path = Path.Combine(Application.persistentDataPath, $"SaveData{Application.version}");
        path = Path.Combine(path, GameName);

        if (!File.Exists(path)) return null;

        BinaryFormatter formatter = new BinaryFormatter();
        string data;

        using (FileStream stream = new FileStream(path, FileMode.Open))
            data = formatter.Deserialize(stream) as string;

        return JsonUtility.FromJson<SaveData>(data);
    }

    public static bool DeleteSave()
    {
        string path = Path.Combine(Application.persistentDataPath, $"SaveData{Application.version}");
        path = Path.Combine(path, GameName);

        if (!File.Exists(path)) return false;

        File.Delete(path);

        return true;
    }
}

[System.Serializable]
public class SaveData
{
    public bool TutorialFinished = false;
}
