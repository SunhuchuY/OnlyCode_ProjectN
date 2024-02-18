using System.Collections.Generic;
using UnityEngine;

public class PlayerLevelParser : MonoBehaviour
{
    private const string jsonPath = "Player/Json/playerLevel";
    public Dictionary<int, PlayerLevelData> PlayerLevelDatas;

    private void Awake()
    {
        PlayerLevelDatas = PlayerTableReader<PlayerLevelData>.ReadTable(jsonPath);
    }
}