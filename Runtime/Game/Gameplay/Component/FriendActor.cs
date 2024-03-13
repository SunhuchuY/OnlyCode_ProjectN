using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.Assertions.Must;

public class FriendActor : MonoBehaviour, IGameActor
{
    public GameObject Go => gameObject;
    private GameObject modelGO;
    public Stats Stats { get; private set; } = new();
    public GameplayEffectController EffectController { get; private set; }
    public TagController TagController { get; private set; } = new();
    public ActiveSkillController SkillController { get; private set; }
    public Animator Anim { get; private set; }
    public ActorType ActorType => ActorType.Friend;

    public FriendData Data;
    private BoxCollider2D collider;
    public Rigidbody2D rigidbody2D;
    protected ObservableStateMachineTrigger animStateMachineTrigger;
    private bool isAttacking = false;
    private bool isDead = false;

    private void Awake()
    {
        Stats.AddStat("Attack", new Stat().Initialize());
        Stats.AddStat("MaxHp", new Stat().Initialize());
        Stats.AddStat("AttackRange", new Stat().Initialize());
        Stats.AddStat("ChaseRange", new Stat().Initialize());
        Stats.AddStat("MoveSpeed", new Stat().Initialize());
        Stats.AddStat("Hp", new Attribute());

        collider = GetComponent<BoxCollider2D>();
        
        EffectController = gameObject.AddComponent<GameplayEffectController>();
        EffectController.Initialize(this);
        
        SkillController = gameObject.AddComponent<ActiveSkillController>();
        
        Anim = gameObject.GetComponentInChildren<Animator>();
        
        animStateMachineTrigger = Anim.GetBehaviours<ObservableStateMachineTrigger>()[0];

        animStateMachineTrigger
            .OnStateEnterAsObservable()
            .Where(x => x.StateInfo.IsName("Attack"))
            .Subscribe(x => isAttacking = true);
        animStateMachineTrigger
            .OnStateExitAsObservable()
            .Where(x => x.StateInfo.IsName("Attack"))
            .Subscribe(x => isAttacking = false);
    }

    private void Start()
    {
        Stats["Hp"].Cap = new(Stats["MaxHp"].CurrentValueInt);
        Stats["Hp"].Initialize();
        Stats["Hp"].OnChangesCurrentValueIntAsObservable.Subscribe((hp) =>
        {
            if (hp <= 0)
            {
                isDead = true;
                collider.enabled = false;
                Anim.SetTrigger("Dead");
            }
        });

        AnimKeyframeEventReceiver animKeyframeEventReceiver = modelGO.GetComponent<AnimKeyframeEventReceiver>();
        animKeyframeEventReceiver.OnDeadEvent += OnDead;
        animKeyframeEventReceiver.OnAttackEvent += OnAttack;
    }

    public void Update()
    {   
        if (isAttacking)
            return;

        if (isDead)
        {
            rigidbody2D.velocity = Vector2.zero;    
            return;
        }

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
        {
            DoAttack();
            UpdateFlip(_monstersInRange[0].Go.transform.position);
        }
    }

    private void UpdateChaseType()
    {
        float _chaseRange = Stats["ChaseRange"].CurrentValue;
        var _monstersInChaseRange =
            GameManager.Instance.world.Actors.Where(x =>
                x.ActorType == ActorType.Monster
                && Vector3.Distance(x.Go.transform.position, transform.position) <= _chaseRange).ToList();

        if (_monstersInChaseRange.Count == 0)
        {
            rigidbody2D.velocity = Vector2.zero;
            Anim.SetFloat("MoveSpeed", rigidbody2D.velocity.magnitude);
            return;
        }

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
            UpdateFlip(_nearestMonster.Go.transform.position);
        }

        Anim.SetFloat("MoveSpeed", rigidbody2D.velocity.magnitude);
    }

    private void DoAttack()
    {
        rigidbody2D.velocity = Vector2.zero;
        Anim.SetTrigger("Attack");
    }

    private void DoChase(IGameActor _target)
    {
        Vector3 _direction = (_target.Go.transform.position - transform.position).normalized;
        rigidbody2D.velocity = _direction * Stats["MoveSpeed"].CurrentValue;
    }

    private void UpdateFlip(Vector3 _targetPosition)
    {
        int toTargetDirection = transform.position.x > _targetPosition.x ? -1 : 1;

        transform.localScale = new Vector2(
            Mathf.Abs(transform.localScale.x) * toTargetDirection,
            transform.localScale.y);
    }

    private void OnAttack() 
    {
        var _skillData = DataTable.ActiveSkillDataTable[Data.AttackSkillId];
        SkillController.Perform(this, _skillData, 1, transform.position);
    }

    private void OnDead() 
    {
        Destroy(gameObject);
    }

    public void SetModel(GameObject _model)
    {
        modelGO = _model;
    }
}