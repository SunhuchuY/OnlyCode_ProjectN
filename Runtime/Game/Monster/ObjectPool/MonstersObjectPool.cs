using System.Linq;
using UnityEngine;
using UnityEngine.Pool;

using Random = System.Random;

public class MonstersObjectPool : ObjectPoolMapBase<int>
{
    public static MonstersObjectPool Instance;  

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    protected override void CreatePool(int _id, out IObjectPool<GameObject> _pool)
    {
        GameObject _prefab = Resources.Load<GameObject>(StageParser.MonsterPrefabPath + "/" + StageParser.Instance.monsterProfiles[_id].MonsterImageName);
        GameObject _root = new GameObject(_id.ToString());
        _root.transform.parent = parent;

        if (_prefab == null)
        {
            Debug.LogError($"{StageParser.Instance.monsterProfiles[_id].MonsterImageName}의 프리팹은 NULL 상태입니다.");
            Debug.LogError($"{StageParser.Instance.monsterProfiles[_id].MonsterImageName}은 NULL이므로, 다른 Pool로 대체합니다.");
            Random rand = new Random();
            _pool = poolMap.ElementAt(rand.Next(0, poolMap.Count)).Value;
            return;
        }

        _pool = new ObjectPool<GameObject>(
            () =>
            {
                GameObject _go = Object.Instantiate(_prefab, _root.transform);
                _go.SetActive(true);
                return _go;
            });
    }

    public void ActiveAllRelease()
    {
        int monsterCount = parent.transform.childCount;

        for (int i = 0; i < monsterCount; i++)
        {
            Transform temp = parent.transform.GetChild(i);
            int id = int.Parse(temp.gameObject.name);

            for (int j = 0; j < temp.childCount; j++)
            {
                GameObject go = temp.GetChild(j).gameObject;

                if (go.activeSelf)
                {
                    ReleaseGO(id, go);
                }
            }
        }
    }
}