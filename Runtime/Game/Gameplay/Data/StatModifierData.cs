public enum ModifierOperationType
{
    Add,
    Multiply,
    Override
}

public interface IStatModifierData
{
    public string StatName { get; }
    public string Value { get; } // 계산식
    public ModifierOperationType OperationType { get; }
}

public interface IStatModifierDataOf<T>
    where T : IStatModifier
{
}

public class StatModifierData : IStatModifierData, IStatModifierDataOf<StatModifier>
{
    public string StatName { get; set; }
    public string Value { get; set; }
    public ModifierOperationType OperationType { get; set; } = ModifierOperationType.Add;
}

public class DamageData : IStatModifierData, IStatModifierDataOf<Damage>
{
    public string StatName => "Hp";
    public string Value { get; set; }
    public ModifierOperationType OperationType { get; set; } = ModifierOperationType.Add;
}