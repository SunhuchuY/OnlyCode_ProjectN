using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine;

public class QuestExcelDataReader : ITableReader<Dictionary<int, List<Quest>>>
{
    public const string JsonPath = "Table/quest";

    public Dictionary<int, List<Quest>> ReadTable(string jsonPath)
    {
        TextAsset jsonTextAsset = Resources.Load<TextAsset>(jsonPath);

        if (jsonTextAsset == null)
        {
            Debug.LogError($"Failed to load JSON file at path {jsonPath}");
            throw new System.Exception($"Failed to load JSON file at path {jsonPath}");
        }

        string json = jsonTextAsset.text;
        return JsonConvert.DeserializeObject<Dictionary<int, List<Quest>>>(json);
    }
}