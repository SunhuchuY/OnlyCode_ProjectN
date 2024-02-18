using System.Collections.Generic;

public enum SkillObjectType
{
    Projectile,
    Field,
    HitFrameObject
}

public class SkillObjectData
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string PrefabAddress { get; set; }
    public SkillObjectType Type { get; set; }
    public List<IGameplayEffectData> Effects { get; set; } = new();
    public List<float> ManualHitTimes { get; set; } = new();
    public List<List<int>> ActionIndexes { get; set; } = new() { new() { 0 } };
    public List<ActionData> Actions { get; set; } = new();
    public string VfxOnSpawnAddress { get; set; }
    public string VfxOnDespawnAddress { get; set; }
}