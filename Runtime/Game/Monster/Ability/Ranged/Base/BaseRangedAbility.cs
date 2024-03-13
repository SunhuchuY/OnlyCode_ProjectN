using System.Collections.Generic;
using Unity.Linq;
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

        Transform firePoint = GetFirePoint();
        FireTf = firePoint != null ? firePoint : owner.transform;
    }
        
    private Transform GetFirePoint() 
    {
        // "arm-B-IK"를 우선 탐색
        Transform armBIK = Owner.GetModel().transform.Find("arm-B-IK");
        if (armBIK != null)
        {
            Transform firePoint = armBIK.Find("FirePoint");
            if (firePoint != null)
            {
                return firePoint;
            }
        }

        // "arm-B-IK"에 "FirePoint"가 없는 경우, 전체 모델에서 "FirePoint"를 탐색
        return FindInDescendants(Owner.GetModel().transform, "FirePoint");
    }

    private Transform FindInDescendants(Transform root, string targetName)
    {
        Queue<Transform> queue = new Queue<Transform>();
        queue.Enqueue(root);

        while (queue.Count > 0)
        {
            Transform current = queue.Dequeue();
            if (current.name == targetName)
            {
                return current;
            }

            foreach (Transform child in current)
            {
                queue.Enqueue(child);
            }
        }

        return null; // "FirePoint"를 찾지 못함
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

