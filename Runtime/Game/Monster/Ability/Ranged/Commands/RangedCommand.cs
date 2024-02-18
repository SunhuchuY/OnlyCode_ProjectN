using Newtonsoft.Json;
using UnityEngine;

public class RangedCommand
{
    public readonly RangedAbility Ability;
    public readonly string AddDamage;
    public readonly string[] Parameters;  

    public string PrefabName { get; private set; }
    public float PrefabSpeed { get; private set; }

    [JsonConstructor]
    public RangedCommand(RangedAbility ability, string addDamage, string prefab, string parameters)
    {
        Ability = ability;
        AddDamage = addDamage;

        if (!string.IsNullOrEmpty(parameters))
        {
            Parameters = parameters.Split(',');
        }

        SetupPrefab(prefab);
    }

    private void SetupPrefab(string prefab)
    {
        string[] parts = prefab.Split(',');

        if (parts.Length == 2)
        {
            PrefabName = parts[0].Trim(); // 첫 번째 부분은 Name
            if (float.TryParse(parts[1].Trim(), out float speed))
            {
                PrefabSpeed = speed; // 두 번째 부분은 Speed
            }
            else
            {
                Debug.LogError("속도 파싱 실패: " + parts[1]);
            }
        }
        else
        {
            Debug.LogError("잘못된 형식의 프리팹 문자열: " + prefab);
        }
    }
}