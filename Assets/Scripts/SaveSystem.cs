using System.IO;
using UnityEngine;

public static class SaveSystem
{
    private static string Path => System.IO.Path.Combine(Application.persistentDataPath, "save.json");

    public static void Save(PlayerData data)
    {
        string json = JsonUtility.ToJson(data, prettyPrint: true);
        File.WriteAllText(Path, json);
    }

    public static PlayerData Load()
    {
        if (!File.Exists(Path))
        {
            return new PlayerData();
        }

        string json = File.ReadAllText(Path);
        return JsonUtility.FromJson<PlayerData>(json);

    }
}
