using System.Collections.Generic;

public enum AddForceDirectionType
{
    FromActorToTarget,
    FromTargetToActor,
    FromActionPositionToTarget,
    FromTargetToActionPosition
}

public interface IGameplayEffectData
{
}

public class GameplayEffectData : IGameplayEffectData
{
}

public class StatModify : IGameplayEffectData
{
    public List<IStatModifierData> StatModifiers { get; set; } = new();
}

public class GameplayPersistentEffectData : IGameplayEffectData
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string IconAddress { get; set; }
    public List<IGameplayEffectData> Effects { get; set; } = new();
    public string Period { get; set; } = "-1"; // periodic할 때 작성되어야 합니다.
    public string VfxOnApplicationAddress { get; set; }
}

public class AddForce : IGameplayEffectData
{
    public AddForceDirectionType DirectionType { get; set; } = AddForceDirectionType.FromActorToTarget;
    public string Force { get; set; }
}

public class AddTag : IGameplayEffectData
{
    public List<string> Tags { get; set; } = new();
}