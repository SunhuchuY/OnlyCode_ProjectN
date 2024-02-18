public interface IStatModifier
{
    public ModifierOperationType OperationType { get; }
    public float Magnitude { get; }
    public IGameActor Instigator { get; }
}

public class StatModifier : IStatModifier
{
    public ModifierOperationType OperationType { get; set; } = ModifierOperationType.Add;
    public float Magnitude { get; set; }
    public IGameActor Instigator { get; set; }
}

public class Damage : IStatModifier
{
    public ModifierOperationType OperationType { get; set; } = ModifierOperationType.Add;
    public float Magnitude { get; set; }
    public IGameActor Instigator { get; set; }
}