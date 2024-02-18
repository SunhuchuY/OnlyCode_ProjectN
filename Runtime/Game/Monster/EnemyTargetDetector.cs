using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTargetDetector : MonoBehaviour
{
    public Monster Owner;
    Transform currentTarget;

    void Start()
    {
        StartCoroutine(DelayFindTarget());
    }

    IEnumerator DelayFindTarget()
    {
        while (true)
        {
            FindTarget();
            yield return new WaitForSeconds(2);
        }
    }

    void FindTarget()
    {
        Collider2D[] targets = Physics2D.OverlapCircleAll(transform.position, Owner.attributes.Detection, 1 << LayerMask.GetMask("Actor"));
        Transform closestTarget = null;
        float closestDistance = float.MaxValue;

        foreach (var target in targets)
        {
            if (target.CompareTag("Friend"))
            {
                float distance = Vector2.Distance(transform.position, target.transform.position);
                if (distance < closestDistance)
                {
                    closestTarget = target.transform;
                    closestDistance = distance;
                }
            }
        }

        currentTarget = closestTarget != null ? closestTarget : GameManager.Instance.player.transform;
    }

    public Transform GetCurrentTargetTransform()
    {
        if (currentTarget == null)
        {
            FindTarget();
        }

        return currentTarget;
    }

    public Collider2D GetCurrentTargetCollider()
    {
        if (currentTarget == null)
        {
            Debug.LogError("현재 target이 null입니다.");
        }

        Collider2D col = null;
        col = currentTarget.GetComponent<Collider2D>();
        if (col == null && currentTarget.parent != null)
        {
            col = currentTarget.parent.GetComponent<Collider2D>();
        }

        if (col == null)
        {
            Debug.LogError("Collider2D not found on object or its parent.");
        }

        return col;
    }

    void OnDrawGizmos()
    {
        // 탐지 범위를 시각화하기 위한 Gizmos
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, Owner.attributes.Detection);
    }
}