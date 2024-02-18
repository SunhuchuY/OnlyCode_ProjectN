using Cysharp.Threading.Tasks;
using DG.Tweening;
using Spine.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UniRx;
using UniRx.Triggers;
using Unity.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

public static class GameplayHandlers
{
    private static Dictionary<Type, IActionHandler> actionHandlers;
    private static Dictionary<Type, IEffectHandler> effectHandlers;
    private static Dictionary<Type, ITargetMethodHandler> targetMethodHandlers;
    private static Dictionary<Type, IStatModifierHandler> statModifierHandlers;

    public static void Initialize()
    {
        actionHandlers = new();
        effectHandlers = new();
        targetMethodHandlers = new();
        statModifierHandlers = new();

        var _actionHandlerType = typeof(ActionHandler<>);
        Assembly.GetExecutingAssembly().GetTypes()
            .Where(x => x.BaseType != null
                        && x.BaseType.IsGenericType
                        && x.BaseType.GetGenericTypeDefinition() == _actionHandlerType)
            .ToList().ForEach(x =>
                actionHandlers.Add(x.BaseType.GetGenericArguments()[0], Activator.CreateInstance(x) as IActionHandler));

        var _effectHandlerType = typeof(EffectHandler<,>);
        Assembly.GetExecutingAssembly().GetTypes()
            .Where(x => x.BaseType != null
                        && x.BaseType.IsGenericType
                        && x.BaseType.GetGenericTypeDefinition() == _effectHandlerType)
            .ToList().ForEach(x =>
            {
                effectHandlers.Add(x.BaseType.GetGenericArguments()[0],
                    Activator.CreateInstance(x) as IEffectHandler);
                effectHandlers.Add(x.BaseType.GetGenericArguments()[1],
                    Activator.CreateInstance(x) as IEffectHandler);
            });

        var _targetMethodHandlerType = typeof(TargetMethodHandler<>);
        Assembly.GetExecutingAssembly().GetTypes()
            .Where(x => x.BaseType != null
                        && x.BaseType.IsGenericType
                        && x.BaseType.GetGenericTypeDefinition() == _targetMethodHandlerType)
            .ToList().ForEach(x =>
                targetMethodHandlers.Add(x.BaseType.GetGenericArguments()[0],
                    Activator.CreateInstance(x) as ITargetMethodHandler));

        var _statModifierHandlerType = typeof(StatModifierHandler<,>);
        typeof(GameplayHandlers).GetNestedTypes()
            .Where(x => x.BaseType != null
                        && x.BaseType.IsGenericType
                        && x.BaseType.GetGenericTypeDefinition() == _statModifierHandlerType)
            .ToList().ForEach(x =>
                statModifierHandlers.Add(x.BaseType.GetGenericArguments()[0],
                    Activator.CreateInstance(x) as IStatModifierHandler));
    }

    public static IActionHandler GetActionHandler(Type type) =>
        actionHandlers.TryGetValue(type, out var handler) ? handler : null;

    public static IEffectHandler GetEffectHandler(Type type) =>
        effectHandlers.TryGetValue(type, out var handler) ? handler : null;

    public static ITargetMethodHandler GetTargetMethodHandler(Type type) =>
        targetMethodHandlers.TryGetValue(type, out var handler) ? handler : null;

    public static IStatModifierHandler GetStatModifierHandler(Type type) =>
        statModifierHandlers.TryGetValue(type, out var handler) ? handler : null;

    #region Action

    public interface IActionHandler
    {
        void Invoke(ActionData _data, ActionSequenceContext _context);
    }

    public abstract class ActionHandler<T> : IActionHandler where T : ActionData
    {
        public abstract void Invoke(T _data, ActionSequenceContext _context);

        public void Invoke(ActionData _data, ActionSequenceContext _context) =>
            Invoke(_data as T, _context);
    }

    public class ApplyEffectsActionHandler : ActionHandler<ApplyEffects>
    {
        public override void Invoke(ApplyEffects _data, ActionSequenceContext _context)
        {
            Debug.Log("ApplyEffects performed!");
            var _targets = GetTargetMethodHandler(_data.TargetMethod.GetType()).Invoke(_data.TargetMethod, _context);

            GameObject _vfxOnTargetPrefab =
                string.IsNullOrWhiteSpace(_data.VfxOnTargetAddress)
                    ? null
                    : Resources.Load<GameObject>(_data.VfxOnTargetAddress);

            _data.Effects.ForEach(data =>
            {
                _targets.ForEach(target =>
                {
                    var _effectContext = new GameplayEffectContext()
                    {
                        ActionContext = _context, Actor = _context.Actor, Target = target
                    };
                    var _effect = GetEffectHandler(data.GetType()).InvokeDataHandler(data, _effectContext);
                    target.EffectController.AddEffect(_effect);
                });
            });

            if (_data.VfxOnApplication != null)
            {
                new PlayVfxOnActorActionHandler().Invoke(_data.VfxOnApplication, _context);
            }

            if (_vfxOnTargetPrefab != null)
            {
                _targets.ForEach(x =>
                    Object.Instantiate(_vfxOnTargetPrefab, x.Go.transform.position, Quaternion.identity));
            }
        }
    }

    public class ApplyPersistentEffectActionHandler : ActionHandler<AddPersistentEffect>
    {
        public override void Invoke(AddPersistentEffect _data, ActionSequenceContext _context)
        {
            Debug.Log("Add Persistent Effect Performed!");
            var _effectData = DataTable.PersistentEffectDataTable[_data.Id];
            var _targets = GetTargetMethodHandler(_data.TargetMethod.GetType()).Invoke(_data.TargetMethod, _context);
            float _duration = ExpressionEvaluator.Evaluate<float>(_data.Duration, _context);

            GameObject _vfxOnTargetPrefab =
                string.IsNullOrWhiteSpace(_data.VfxOnTargetAddress)
                    ? null
                    : Resources.Load<GameObject>(_data.VfxOnTargetAddress);

            _context.Param1 = string.IsNullOrWhiteSpace(_data.Param1) == false
                ? ExpressionEvaluator.Evaluate<float>(_data.Param1, _context)
                : 0f;
            _context.Param2 = string.IsNullOrWhiteSpace(_data.Param2) == false
                ? ExpressionEvaluator.Evaluate<float>(_data.Param2, _context)
                : 0f;
            _context.Param3 = string.IsNullOrWhiteSpace(_data.Param3) == false
                ? ExpressionEvaluator.Evaluate<float>(_data.Param3, _context)
                : 0f;

            _targets.ForEach(target =>
            {
                var _effectContext = new GameplayEffectContext()
                {
                    ActionContext = _context, Actor = _context.Actor, Target = target
                };
                var _effect =
                    GetEffectHandler(_effectData.GetType()).InvokeDataHandler(_effectData, _effectContext) as
                        GameplayPersistentEffect;
                _effect.RemainingDuration = _duration;
                target.EffectController.AddEffect(_effect);
            });

            if (_data.VfxOnApplication != null)
            {
                new PlayVfxOnActorActionHandler().Invoke(_data.VfxOnApplication, _context);
            }

            if (_vfxOnTargetPrefab != null)
            {
                _targets.ForEach(x =>
                    Object.Instantiate(_vfxOnTargetPrefab, x.Go.transform.position, Quaternion.identity));
            }
        }
    }

    public class SpawnProjectileObjectActionHandler : ActionHandler<SpawnProjectileObject>
    {
        public override void Invoke(SpawnProjectileObject _data, ActionSequenceContext _context)
        {
            Debug.Log("Spawn Projectile Object Performed!");
            var _objectData = DataTable.SkillObjectDataTable[_data.Id];
            var _prefab = Resources.Load<GameObject>(_objectData.PrefabAddress);
            var _vfxOnSpawnPrefab =
                string.IsNullOrWhiteSpace(_objectData.VfxOnSpawnAddress)
                    ? null
                    : Resources.Load<GameObject>(_objectData.VfxOnSpawnAddress);
            var _vfxOnDespawnPrefab =
                string.IsNullOrWhiteSpace(_objectData.VfxOnDespawnAddress)
                    ? null
                    : Resources.Load<GameObject>(_objectData.VfxOnDespawnAddress);

            _context.Param1 = string.IsNullOrWhiteSpace(_data.Param1) == false
                ? ExpressionEvaluator.Evaluate<float>(_data.Param1, _context)
                : 0f;
            _context.Param2 = string.IsNullOrWhiteSpace(_data.Param2) == false
                ? ExpressionEvaluator.Evaluate<float>(_data.Param2, _context)
                : 0f;
            _context.Param3 = string.IsNullOrWhiteSpace(_data.Param3) == false
                ? ExpressionEvaluator.Evaluate<float>(_data.Param3, _context)
                : 0f;

            Vector3 _position = _context.Actor.Go.transform.position;
            Vector3 _direction = (_context.Position - _context.Actor.Go.transform.position).normalized;
            float _angle = Mathf.Atan2(_direction.y, _direction.x) * Mathf.Rad2Deg;
            Quaternion _rotation = Quaternion.AngleAxis(_angle, Vector3.forward);
            var _go = Object.Instantiate(_prefab, _position, _rotation);

            if (_vfxOnSpawnPrefab != null)
                Object.Instantiate(_vfxOnSpawnPrefab, _position, _rotation);

            _go.AddComponent<ObservableUpdateTrigger>().UpdateAsObservable().Subscribe(_ =>
            {
                // 발사체를 매 프레임 앞으로 전진시킵니다.
                _go.transform.position = _go.transform.position + _go.transform.right * _data.Speed * Time.deltaTime;
            });
            _go.OnTriggerEnter2DAsObservable().Subscribe(collider =>
            {
                var _actor = collider.GetComponent<IGameActor>();
                if (_actor == null)
                    return;

                ActorType _targetType =
                    _context.Actor.ActorType == ActorType.Monster ? ActorType.Player : ActorType.Monster;
                if (_actor.ActorType != _targetType)
                    return;

                // 발사체가 적에게 닿았을 때, effect를 적용합니다.
                _objectData.Effects.ForEach(x =>
                {
                    var _effectContext = new GameplayEffectContext()
                    {
                        ActionContext = _context, Actor = _context.Actor, Target = _actor
                    };
                    var _effect = GetEffectHandler(x.GetType()).InvokeDataHandler(x, _effectContext);
                    _actor.EffectController.AddEffect(_effect);
                });

                if (_data.IsPenetrable == false)
                {
                    _go.Destroy();
                    if (_vfxOnDespawnPrefab != null)
                        Object.Instantiate(_vfxOnDespawnPrefab, _go.transform.position, Quaternion.identity);
                }
            });

            if (_data.IsPenetrable)
            {
                // 관통하는 총알이라면, 2초 뒤 자동으로 파괴됩니다.
                UniTask.Create(async () =>
                {
                    await UniTask.WaitForSeconds(2);
                    _go.Destroy();
                    if (_vfxOnDespawnPrefab != null)
                        Object.Instantiate(_vfxOnDespawnPrefab, _go.transform.position, Quaternion.identity);
                }).Forget();
            }
        }
    }

    public class SpawnHitFrameObjectActionHandler : ActionHandler<SpawnHitFrameObject>
    {
        public override void Invoke(SpawnHitFrameObject _data, ActionSequenceContext _context)
        {
            Debug.Log("Spawn Hit Frame Object Performed!");

            var _objectData = DataTable.SkillObjectDataTable[_data.Id];
            var _prefab = Resources.Load<GameObject>(_objectData.PrefabAddress);

            float _param1 = string.IsNullOrWhiteSpace(_data.Param1) == false
                ? ExpressionEvaluator.Evaluate<float>(_data.Param1, _context)
                : 0f;
            float _param2 = string.IsNullOrWhiteSpace(_data.Param2) == false
                ? ExpressionEvaluator.Evaluate<float>(_data.Param2, _context)
                : 0f;
            float _param3 = string.IsNullOrWhiteSpace(_data.Param3) == false
                ? ExpressionEvaluator.Evaluate<float>(_data.Param3, _context)
                : 0f;
            HitFrameObjectContext _objContext = new()
            {
                Data = _objectData,
                NextHitFrameIndex = 0,
                ActionContext = new()
                {
                    Actor = _context.Actor,
                    Position = _context.Position,
                    Param1 = _param1,
                    Param2 = _param2,
                    Param3 = _param3
                },
                Param1 = _param1,
                Param2 = _param2,
                Param3 = _param3
            };

            var _go = Object.Instantiate(_prefab, _context.Position, Quaternion.identity);
            var _hfo = _go.AddComponent<HitFrameObject>();
            _hfo.Context = _objContext;
        }
    }

    public class SummonFriendActionHandler : ActionHandler<SummonFriend>
    {
        public override void Invoke(SummonFriend _data, ActionSequenceContext _context)
        {
            var _friendData = DataTable.FriendDataTable[_data.Id];
            var _friendPrefab = Resources.Load<GameObject>($"Friend/{_data.Id}/{_data.Id}_Prefab");
            var _go = Object.Instantiate(_friendPrefab, _context.Position, Quaternion.identity);
            var _goActor = _go.GetComponent<FriendActor>();
            _goActor.Data = _friendData;

            var _statModifiers = new List<IStatModifierData>()
            {
                new StatModifierData()
                {
                    StatName = "Attack", OperationType = ModifierOperationType.Add, Value = _data.Attack
                },
                new StatModifierData()
                {
                    StatName = "AttackRange",
                    OperationType = ModifierOperationType.Add,
                    Value = _data.AttackRange
                }
            };

            if (_friendData.MoveType == FriendMoveType.Chase)
            {
                _statModifiers.AddRange(new[]
                {
                    new StatModifierData()
                    {
                        StatName = "ChaseRange",
                        OperationType = ModifierOperationType.Add,
                        Value = _data.ChaseRange
                    },
                    new StatModifierData()
                    {
                        StatName = "MoveSpeed",
                        OperationType = ModifierOperationType.Add,
                        Value = _data.MoveSpeed
                    }
                });
            }

            var _effectData = new GameplayPersistentEffectData()
            {
                Effects = { new StatModify() { StatModifiers = _statModifiers } }
            };

            var _effectContext = new GameplayEffectContext()
            {
                ActionContext = _context, Actor = _context.Actor, Target = _goActor
            };
            var _effect = GetEffectHandler(_effectData.GetType()).InvokeDataHandler(_effectData, _effectContext);

            GetEffectHandler(_effect.GetType()).InvokeExecuteHandler(_effect, _goActor);

            // world에 추가합니다.
            GameManager.Instance.world.AddActor(_goActor);
        }
    }

    public class PlayVfxOnActorActionHandler : ActionHandler<PlayVfxOnActor>
    {
        public override void Invoke(PlayVfxOnActor _data, ActionSequenceContext _context)
        {
            var _prefab = Resources.Load<GameObject>(_data.VfxAddress);
            float _scale = ExpressionEvaluator.Evaluate<float>(_data.Scale, _context);
            Quaternion _rotation = Quaternion.identity;

            switch (_data.RotationType)
            {
                case PlayVfxRotationType.Identity:
                    _rotation = Quaternion.identity;
                    break;
                case PlayVfxRotationType.ForwardToSkillPosition:
                    Vector3 _direction = (_context.Position - _context.Actor.Go.transform.position).normalized;
                    float _angle = Mathf.Atan2(_direction.y, _direction.x) * Mathf.Rad2Deg;
                    _rotation = Quaternion.AngleAxis(_angle, Vector3.forward);
                    break;
            }

            var _go = Object.Instantiate(_prefab, _context.Actor.Go.transform.position, _rotation);
            _go.transform.localScale = Vector3.one * _scale;
            var _spineAnim = _go.GetComponentInChildren<SkeletonAnimation>();
            UniTask.Create(async () =>
            {
                await UniTask.WaitForSeconds(0.2f);
                DOTween.To(() => _spineAnim.skeleton.A, value => { _spineAnim.skeleton.A = value; }, 0f, .1f)
                    .OnComplete(() => Object.Destroy(_go));
            }).Forget();
        }
    }

    public class DespawnFriendsActionHandler : ActionHandler<DespawnFriends>
    {
        public override void Invoke(DespawnFriends _data, ActionSequenceContext _context)
        {
            var _friends =
                GameManager.Instance.world.Actors.Where(x => x.ActorType == ActorType.Friend).ToList();
            foreach (var x in _friends)
                GameManager.Instance.world.DespawnActor(x);
        }
    }

    public class SelectTargetActionHandler : ActionHandler<SelectTarget>
    {
        public override void Invoke(SelectTarget _data, ActionSequenceContext _context)
        {
            var _targets =
                GetTargetMethodHandler(_data.TargetMethod.GetType()).Invoke(_data.TargetMethod, _context);
            _context.Targets = _targets;
        }
    }

    #endregion

    #region Effect

    public interface IEffectHandler
    {
        IGameplayEffect InvokeDataHandler(IGameplayEffectData _data, GameplayEffectContext _context);
        void InvokeExecuteHandler(IGameplayEffect _effect, IGameActor _target);
        void InvokeRemoveHandler(IGameplayEffect _effect, IGameActor _target);
    }

    public abstract class EffectHandler<TData, T> : IEffectHandler
        where TData : IGameplayEffectData
        where T : class, IGameplayEffect
    {
        public abstract IGameplayEffect InvokeDataHandler_Impl(TData _data, GameplayEffectContext _context);
        public abstract void InvokeExecuteHandler_Impl(T _effect, IGameActor _target);
        public abstract void InvokeRemoveHandler_Impl(T _effect, IGameActor _target);

        public IGameplayEffect InvokeDataHandler(IGameplayEffectData _data, GameplayEffectContext _context) =>
            InvokeDataHandler_Impl((TData)_data, _context);

        public void InvokeExecuteHandler(IGameplayEffect _effect, IGameActor _target) =>
            InvokeExecuteHandler_Impl(_effect as T, _target);

        public void InvokeRemoveHandler(IGameplayEffect _effect, IGameActor _target) =>
            InvokeRemoveHandler_Impl(_effect as T, _target);
    }

    public class StatModifyEffectHandler : EffectHandler<StatModify, StatModifyEffect>
    {
        public override IGameplayEffect InvokeDataHandler_Impl(StatModify _data, GameplayEffectContext _context)
        {
            var _modifiers =
                _data.StatModifiers
                    .Select(x => GetStatModifierHandler(x.GetType()).Invoke(x, _context))
                    .ToList();
            var _effect = new StatModifyEffect() { Data = _data, StatModifiers = _modifiers };
            return _effect;
        }

        public override void InvokeExecuteHandler_Impl(StatModifyEffect _effect, IGameActor _target)
        {
            for (int i = 0; i < _effect.StatModifiers.Count; i++)
            {
                var _stat = _target.Stats[_effect.Data.StatModifiers[i].StatName];
                var _modifier = _effect.StatModifiers[i];
                _stat.ApplyModifier(_modifier);
            }
        }

        public override void InvokeRemoveHandler_Impl(StatModifyEffect _effect, IGameActor _target)
        {
            for (int i = 0; i < _effect.StatModifiers.Count; i++)
            {
                var _stat = _target.Stats[_effect.Data.StatModifiers[i].StatName];
                var _modifier = _effect.StatModifiers[i];
                _stat.RemoveModifier(_modifier);
            }
        }
    }

    public class PersistentEffectHandler : EffectHandler<GameplayPersistentEffectData, GameplayPersistentEffect>
    {
        public override IGameplayEffect InvokeDataHandler_Impl(
            GameplayPersistentEffectData _data,
            GameplayEffectContext _context)
        {
            var _effects = _data.Effects
                .Select(x => GameplayHandlers.GetEffectHandler(x.GetType()).InvokeDataHandler(x, _context))
                .ToList();
            float _period = ExpressionEvaluator.Evaluate<float>(_data.Period, _context);
            var _effect = new GameplayPersistentEffect()
            {
                Data = _data, Effects = _effects, Period = _period, RemainingPeriod = _period
            };
            return _effect;
        }

        public override void InvokeExecuteHandler_Impl(GameplayPersistentEffect _effect, IGameActor _target)
        {
            _effect.Effects.ForEach(x => GetEffectHandler(x.GetType()).InvokeExecuteHandler(x, _target));

            GameObject _vfxOnApplicationPrefab =
                string.IsNullOrWhiteSpace(_effect.Data.VfxOnApplicationAddress)
                    ? null
                    : Resources.Load<GameObject>(_effect.Data.VfxOnApplicationAddress);

            if (_vfxOnApplicationPrefab != null)
            {
                var _go = Object.Instantiate(
                    _vfxOnApplicationPrefab,
                    _target.Go.transform.position, Quaternion.identity);
                _go.transform.SetParent(_target.Go.transform);
                _effect.OnRemoveAsObservable
                    .Subscribe(_ =>
                    {
                        var _spineAnim = _go.GetComponentInChildren<SkeletonAnimation>();
                        UniTask.Create(async () =>
                        {
                            await UniTask.WaitForSeconds(0.2f);
                            DOTween.To(() => _spineAnim.skeleton.A, value => { _spineAnim.skeleton.A = value; }, 0f,
                                    .1f)
                                .OnComplete(() => Object.Destroy(_go));
                        }).Forget();
                    })
                    .AddTo(_target.Go);
            }
        }

        public override void InvokeRemoveHandler_Impl(GameplayPersistentEffect _effect, IGameActor _target)
        {
            _effect.Effects.ForEach(x => GetEffectHandler(x.GetType()).InvokeRemoveHandler(x, _target));
            _effect.OnRemoveSubject.OnNext(Unit.Default);
        }
    }

    public class AddForceEffectHandler : EffectHandler<AddForce, AddForceEffect>
    {
        public override IGameplayEffect InvokeDataHandler_Impl(AddForce _data, GameplayEffectContext _context)
        {
            float _force = ExpressionEvaluator.Evaluate<float>(_data.Force, _context);
            Vector2 _direction = Vector3.zero;
            switch (_data.DirectionType)
            {
                case AddForceDirectionType.FromActorToTarget:
                    _direction =
                        (_context.Target.Go.transform.position - _context.Actor.Go.transform.position).normalized;
                    break;
                case AddForceDirectionType.FromTargetToActor:
                    _direction =
                        (_context.Actor.Go.transform.position - _context.Target.Go.transform.position).normalized;
                    break;
                case AddForceDirectionType.FromActionPositionToTarget:
                    _direction =
                        (_context.Target.Go.transform.position - _context.ActionContext.Position).normalized;
                    break;
                case AddForceDirectionType.FromTargetToActionPosition:
                    _direction =
                        (_context.ActionContext.Position - _context.Target.Go.transform.position).normalized;
                    break;
            }

            var _effect = new AddForceEffect() { Data = _data, Direction = _direction, Force = _force };
            return _effect;
        }

        public override void InvokeExecuteHandler_Impl(AddForceEffect _effect, IGameActor _target)
        {
            var _movement = _target.Go.GetComponent<ActorMovement>();
            if (_movement != null)
            {
                _movement.AddKnockBack(_effect.Direction * _effect.Force * 5);
                return;
            }

            var _rb = _target.Go.GetComponent<Rigidbody2D>();
            if (_rb != null)
            {
                _rb.AddForce(_effect.Direction * _effect.Force, ForceMode2D.Impulse);
            }
        }

        public override void InvokeRemoveHandler_Impl(AddForceEffect _effect, IGameActor _target)
        {
        }
    }

    public class AddTagEffectHandler : EffectHandler<AddTag, AddTagEffect>
    {
        public override IGameplayEffect InvokeDataHandler_Impl(AddTag _data, GameplayEffectContext _context)
        {
            return new AddTagEffect() { Data = _data };
        }

        public override void InvokeExecuteHandler_Impl(AddTagEffect _effect, IGameActor _target)
        {
            _effect.Data.Tags.ForEach(x => _target.TagController.AddTag(x));
        }

        public override void InvokeRemoveHandler_Impl(AddTagEffect _effect, IGameActor _target)
        {
            _effect.Data.Tags.ForEach(x => _target.TagController.RemoveTag(x));
        }
    }

    #endregion

    #region Target Method

    public interface ITargetMethodHandler
    {
        List<IGameActor> Invoke(TargetMethodData _data, ActionSequenceContext _context);
    }

    public abstract class TargetMethodHandler<T> : ITargetMethodHandler where T : TargetMethodData
    {
        public abstract List<IGameActor> Invoke_Impl(T _data, ActionSequenceContext _context);

        public List<IGameActor> Invoke(TargetMethodData _data, ActionSequenceContext _context) =>
            Invoke_Impl(_data as T, _context);
    }

    public class SpecificTargetMethodHandler : TargetMethodHandler<SpecificTargetMethod>
    {
        public override List<IGameActor> Invoke_Impl(SpecificTargetMethod _data, ActionSequenceContext _context)
            => _data.Targets;
    }

    public class SelectedTargetMethodHandler : TargetMethodHandler<SelectedTargetMethod>
    {
        public override List<IGameActor> Invoke_Impl(SelectedTargetMethod _data, ActionSequenceContext _context)
            => _context.Targets;
    }

    public class RadiusTargetMethodHandler : TargetMethodHandler<RadiusRangeTargetMethod>
    {
        public override List<IGameActor> Invoke_Impl(RadiusRangeTargetMethod _data, ActionSequenceContext _context)
        {
            ActorType _targetType =
                _context.Actor.ActorType == ActorType.Monster ? ActorType.Player : ActorType.Monster;
            Vector2 _position = Vector2.zero;
            switch (_data.PositionType)
            {
                case PositionType.ActorPosition:
                    _position = _context.Actor.Go.transform.position;
                    break;
                case PositionType.ActionPosition:
                    _position = _context.Position;
                    break;
            }

            float _radius = ExpressionEvaluator.Evaluate<float>(_data.Radius, _context);
            int _maxCount = ExpressionEvaluator.Evaluate<int>(_data.MaxCount, _context);
            var _actors =
                GameManager.Instance.world.Actors
                    .Where(x =>
                        x.ActorType == _targetType
                        && Vector3.Distance(x.Go.transform.position, _position) <= _radius);

            if (_maxCount < 0)
                return _actors.ToList();

            return _actors.Take(_maxCount).ToList();
        }
    }

    public class BeamRangeTargetMethodHandler : TargetMethodHandler<BeamRangeTargetMethod>
    {
        public override List<IGameActor> Invoke_Impl(BeamRangeTargetMethod _data, ActionSequenceContext _context)
        {
            float _thickness = ExpressionEvaluator.Evaluate<float>(_data.Thickness, _context);
            int _maxCount = ExpressionEvaluator.Evaluate<int>(_data.MaxCount, _context);

            ActorType _targetType =
                _context.Actor.ActorType == ActorType.Monster ? ActorType.Player : ActorType.Monster;

            float _length = 10f;
            Vector3 _direction = (_context.Position - _context.Actor.Go.transform.position).normalized;
            float _angleRad = Mathf.Atan2(_direction.y, _direction.x);
            Vector2 _center = new Vector2(Mathf.Cos(_angleRad) * _length / 2, Mathf.Sin(_angleRad) * _length / 2);
            Vector2 _size = new Vector2(_length, 1);

            var _colliders = Physics2D.OverlapBoxAll(_center, _size, _angleRad * Mathf.Rad2Deg);
            var _actors =
                _colliders
                    .Select(x => x.GetComponent<IGameActor>())
                    .Where(x => x != null && x.ActorType == _targetType);

            if (_maxCount < 0)
                return _actors.ToList();

            return _actors.Take(_maxCount).ToList();
        }
    }

    public class GlobalTargetMethodHandler : TargetMethodHandler<GlobalTargetMethod>
    {
        public override List<IGameActor> Invoke_Impl(GlobalTargetMethod _data, ActionSequenceContext _context)
        {
            ActorType _targetType =
                _context.Actor.ActorType == ActorType.Monster ? ActorType.Player : ActorType.Monster;
            var _actors =
                GameManager.Instance.world.Actors
                    .Where(x => x.ActorType == _targetType);

            return _actors.ToList();
        }
    }

    public class SelfTargetMethodHandler : TargetMethodHandler<SelfTargetMethod>
    {
        public override List<IGameActor> Invoke_Impl(SelfTargetMethod _data, ActionSequenceContext _context)
        {
            return new() { _context.Actor };
        }
    }

    public class FriendsTargetMethodHandler : TargetMethodHandler<FriendsTargetMethod>
    {
        public override List<IGameActor> Invoke_Impl(FriendsTargetMethod _data, ActionSequenceContext _context)
        {
            return GameManager.Instance.world.Actors.Where(x => x.ActorType == ActorType.Friend).ToList();
        }
    }

    #endregion

    #region Stat Modifier

    public interface IStatModifierHandler
    {
        IStatModifier Invoke(IStatModifierData _data, GameplayEffectContext _context);
    }

    public abstract class StatModifierHandler<TData, T> : IStatModifierHandler
        where TData : IStatModifierData
        where T : class, IStatModifier
    {
        public abstract T Invoke_Impl(TData _data, GameplayEffectContext _context);

        public IStatModifier Invoke(IStatModifierData _data, GameplayEffectContext _context)
            => Invoke_Impl((TData)_data, _context);
    }

    public class StatModifierHandler
        : StatModifierHandler<StatModifierData, StatModifier>
    {
        public override StatModifier Invoke_Impl(StatModifierData _data, GameplayEffectContext _context)
        {
            float _magnitude = ExpressionEvaluator.Evaluate<float>(_data.Value, _context);
            return new StatModifier()
            {
                Magnitude = _magnitude,
                OperationType = _data.OperationType,
                Instigator = _context != null ? _context.Actor : null
            };
        }
    }

    public class DamageHandler
        : StatModifierHandler<DamageData, Damage>
    {
        public override Damage Invoke_Impl(DamageData _data, GameplayEffectContext _context)
        {
            float _magnitude = ExpressionEvaluator.Evaluate<float>(_data.Value, _context);
            return new Damage()
            {
                Magnitude = _magnitude, OperationType = _data.OperationType, Instigator = _context.Actor
            };
        }
    }

    #endregion
}