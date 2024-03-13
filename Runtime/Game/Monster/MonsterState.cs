using System;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityHFSM;
using Object = UnityEngine.Object;

public class MonsterState_Move : ActionState
{
    public Monster Owner;

    public MonsterState_Move(bool needsExitTime = false, bool isGhostState = false)
        : base(needsExitTime, isGhostState)
    {
    }

    public override void OnLogic()
    {
        base.OnLogic();

        Vector2 targetPos = Owner.detector.GetCurrentTargetActor().Go.transform.position;
        Vector2 ownerPos = Owner.transform.position;
        //Debug.Log($"distance: {Vector2.Distance(ownerPos, targetPos)}, detectionValue: {Owner.attributes.Detection.CurrentValue}");

        if (Vector2.Distance(ownerPos, targetPos) <= Owner.Stats["AttackRange"].CurrentValue)
        {
            // 타겟대상이 공격 범위 안에 들어오면 공격 상태로 전이합니다.
            Owner.Fsm.RequestStateChange(nameof(MonsterState_Attack));
            return;
        }

        // 플레이어를 향해 움직입니다.
        Vector2 direction = Owner.IsBinding()
            ? Vector2.zero
            : (targetPos - ownerPos).normalized;
        Owner.rigidbody2D.velocity = direction * Owner.Stats["MoveSpeed"].CurrentValue;

        Owner.Anim.SetFloat("MoveSpeed", Owner.rigidbody2D.velocity.magnitude);
    }

    public override void OnExit()
    {
        base.OnExit();

        // 이동을 멈춥니다.
        Owner.rigidbody2D.velocity = Vector2.zero;
        Owner.Anim.SetFloat("MoveSpeed", 0);
    }
}

public class MonsterState_Attack : ActionState
{
    public Monster Owner;
    private bool isEnd = false;

    public MonsterState_Attack(bool needsExitTime = false, bool isGhostState = false)
        : base(needsExitTime, isGhostState)
    {
    }

    public override void OnEnter()
    {
        base.OnEnter();

        if (Owner.IsCanNotAttack()) 
        {
            isEnd = true;
            return;
        }

        Owner.Anim.SetTrigger("Attack");
        isEnd = false;

        // 공격 애니메이션의 종료 사실을 통지받기 위해 이벤트를 등록합니다.
        IDisposable _disposable = null;
        _disposable = Owner.Anim.GetBehaviour<ObservableStateMachineTrigger>()
            .OnStateExitAsObservable()
            .Subscribe(_stateInfo =>
            {
                if (_stateInfo.StateInfo.IsName("Base Layer.Attack"))
                {
                    isEnd = true;
                    _disposable.Dispose();
                }
            });
    }

    public override void OnLogic()
    {
        base.OnLogic();

        if (isEnd)
        {
            // 공격이 끝났으면 이동 상태로 전이합니다.
            Owner.Fsm.RequestStateChange(nameof(MonsterState_Move));
        }
    }
}

public class MonsterState_Dead : ActionState
{
    public Monster Owner;

    public MonsterState_Dead(bool needsExitTime = false, bool isGhostState = false)
        : base(needsExitTime, isGhostState)
    {
    }

    public override void OnEnter()
    {
        base.OnEnter();

        Owner.Anim.SetTrigger("Dead");

        // Dead 애니메이션 재생이 종료될 때 오브젝트를 삭제합니다.
        IDisposable _disposable = null;
        _disposable = Owner.Anim.GetBehaviour<ObservableStateMachineTrigger>()
            .OnStateExitAsObservable()
            .Subscribe(_stateInfo =>
            {
                if (_stateInfo.StateInfo.IsName("Base Layer.Dead"))
                {
                    Object.Destroy(Owner.Go);
                    _disposable.Dispose();
                }
            });
    }
}