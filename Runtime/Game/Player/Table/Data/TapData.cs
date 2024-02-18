using System.Numerics;

public struct TapData<T> where T : struct
{
    public readonly T Value;
    public readonly BigInteger PayMagicStone;

    public TapData(T value, BigInteger payMagicStone)
    {
        Value = value;
        PayMagicStone = payMagicStone;
    }
}

public struct PlayerAttackData
{
    public TapData<int> Attack;
    public TapData<float> AttackSpeed;
    public TapData<int> AttackDistance;
    public TapData<int> AttackCritical;
    public TapData<int> AttackCriticalDamage;
}

public struct PlayerDefenseData
{
    public TapData<int> Defense;
    public TapData<int> Counter;
}

public struct PlayerHealthData
{
    public TapData<int> Recovery;
    public TapData<int> MaxHP;
}