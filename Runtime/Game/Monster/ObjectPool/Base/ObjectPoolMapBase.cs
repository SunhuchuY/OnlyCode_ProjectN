using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public abstract class ObjectPoolMapBase<KeyType> : MonoBehaviour
{
    protected Dictionary<KeyType, IObjectPool<GameObject>> poolMap = new();
    [SerializeField] protected Transform parent;

    public GameObject GetGO(KeyType key)
    {
        IObjectPool<GameObject> pool = null;

        if (!poolMap.TryGetValue(key, out pool))
        {
            CreatePool(key, out pool);
            poolMap.Add(key, pool);
        }

        GameObject go = pool.Get();
        go.SetActive(true);
        return go;
    }

    public void ReleaseGO(KeyType key, GameObject go)
    {
        if (poolMap.ContainsKey(key))
        {
            poolMap[key].Release(go);
            go.SetActive(false);
        }
    }

    protected abstract void CreatePool(KeyType key, out IObjectPool<GameObject> pool);
}


public abstract class ObjectPoolBase<ValueType> : MonoBehaviour where ValueType : MonoBehaviour
{
    private IObjectPool<ValueType> pool;
    [SerializeField] protected Transform parent;

    private void Awake()
    {
        
    }


}
