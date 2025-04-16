using UnityEngine;
using System.IO;

public static class SaveSystem
{
    public static void SavePlayerData(PlayerData data)
    {
        // Convert the PlayerData to JSON format
        string json = JsonUtility.ToJson(data);
        
        // Write the JSON to a file in the persistent data path
        string path = Application.persistentDataPath + "/playerData.json";
        File.WriteAllText(path, json);
        
        Debug.Log("Player data saved to: " + path);
    }
}