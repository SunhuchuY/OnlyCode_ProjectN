using System.Linq;
using Unity.Linq;
using UnityEngine;
using UnityHFSM;

public class PlayerState_Idle : ActionState
{
    public Player Owner;

    public PlayerState_Idle(bool needsExitTime = false, bool isGhostState = false)
        : base(needsExitTime, isGhostState)
    {
    }

    public override void OnLogic()
    {
        base.OnLogic();

        // 자기 자신은 공격 범위 내 액터 검사에 포함하지 않습니다.
        var _collidersInSelf = Owner.gameObject.DescendantsAndSelf().OfComponent<Collider2D>();
        var _originLayer = Owner.gameObject.layer;
        _collidersInSelf.ForEach(x => x.gameObject.layer = 1 << LayerMask.NameToLayer("Ignore Raycast"));

        // 중심점 위치를 기준으로 AttackRange 반경 안에 있는 액터를 검사합니다.
        var _pivot = Owner.gameObject.DescendantsAndSelf().First(x => x.name == "Pivot");

        var _collider = Physics2D.OverlapCircle(
            _pivot.transform.position, Owner.AttackRange.CurrentValue, 1 << LayerMask.NameToLayer("Actor"));
        if (_collider != null && _collider.CompareTag("Monster"))
        {
            GameManager.Instance.bulletController.SetTarget(_collider.transform);
            // 몬스터가 공격 범위 안에 들어오면 공격 상태로 전이합니다.
            Owner.Fsm.RequestStateChange(nameof(PlayerState_Attack));
        }
        else
        {
            GameManager.Instance.bulletController.SetTarget(null);
        }

        _collidersInSelf.ForEach(x => x.gameObject.layer = _originLayer);
    }
}

public class PlayerState_Attack : ActionState
{
    public Player Owner;
    private bool isEnd = false;

    public PlayerState_Attack(bool needsExitTime = false, bool isGhostState = false)
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

        // 공격 애니메이션의 종료 사실을 통지받기 위해 이벤트를 등록합니다.
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
            // 공격이 끝났으면 이동 상태로 전이합니다.
            Owner.Fsm.RequestStateChange(nameof(PlayerState_Idle));
        }
    }
}

public class PlayerState_Dead : ActionState
{
    public Player Owner;
    private bool isEnd = false;

    public PlayerState_Dead(bool needsExitTime = false, bool isGhostState = false)
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