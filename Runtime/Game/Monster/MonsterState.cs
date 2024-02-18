using UnityEngine;
using UnityHFSM;

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

        Vector2 targetPos = Owner.detector.GetCurrentTargetTransform().position;
        Vector2 ownerPos = Owner.transform.position;
        //Debug.Log($"distance: {Vector2.Distance(ownerPos, targetPos)}, detectionValue: {Owner.attributes.Detection.CurrentValue}");

        if (Vector2.Distance(ownerPos, targetPos) <= Owner.attributes.Detection)
        {
            // 타겟대상이 공격 범위 안에 들어오면 공격 상태로 전이합니다.
            Owner.Fsm.RequestStateChange(nameof(MonsterState_Attack));
            return;
        }

        // 플레이어를 향해 움직입니다.
        Vector2 direction = (targetPos - ownerPos).normalized;
        Owner.rigidbody2D.velocity = direction * Owner.attributes.MoveSpeed;
    }

    public override void OnExit()
    {
        base.OnExit();

        // 이동을 멈춥니다.
        Owner.rigidbody2D.velocity = Vector2.zero;
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

    private void OnAnimStateExit(AnimatorStateInfo _animStateInfo, int i)
    {
        if (_animStateInfo.IsName("Attack"))
            isEnd = true;
    }

    public override void OnEnter()
    {
        base.OnEnter();

        Owner.Animator.SetTrigger("isAttack");
        isEnd = false;

        // 공격 애니메이션의 종료 사실을 통지받기 위해 이벤트를 등록합니다.
        foreach (var x in Owner.Animator.GetBehaviours<AnimationStateEventReceiver>())
            x.OnStateExitEvent += OnAnimStateExit;
    }

    public override void OnExit()
    {
        base.OnExit();

        foreach (var x in Owner.Animator.GetBehaviours<AnimationStateEventReceiver>())
            x.OnStateExitEvent -= OnAnimStateExit;
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
    private bool isEnd = false;

    public MonsterState_Dead(bool needsExitTime = false, bool isGhostState = false)
        : base(needsExitTime, isGhostState)
    {
    }

    public override void OnEnter()
    {
        base.OnEnter();

        Owner.Animator.SetTrigger("isDead");
        isEnd = false;
    }
}