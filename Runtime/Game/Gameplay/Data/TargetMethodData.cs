using System.Collections.Generic;

public enum PositionType
{
    ActorPosition,
    ActionPosition
}

public class TargetMethodData
{
}

// 코드로 직접 대상을 지정하는 방법입니다.
public class SpecificTargetMethod : TargetMethodData
{
    public List<IGameActor> Targets { get; set; } = new();
}

// 가장 마지막으로 지정된 대상을 다시 지정하는 방법입니다.
public class SelectedTargetMethod : TargetMethodData
{
}

public class RadiusRangeTargetMethod : TargetMethodData
{
    public string Radius { get; set; } = "1";
    public string MaxCount { get; set; } = "-1";
    public PositionType PositionType { get; set; } = PositionType.ActorPosition;
}

public class BeamRangeTargetMethod : TargetMethodData
{
    public string Thickness { get; set; } = "1";
    public string MaxCount { get; set; } = "-1";
}

public class GlobalTargetMethod : TargetMethodData
{
}

public class SelfTargetMethod : TargetMethodData
{
}

public class FriendsTargetMethod : TargetMethodData
{
}