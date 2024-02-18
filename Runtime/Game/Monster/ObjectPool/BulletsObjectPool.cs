using UnityEngine;
using UnityEngine.Pool;

public class BulletsObjectPool : ObjectPoolMapBase<string>
{
    private const string BulletPath = "Bullet";
    public static BulletsObjectPool Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    protected override void CreatePool(string name, out IObjectPool<GameObject> _pool)
    {
        GameObject _prefab = Resources.Load<GameObject>(BulletPath + "/" + name);
        GameObject _root = new GameObject(name);
        _root.transform.parent = parent;

        _pool = new ObjectPool<GameObject>(
            () =>
            {
                GameObject _go = Object.Instantiate(_prefab, _root.transform);
                _go.SetActive(true);
                return _go;
            });
    }
}