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

    public static void Save(SaveData data, int character)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Path.Combine(Application.persistentDataPath, $"SaveData{Application.version}{character}");
        Directory.CreateDirectory(path);
        path = Path.Combine(path, GameName);

        using (FileStream stream = new FileStream(path, FileMode.Create))
            formatter.Serialize(stream, JsonUtility.ToJson(data));
    }

    public static SaveData Load(int character)
    {
        string path = Path.Combine(Application.persistentDataPath, $"SaveData{Application.version}{character}");
        path = Path.Combine(path, GameName);

        if (!File.Exists(path)) return null;

        BinaryFormatter formatter = new BinaryFormatter();
        string data;

        using (FileStream stream = new FileStream(path, FileMode.Open))
            data = formatter.Deserialize(stream) as string;

        return JsonUtility.FromJson<SaveData>(data);
    }

    public static bool DeleteSave(int character)
    {
        string path = Path.Combine(Application.persistentDataPath, $"SaveData{Application.version}{character}");
        path = Path.Combine(path, GameName);

        if (!File.Exists(path)) return false;

        File.Delete(path);

        return true;
    }
}

[System.Serializable]
public class SaveData
{
    public SaveData()
    {
        PlayerCurrentHP = -1;
        PlayerCurrentDebuffs = new();
        PlayersLoot = new();
        TutorialFinished = false;
    }

    [Header("Combat data")]
    public int PlayerCurrentHP;
    public List<DebuffStackSO> PlayerCurrentDebuffs;

    [Header("Player inventory")]
    public List<Loot> PlayersLoot;

    [Header("Other vars")]
    public bool TutorialFinished;
}
