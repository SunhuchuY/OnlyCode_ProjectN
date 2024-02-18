using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTableReader<T> where T : struct
{
    public static Dictionary<int, T> ReadTable(string jsonPath)
    {
        TextAsset jsonTextAsset = Resources.Load<TextAsset>(jsonPath);

        if (jsonTextAsset == null)
        {
            Debug.LogError($"Failed to load JSON file at path {jsonPath}");
            throw new System.Exception($"Failed to load JSON file at path {jsonPath}");
        }

        string json = jsonTextAsset.text;
        return JsonConvert.DeserializeObject<Dictionary<int, T>>(json);
    }
}