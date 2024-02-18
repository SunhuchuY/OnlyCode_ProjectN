using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public interface IGameplayEffect
{
    public IGameplayEffectData Data { get; }
}

public class GameplayEffect : IGameplayEffect
{
    public IGameplayEffectData Data { get; set; }
}

public class StatModifyEffect : IGameplayEffect
{
    IGameplayEffectData IGameplayEffect.Data => Data;

    public StatModify Data { get; set; }
    public List<IStatModifier> StatModifiers { get; set; } = new();
}

public class GameplayPersistentEffect : IGameplayEffect
{
    IGameplayEffectData IGameplayEffect.Data => Data;

    public new GameplayPersistentEffectData Data { get; set; }
    public List<IGameplayEffect> Effects { get; set; } = new();
    public float RemainingDuration { get; set; }
    public float Period { get; set; } = -1;
    public float RemainingPeriod { get; set; }

    public IObservable<Unit> OnRemoveAsObservable => OnRemoveSubject;
    public Subject<Unit> OnRemoveSubject = new();
}

public class AddForceEffect : IGameplayEffect
{
    IGameplayEffectData IGameplayEffect.Data => Data;

    public new AddForce Data { get; set; }
    public Vector2 Direction { get; set; }
    public float Force { get; set; }
}

public class AddTagEffect : IGameplayEffect
{
    IGameplayEffectData IGameplayEffect.Data => Data;

    public new AddTag Data { get; set; }
}