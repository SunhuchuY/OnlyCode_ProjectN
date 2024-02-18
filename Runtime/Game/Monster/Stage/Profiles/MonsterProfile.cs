using Newtonsoft.Json;
using System.Numerics;

public class MonsterProfile
{
    public readonly string MonsterImageName;
    public readonly string Explanation;
    public readonly int AttackType;
    public readonly int HP;
    public readonly int ATK;
    public readonly float AttackSpeed;
    public readonly float Speed;
    public readonly float DetectionRange;
    public readonly BigInteger GetMagicStone;
    public readonly BigInteger GetXP;
    public readonly BigInteger GetGold;
    public readonly string UniqueAbilityJsonText;
    public readonly string SpecialAbilityJsonText;
    public readonly string EventJsonText;

    public MonsterProfile(string monsterImageName, string explanation, int attackType, int hp, int atk, 
        float attackSpeed, float speed, float detectionRange, BigInteger getMagicStone, BigInteger getXP, BigInteger getGold, 
        string uniqueAbilityJsonText, string specialAbilityJsonText, string eventJsonText)
    {
        MonsterImageName = monsterImageName;
        Explanation = explanation;
        AttackType = attackType;
        HP = hp;
        ATK = atk;
        AttackSpeed = attackSpeed;
        Speed = speed;
        DetectionRange = detectionRange;
        GetMagicStone = getMagicStone;
        GetXP = getXP;
        GetGold = getGold;
        UniqueAbilityJsonText = uniqueAbilityJsonText;
        SpecialAbilityJsonText = specialAbilityJsonText;
        EventJsonText = eventJsonText;
    }
}