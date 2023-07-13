using System.IO;
using UnityEngine;

public class ConfigLoader
{
    private static string configPath = "Assets/Scripts/Config/ConfigFiles/";

    public GeneralConfig LoadGeneralConfig()
    {
        string savedJson = File.ReadAllText(configPath + "general-config.json");
        return JsonUtility.FromJson<GeneralConfig>(savedJson);
    }

    public LevelConfig LoadLevelConfig(string levelKonfigName)
    {
        string savedJson = File.ReadAllText(configPath + levelKonfigName + ".json");
        return JsonUtility.FromJson<LevelConfig>(savedJson);
    }
}