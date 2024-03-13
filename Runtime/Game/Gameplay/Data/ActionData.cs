using System.Collections.Generic;

public enum PlayVfxRotationType
{
    Identity,
    ForwardToSkillPosition
}

public class ActionData
{
}

public class ApplyEffects : ActionData
{
    public TargetMethodData TargetMethod { get; set; }
    public List<IGameplayEffectData> Effects { get; set; } = new();
    public PlayVfxOnActor VfxOnApplication { get; set; }
    public PlayVfxOnTargets VfxOnTargets { get; set; }
    public string VfxOnTargetAddress { get; set; }
}

public class AddPersistentEffect : ActionData
{
    public int Id { get; set; }
    public TargetMethodData TargetMethod { get; set; }
    public string Duration { get; set; }
    public string Param1 { get; set; }
    public string Param2 { get; set; }
    public string Param3 { get; set; }
    public PlayVfxOnActor VfxOnApplication { get; set; }
    public string VfxOnTargetAddress { get; set; }
}

public class SpawnProjectileObject : ActionData
{
    public int Id { get; set; }
    public float Speed { get; set; } = 1f;
    public bool IsPenetrable { get; set; } = false;
    public string Param1 { get; set; }
    public string Param2 { get; set; }
    public string Param3 { get; set; }
}

public class SpawnHitFrameObject : ActionData
{
    public int Id { get; set; }
    public string Param1 { get; set; }
    public string Param2 { get; set; }
    public string Param3 { get; set; }
}

public class SummonFriend : ActionData
{
    public int Id { get; set; }
    public string Attack { get; set; }
    public string MaxHp { get; set; }
    public string AttackRange { get; set; }
    // friend의 move type이 chase일 때에만 적용됩니다.
    public string ChaseRange { get; set; }
    public string MoveSpeed { get; set; }
}

public class PlayVfxOnActor : ActionData
{
    public string VfxAddress { get; set; }
    public PlayVfxRotationType RotationType { get; set; } = PlayVfxRotationType.Identity;
    public string Scale { get; set; } = "1";
    public float Duration { get; set; } = 0.2f;
    public bool FollowPivot { get; set; } = false;
}

public class PlayVfxOnTargets : ActionData
{
    public string VfxAddress { get; set; }
    public PlayVfxRotationType RotationType { get; set; } = PlayVfxRotationType.Identity;
    public string Scale { get; set; } = "1";
    public float Duration { get; set; } = 0.2f;
}

public class DespawnFriends : ActionData
{
}

public class SelectTarget : ActionData
{
    public TargetMethodData TargetMethod { get; set; }
}