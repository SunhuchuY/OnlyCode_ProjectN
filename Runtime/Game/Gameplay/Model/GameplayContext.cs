using System.Collections.Generic;
using UnityEngine;

public class ActiveSkillContext
{
    public ActiveSkillData Data { get; set; }
    public int SkillLevel { get; set; }
    public Vector3 SkillPosition { get; set; }
    public float RunningTime { get; set; }
    public int NextActionTimesIndex { get; set; }
    public ActionSequenceContext ActionContext { get; set; }
}

public class HitFrameObjectContext
{
    public SkillObjectData Data { get; set; }
    public float Param1 { get; set; }
    public float Param2 { get; set; }
    public float Param3 { get; set; }
    public int NextHitFrameIndex { get; set; }
    public ActionSequenceContext ActionContext { get; set; }
}

public class ActionSequenceContext
{
    public IGameActor Actor { get; set; }
    public List<IGameActor> Targets { get; set; } = new();

    // HitFrameObject의 Action 재생시에는 초기화되지 않으며, 사용하지 말아야 합니다.
    public float ScaleByLevel { get; set; } // levelPer ^ skillLevel
    public Vector3 Position { get; set; }
    public float Param1 { get; set; }
    public float Param2 { get; set; }
    public float Param3 { get; set; }
}

public class GameplayEffectContext
{
    public ActionSequenceContext ActionContext { get; set; }
    public IGameActor Actor { get; set; }
    public IGameActor Target { get; set; }

    public float ScaleByLevel => ActionContext.ScaleByLevel;
    public float Param1 => ActionContext.Param1;
    public float Param2 => ActionContext.Param2;
    public float Param3 => ActionContext.Param3;

    public float DistanceFromInstigator =>
        Vector3.Distance(Actor.Go.transform.position, Target.Go.transform.position);

    public Vector3 DirectionFromInstigator =>
        (Target.Go.transform.position - Actor.Go.transform.position).normalized;
}