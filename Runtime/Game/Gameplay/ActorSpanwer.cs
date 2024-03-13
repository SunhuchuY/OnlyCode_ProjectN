using System.Linq;
using Unity.Linq;
using UnityEngine;

public static class ActorSpanwer
{
    public static IGameActor SpawnActor(GameActorData _data)
    {
        if (_data is FriendData _friendData)
            return SpawnActor(_friendData);

        if (_data is MonsterProfile _monsterData)
            return SpawnActor(_monsterData);

        return null;
    }

    public static IGameActor SpawnActor(FriendData _data)
    {
        var _go = new GameObject();
        _go.name = $"Friend_{_data.Name}({_go.GetInstanceID()})";
        _go.layer = LayerMask.NameToLayer("Actor");

        var _modelParentGo = new GameObject("Model");
        _modelParentGo.transform.SetParent(_go.transform);

        var _modelPrefab = Resources.Load<GameObject>($"Friend/{_data.Id}/{_data.Id}_Prefab");
        var _modelGo = Object.Instantiate(_modelPrefab, _modelParentGo.transform);

        var _hitPivot = _modelGo.DescendantsAndSelf().FirstOrDefault(x => x.name == _data.HitPivot);
        var _hitPivotGo = new GameObject("HitPivot");
        _hitPivotGo.transform.SetParent(_hitPivot.transform);
        _hitPivotGo.transform.localPosition = Vector3.zero;

        var _damagePivot = _modelGo.DescendantsAndSelf().FirstOrDefault(x => x.name == _data.DamagePivot);
        var _damagePivotGo = new GameObject("DamagePivot");
        _damagePivotGo.transform.SetParent(_damagePivot.transform);
        _damagePivotGo.transform.localPosition = Vector3.zero;

        var _animControllerAsset = Resources.Load<RuntimeAnimatorController>($"Friend/{_data.Id}/{_data.Id}_AC");
        var _animator = _modelGo.GetOrAddComponent<Animator>();
        _animator.runtimeAnimatorController = _animControllerAsset;

        BoxCollider2D _originalCollider = _modelGo.GetComponent<BoxCollider2D>();
        var _collider = _go.AddComponent<BoxCollider2D>();
        _collider.size = _originalCollider.size;
        _collider.offset = _originalCollider.offset;
        _collider.isTrigger = true;
        Object.Destroy(_originalCollider);

        var _rigidbody = _go.AddComponent<Rigidbody2D>();
        _rigidbody.gravityScale = 0f;
        _rigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
        
        var _actor = _go.AddComponent<FriendActor>();
        _actor.Data = _data;
        _actor.rigidbody2D = _rigidbody;
        _actor.SetModel(_modelGo);

        var _animKeyframeEventReceiver = _modelGo.GetOrAddComponent<AnimKeyframeEventReceiver>();

        _go.transform.localScale = Vector3.one * .25f; // temp: 임시적으로 몬스터의 크기를 확 줄입니다. .25f는 네피의 스케일링 값과 같습니다.

        HealthBar _healthBar = InstantiateHealthBar(_actor, _collider, _go.transform);
        _healthBar.SetFillImageColor(Color.blue);

        return _actor;
    }

    public static IGameActor SpawnActor(MonsterProfile _data)
    {
        var _go = new GameObject();
        _go.name = $"Monster_{_data.MonsterImageName}({_go.GetInstanceID()})";
        _go.layer = LayerMask.NameToLayer("Actor");

        var _modelParentGo = new GameObject("Model");
        _modelParentGo.transform.SetParent(_go.transform);

        var _modelPrefab = Resources.Load<GameObject>($"Monster/{_data.Id}/{_data.Id}_Prefab");
        var _modelGo = GameObject.Instantiate(_modelPrefab, _modelParentGo.transform);

        // 몬스터의 경우, 독자적인 스킬 시스템으로 스킬을 사용하기 때문에 HitPivot을 설정하지 않습니다.
        // var _hitPivot = _modelGo.DescendantsAndSelf().FirstOrDefault(x => x.name == _data.HitPivot);
        // var _hitPivotGo = new GameObject("HitPivot");
        // _hitPivotGo.transform.SetParent(_hitPivot.transform);
        // _hitPivotGo.transform.localPosition = Vector3.zero;

        var _damagePivot = _modelGo.DescendantsAndSelf().FirstOrDefault(x => x.name == _data.DamagePivot);
        var _damagePivotGo = new GameObject("DamagePivot");
        _damagePivotGo.transform.SetParent(_damagePivot.transform);
        _damagePivotGo.transform.localPosition = Vector3.zero;

        var _animControllerAssetAddress = string.IsNullOrEmpty(_data.AnimationControllerAddress)
            ? $"Monster/{_data.Id}/{_data.Id}_AC"
            : _data.AnimationControllerAddress;
        var _animControllerAsset = Resources.Load<RuntimeAnimatorController>(_animControllerAssetAddress);
        var _animator = _modelGo.GetOrAddComponent<Animator>();
        _animator.runtimeAnimatorController = _animControllerAsset;

        var _collider = _go.AddComponent<BoxCollider2D>();
        var _colliderInModel = _modelGo.GetComponent<BoxCollider2D>();
        _collider.isTrigger = true;
        _collider.size = _colliderInModel.size;
        _collider.offset = _colliderInModel.offset;
        Object.Destroy(_colliderInModel);

        var _rigidbody = _go.AddComponent<Rigidbody2D>();
        _rigidbody.gravityScale = 0f;
        _rigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;

        var _actor = _go.AddComponent<Monster>();
        _actor.Data = _data;
        _actor.SetModel(_modelGo);

        var _animKeyframeEventReceiver = _modelGo.GetOrAddComponent<AnimKeyframeEventReceiver>();
        _actor.keyframeEventReceiver = _animKeyframeEventReceiver;

        _go.transform.localScale = Vector3.one * .25f; // temp: 임시적으로 몬스터의 크기를 확 줄입니다. .25f는 네피의 스케일링 값과 같습니다.

        HealthBar _healthBar = InstantiateHealthBar(_actor, _collider, _go.transform);
        _healthBar.SetFillImageColor(Color.red);

        return _actor;
    }

    private static HealthBar InstantiateHealthBar(IGameActor _actor, BoxCollider2D _collider, Transform _parentTf) 
    {
        var _healthUIPrefab = Resources.Load<GameObject>("Prefab/UI/MonsterHealthBar");
        var _healthUIGo = GameObject.Instantiate(_healthUIPrefab);
        _healthUIGo.transform.SetParent(_parentTf);
        _healthUIGo.GetComponent<Canvas>().sortingLayerID = SortingLayer.NameToID("WorldUI");
        _healthUIGo.transform.localPosition = Vector2.up *
                                              (_collider.bounds.max.y +
                                               _healthUIGo.GetComponent<RectTransform>().rect.height *
                                               _healthUIGo.transform.localScale.y); // 캐릭터의 머리쪽에 표시될 수 있도록 합니다.

        var _healthUI = _healthUIGo.GetOrAddComponent<HealthBar>();
        _healthUI.SetStats(_actor.Stats);

        return _healthUI;
    }
}