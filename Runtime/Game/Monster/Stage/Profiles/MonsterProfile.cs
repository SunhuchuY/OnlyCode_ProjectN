using System.Numerics;
using Vector2 = UnityEngine.Vector2;

public class MonsterProfile : GameActorData
{
    public readonly int Id;
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
    public readonly string AnimationControllerAddress;
    public readonly string HitPivot;
    public readonly string DamagePivot;

    public MonsterProfile(int id, string monsterImageName, string explanation, int attackType, int hp, int atk, 
        float attackSpeed, float speed, float detectionRange, BigInteger getMagicStone, BigInteger getXP, BigInteger getGold, 
        string uniqueAbilityJsonText, string specialAbilityJsonText, string eventJsonText, string animationControllerAddress,
        string hitPivot, string damagePivot)
    {
        Id = id;
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
        AnimationControllerAddress = animationControllerAddress;
        HitPivot = hitPivot;
        DamagePivot = damagePivot;
    }
}