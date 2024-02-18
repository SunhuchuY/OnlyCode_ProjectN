using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class PlayerTapParser : MonoBehaviour
{
    public static PlayerTapParser Instance;
    const string JsonPath = "Table/Player/Json";
    public Dictionary<int, PlayerAttackData> attack { get; private set; }
    public Dictionary<int, PlayerDefenseData> defense { get; private set; }
    public Dictionary<int, PlayerHealthData> health { get; private set; }
    public Dictionary<int, PlayerLevelData> levelOfPlayer { get; private set; }
    public Dictionary<int, PlayerSkillLevelData> levelOfSkill { get; private set; }
    public int MAX_LEVEL_PLAYER { get; private set; }
    public int MAX_LEVEL_SKILL { get; private set; }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        LoadData();
        InitMaxLevel();
    }

    private void InitMaxLevel()
    {
        MAX_LEVEL_PLAYER = levelOfPlayer.Count;
        MAX_LEVEL_SKILL = levelOfSkill.Count;   
    }

    private void LoadData()
    {
        attack = LoadJson<PlayerAttackData>("attack");
        defense = LoadJson<PlayerDefenseData>("defense");
        health = LoadJson<PlayerHealthData>("health");
        levelOfPlayer = LoadJson<PlayerLevelData>("playerLevel");
        levelOfSkill = LoadJson<PlayerSkillLevelData>("skillLevel");

#if UNITY_EDITOR
        Debug.Log("플레이어 테이블 관련 데이터를 성공적으로 불러왔습니다!");
#endif
    }

    private Dictionary<int, T> LoadJson<T>(string jsonName) where T : struct
    {
        return PlayerTableReader<T>.ReadTable($"{JsonPath}/{jsonName}");
    }

    public BigInteger GetCost<T>(T type, Dictionary<int, T> data, int level) where T : IUpgradeCost
    {
        if (!data.ContainsKey(level))
        {
            Debug.LogError($"레벨 {level} 데이터가 존재하지 않으므로 0을 리턴합니다.");
            return 0;
        }

        return type.GetCost(level);
    }
}

public interface IUpgradeCost
{
    BigInteger GetCost(int level);
}