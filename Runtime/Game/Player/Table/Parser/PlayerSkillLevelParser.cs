using System.Collections.Generic;
using UnityEngine;

public class PlayerSkillLevelParser : MonoBehaviour
{
    public const string JsonPath = "Player/Json/skillLevel";
    public Dictionary<int, PlayerSkillLevelData> PlayerSkillLevels { get; private set;  }

    private void Awake()
    {
        PlayerSkillLevels = PlayerTableReader<PlayerSkillLevelData>.ReadTable(JsonPath);    
    }
}