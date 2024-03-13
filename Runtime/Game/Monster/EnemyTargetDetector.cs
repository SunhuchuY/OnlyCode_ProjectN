using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

public class EnemyTargetDetector : MonoBehaviour
{
    public Monster Owner;
    private IGameActor curTargetActor;

    private void Start()
    {
        StartCoroutine(DelayFindTarget());
    }

    private IEnumerator DelayFindTarget()
    {
        while (true)
        {
            FindTarget();
            yield return new WaitForSeconds(1);
        }
    }

    private void FindTarget()
    {
        // 몬스터를 제외한 대상을 탐색합니다.
        IGameActor friendActor = GameManager.Instance.world.Actors
            .Where(actor => actor.ActorType == ActorType.Friend
                && Vector2.Distance(transform.position, actor.Go.transform.position) < Owner.Stats["AttackRange"].CurrentValue)
            .OrderBy(actor => Vector2.Distance(transform.position, actor.Go.transform.position))
            .FirstOrDefault();

        curTargetActor = friendActor == null 
            ? GameManager.Instance.playerScript 
            : friendActor;
    }

    public IGameActor GetCurrentTargetActor()
    {
        if (!GameManager.Instance.world.Actors.Contains(curTargetActor)) 
        {
            FindTarget();
        }

        return curTargetActor;
    }   
}