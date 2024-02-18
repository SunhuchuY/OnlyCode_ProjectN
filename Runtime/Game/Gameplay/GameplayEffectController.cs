using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.Assertions;

public class GameplayEffectController : MonoBehaviour
{
    public IReadOnlyReactiveCollection<GameplayPersistentEffect> ActiveEffects => activeEffects;

    private ReactiveCollection<GameplayPersistentEffect> activeEffects = new();
    private IGameActor actor;

    public void Initialize(IGameActor _actor)
    {
        actor = _actor;
    }

    public void AddEffect(IGameplayEffect _effect)
    {
        if (_effect is GameplayPersistentEffect _persistentEffect)
        {
            activeEffects.Add(_persistentEffect);
            ExecuteEffect(_persistentEffect);
        }
        else
        {
            ExecuteEffect(_effect);
        }
    }

    public void RemoveEffect(IGameplayEffect _effect)
    {
        if (_effect is GameplayPersistentEffect _persistentEffect)
        {
            activeEffects.Remove(_persistentEffect);
            GameplayHandlers.GetEffectHandler(_persistentEffect.GetType()).InvokeRemoveHandler(_effect, actor);
        }
        else
        {
            // 일회성으로 적용되는 effect (=attribute에게 적용되는 modifier만을 가지는 effect)는 삭제 명령할 수 없습니다.
            // 이것이 실행된다면 그건 잘못된 동작입니다.
            Assert.IsTrue(true, "일회성으로 적용되는 effect는 삭제 명령할 수 없습니다. 이는 잘못된 동작입니다.");
            throw new NotImplementedException();
        }
    }

    private void ExecuteEffect(IGameplayEffect _effect)
    {
        GameplayHandlers.GetEffectHandler(_effect.GetType()).InvokeExecuteHandler(_effect, actor);
    }

    /// <summary>
    /// persistent effect가 만료되었는지 확인하고, 만료된 경우 해당 effect를 삭제합니다.
    /// </summary>
    private void HandleDuration()
    {
        var _effectsToRemove = new List<GameplayPersistentEffect>();
        foreach (GameplayPersistentEffect _effect in activeEffects)
        {
            _effect.RemainingDuration = Math.Max(_effect.RemainingDuration - Time.deltaTime, 0f);
            if (Mathf.Approximately(_effect.RemainingDuration, 0f))
            {
                // 이 effect의 만료 시간이 다 되어 제거되어야 할 때 실행됩니다.
                _effectsToRemove.Add(_effect);
            }

            if (_effect.Period > 0)
            {
                _effect.RemainingPeriod = Math.Max(_effect.RemainingPeriod - Time.deltaTime, 0f);
                if (Mathf.Approximately(_effect.RemainingPeriod, 0f))
                {
                    // 이 effect가 periodic 속성을 가지고 있고,
                    // 반복 주기의 끝에 왔을 때 실행됩니다.
                    // 다시 effect를 적용시키고, 다음 반복 주기가 시작될 때까지 남은 시간을 초기화합니다.

                    ExecuteEffect(_effect);

                    _effect.RemainingPeriod = _effect.Period;
                }
            }
        }

        foreach (GameplayPersistentEffect _effect in _effectsToRemove)
        {
            RemoveEffect(_effect);
        }
    }

    private void Update()
    {
        HandleDuration();
    }
}