using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ObjectPoolManager : MonoBehaviour
{
    private Dictionary<string, IObjectPool<ParticleSystem>> particlesPoolMap = new();

    public void PlayParticle(string _address, Vector3 _position)
    {
        IObjectPool<ParticleSystem> _pool = null;

        bool _poolExisted = particlesPoolMap.TryGetValue(_address, out _pool);
        if (_poolExisted == false)
        {
            CreateParticleSystemPool(_address, out _pool);
            particlesPoolMap.Add(_address, _pool);
        }

        var _particle = _pool.Get();
        _particle.transform.position = _position;
        _particle.Play();
    }

    private void CreateParticleSystemPool(string _address, out IObjectPool<ParticleSystem> _pool)
    {
        var _prefab = Resources.Load<ParticleSystem>(_address);
        GameObject _root = new GameObject($"(t: {nameof(ParticleSystem)}) {_address}");
        _root.transform.SetParent(gameObject.transform);
        _pool = new ObjectPool<ParticleSystem>(
            () =>
            {
                var _ps = Object.Instantiate(_prefab, _root.transform);

                var _mainModule = _ps.main;
                _mainModule.stopAction = ParticleSystemStopAction.Callback;

                var _eventReceiver = _ps.gameObject.AddComponent<ParticleSystemEventReceiver>();
                _eventReceiver.OnParticleSystemStoppedEvent += () => particlesPoolMap[_address].Release(_ps);

                return _ps;
            },
            _ps =>
            {
                _ps.Simulate(0f, true, true); // particle system을 초기화합니다.
                _ps.gameObject.SetActive(true);
            },
            _ps => _ps.gameObject.SetActive(false));
    }
}