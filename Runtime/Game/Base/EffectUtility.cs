using Cysharp.Threading.Tasks;
using DG.Tweening;
using Spine.Unity;
using Unity.Linq;
using UnityEngine;

public static class EffectUtility
{
    public static Quaternion VectorToQuaternion2D(Vector2 _direction)
    {
        float _angle = Mathf.Atan2(_direction.y, _direction.x) * Mathf.Rad2Deg;
        return Quaternion.AngleAxis(_angle, Vector3.forward);
    }

    public static GameObject Spawn(string _prefabAddress, Vector3 _position, float _scale, Quaternion _rotation)
    {
        var _prefab = Resources.Load<GameObject>(_prefabAddress);
        return Spawn(_prefab, _position, _scale, _rotation);
    }

    public static GameObject Spawn(GameObject _prefab, Vector3 _position, float _scale, Quaternion _rotation)
    {
        var _go = Object.Instantiate(_prefab);
        _go.layer = LayerMask.NameToLayer("Vfx");

        var _goParent = new GameObject(_prefab.name);
        _go.transform.SetParent(_goParent.transform);
        _goParent.transform.SetPositionAndRotation(_position, _rotation);
        _goParent.transform.localScale = Vector3.one * _scale;

        var _ps = _go.GetComponent<ParticleSystemRenderer>();
        if (_ps != null)
        {
            // temp: 임시적으로 모든 vfx를 Vfx 레이어로 설정합니다. 추후에 장판 Vfx는 BackgroundVfx 레이어로 설정되어야 합니다.
            _ps.sortingLayerID = SortingLayer.NameToID("Vfx");
        }

        return _goParent;
    }

    public static void WaitAndDestroy(GameObject _go, float _duration)
    {
        var _spineAnim = _go.GetComponentInChildren<SkeletonAnimation>();
        if (_spineAnim != null)
        {
            UniTask.Create(async () =>
            {
                await UniTask.WaitForSeconds(_duration);
                DOTween.To(() => _spineAnim.skeleton.A, value => { _spineAnim.skeleton.A = value; }, 0f, .1f)
                    .OnComplete(() => _go.Destroy());
            }).Forget();

            return;
        }

        var _ps = _go.GetComponentInChildren<ParticleSystem>();
        if (_ps != null)
        {
            UniTask.Create(async () =>
            {
                await UniTask.WaitForSeconds(_duration);
                _ps.Stop(true);
                await UniTask.WaitForSeconds(2f);
                _go.Destroy();
            }).Forget();

            return;
        }

        UniTask.Create(async () =>
        {
            await UniTask.WaitForSeconds(_duration);
            _go.Destroy();
        }).Forget();
    }
}