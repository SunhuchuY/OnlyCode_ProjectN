using UnityEngine;

public abstract class BaseRangedAbility : MonoBehaviour
{
    protected Monster Owner { get; private set; }
    protected string PrefabName { get; private set; }
    protected Transform FireTf { get; private set; }
    protected bool IsParameterInitialized { get; set; }

    public void BasicInitialize(Monster owner, string prefabName)
    {
        Owner = owner;
        PrefabName = prefabName;

        Transform firePoint = null;
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);
            if (child.name.Contains("FirePoint"))
            {
                firePoint = child;
                break;
            }
        }

        if (firePoint != null)
        {
            FireTf = firePoint;
        }
        else
        {
            Debug.LogWarning("FirePoint가 존재하지 않으므로 해당 몬스터의 포지션을 설정합니다.");
            FireTf = owner.transform;
        }
    }

    public bool IsInitialized()
    {
        if (IsParameterInitialized && Owner != null && PrefabName != null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public abstract void Shoot();
}

