using System.Collections.Generic;
using System.Linq;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

public class FriendActor : MonoBehaviour, IGameActor
{
    public GameObject Go => gameObject;
    public Stats Stats { get; private set; } = new();
    public GameplayEffectController EffectController { get; private set; }
    public TagController TagController { get; private set; } = new();
    public ActiveSkillController SkillController { get; private set; }
    public Animator Anim { get; private set; }
    public ActorType ActorType => ActorType.Friend;

    public FriendData Data;

    protected ObservableStateMachineTrigger animStateMachineTrigger;
    private bool isAttacking = false;

    private void Awake()
    {
        EffectController = gameObject.AddComponent<GameplayEffectController>();
        EffectController.Initialize(this);
        SkillController = gameObject.AddComponent<ActiveSkillController>();
        Anim = gameObject.GetComponentInChildren<Animator>();
        animStateMachineTrigger = Anim.GetBehaviours<ObservableStateMachineTrigger>()[0];
        Stats.AddStat("Attack", new Stat().Initialize());
        Stats.AddStat("AttackRange", new Stat().Initialize());
        Stats.AddStat("ChaseRange", new Stat().Initialize());
        Stats.AddStat("MoveSpeed", new Stat().Initialize());

        animStateMachineTrigger
            .OnStateEnterAsObservable()
            .Where(x => x.StateInfo.IsName("Attack"))
            .Subscribe(x =>
            {
                Debug.Log("Attack Start");
                isAttacking = true;
            });

        animStateMachineTrigger
            .OnStateExitAsObservable()
            .Where(x => x.StateInfo.IsName("Attack"))
            .Subscribe(x =>
            {
                Debug.Log("Attack End");
                isAttacking = false;
            });
    }

    public void Update()
    {
        if (isAttacking)
            return;

        switch (Data.MoveType)
        {
            case FriendMoveType.Static:
                UpdateStaticType();
                break;
            case FriendMoveType.Chase:
                UpdateChaseType();
                break;
        }
    }

    private void UpdateStaticType()
    {
        float _attackRange = Stats["AttackRange"].CurrentValue;
        var _monstersInRange =
            GameManager.Instance.world.Actors.Where(x =>
                x.ActorType == ActorType.Monster
                && Vector3.Distance(x.Go.transform.position, transform.position) <= _attackRange).ToList();

        if (_monstersInRange.Count > 0)
            DoAttack();
    }

    private void UpdateChaseType()
    {
        float _chaseRange = Stats["ChaseRange"].CurrentValue;
        var _monstersInChaseRange =
            GameManager.Instance.world.Actors.Where(x =>
                x.ActorType == ActorType.Monster
                && Vector3.Distance(x.Go.transform.position, transform.position) <= _chaseRange).ToList();

        if (_monstersInChaseRange.Count == 0)
            return;

        var _nearestMonster =
            _monstersInChaseRange
                .OrderBy(x => Vector3.Distance(x.Go.transform.position, transform.position))
                .FirstOrDefault();

        float _attackRange = Stats["AttackRange"].CurrentValue;
        if (Vector3.Distance(_nearestMonster.Go.transform.position, transform.position) <= _attackRange)
        {
            DoAttack();
        }
        else
        {
            DoChase(_nearestMonster);
        }
    }

    private void DoAttack()
    {
        var _skillData = DataTable.ActiveSkillDataTable[Data.AttackSkillId];
        SkillController.Perform(this, _skillData, 1, transform.position);
        Anim.SetTrigger("Attack");
    }

    private void DoChase(IGameActor _target)
    {
        float _moveSpeed = Stats["MoveSpeed"].CurrentValue;
        Vector3 _direction = (_target.Go.transform.position - transform.position).normalized;
        transform.position = transform.position + _direction * _moveSpeed * Time.deltaTime;
    }
}