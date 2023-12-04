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

        if (Vector2.Distance(Owner.transform.position, Owner.player.position) <= Owner.AttackRange)
        {
            // �÷��̾ ���� ���� �ȿ� ������ ���� ���·� �����մϴ�.
            Owner.Fsm.RequestStateChange(nameof(MonsterState_Attack));
            return;
        }

        // �÷��̾ ���� �����Դϴ�.
        Vector2 direction = (Owner.player.position - Owner.transform.position).normalized;
        Owner.rigidbody2D.velocity = direction * Owner.moveSpeed;
    }

    public override void OnExit()
    {
        base.OnExit();

        // �̵��� ����ϴ�.
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

        Owner.Anim.SetTrigger("isAttack");
        isEnd = false;

        // ���� �ִϸ��̼��� ���� ����� �����ޱ� ���� �̺�Ʈ�� ����մϴ�.
        foreach (var x in Owner.Anim.GetBehaviours<AnimationStateEventReceiver>())
            x.OnStateExitEvent += OnAnimStateExit;
    }

    public override void OnExit()
    {
        base.OnExit();

        foreach (var x in Owner.Anim.GetBehaviours<AnimationStateEventReceiver>())
            x.OnStateExitEvent -= OnAnimStateExit;
    }

    public override void OnLogic()
    {
        base.OnLogic();

        if (isEnd)
        {
            // ������ �������� �̵� ���·� �����մϴ�.
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

        Owner.Anim.SetTrigger("isDead");
        isEnd = false;
    }
}