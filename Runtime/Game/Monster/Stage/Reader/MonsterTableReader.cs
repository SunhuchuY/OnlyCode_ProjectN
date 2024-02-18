using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine;

public class MonsterTableReader : ITableReader<Dictionary<int, MonsterProfile>>
{
    public const string JsonPath = "Table/MonsterTable";

    public Dictionary<int, MonsterProfile> ReadTable(string jsonPath)
    {
        TextAsset jsonTextAsset = Resources.Load<TextAsset>(jsonPath);

        if (jsonTextAsset == null)
        {
            Debug.LogError($"Failed to load JSON file at path {jsonPath}");
            throw new System.Exception($"Failed to load JSON file at path {jsonPath}");
        }

        string json = jsonTextAsset.text;
        return JsonConvert.DeserializeObject<Dictionary<int, MonsterProfile>>(json);
    }
}