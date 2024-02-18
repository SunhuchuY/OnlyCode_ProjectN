using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine;

public class WaveTableReader : ITableReader<Dictionary<int, WaveProfile>>
{
    public const string JsonPath = "Table/waveTable";

    public Dictionary<int, WaveProfile> ReadTable(string jsonPath)
    {
        TextAsset _jsonTextAsset = Resources.Load<TextAsset>(JsonPath);

        if (_jsonTextAsset == null)
        {
            Debug.LogError($"Failed to load JSON file at path: {JsonPath}");
            throw new System.Exception($"Failed to load JSON file at path: {JsonPath}");
        }

        string _json = _jsonTextAsset.text;
        return JsonConvert.DeserializeObject<Dictionary<int, WaveProfile>>(_json);
    }
}