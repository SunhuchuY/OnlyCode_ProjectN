using UnityEngine;

public class HealMonster : MonoBehaviour
{
    private Monster monster;

    [SerializeField] private HealMonsterAround healMonsterAround;
    [SerializeField] private HealMonsterType type = HealMonsterType.hpPercent;
    [SerializeField] private float value = 0;

    private void Awake()
    {
        monster = GetComponent<Monster>();
    }

    public void AlternativeFuntionHeal()
    {
        switch (type)
        {
            case HealMonsterType.hpPercent:
                float temp = (float)GameManager.Instance.playerScript.Stats["Hp"].CurrentValueInt * (value / 100);
                healMonsterAround.healRange(temp);
                break;
        }
    }
}