using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using Unity.Linq;
using UnityEngine;
using UnityHFSM;

public class Monster : MonoBehaviour, IGameActor
{
    public GameObject Go => gameObject;
    public Stats Stats { get; private set; } = new Stats();
    public GameplayEffectController EffectController { get; set; }
    public TagController TagController { get; private set; } = new();
    public ActiveSkillController SkillController { get; private set; }
    public Animator Anim { get; private set; }
    public ActorType ActorType => ActorType.Monster;

    public MonsterProfile Data;

    public Rigidbody2D rigidbody2D { get; private set; }
    public Collider2D collider { get; private set; }
    public StateMachine Fsm { get; private set; }
    private GameObject model;

    private Transform firePoint;
    float originalmoveSpeed;

    // knock back
    private float knockbackForce = 7f;
    private float knockbackDuration = 0.2f;

    private bool isKnockedBack = false;
    private float knockbackTimer;

    private Transform followParticleTransform;
    private Stack<float> moveSpeedStack = new Stack<float>();
    private Stack<float> attackSpeedStack = new Stack<float>();

    public EnemyTargetDetector detector { get; private set; }
    public AnimKeyframeEventReceiver keyframeEventReceiver;

    public event Action OnAttackEvent;
    public event Action OnPassiveEvent;

    public void Start()
    {
        SetupComponents();

        keyframeEventReceiver.OnAttackEvent += OnAttack;
        keyframeEventReceiver.OnDeadEvent += OnDead;

        Stats["Hp"].OnChangesCurrentValueIntAsObservable
            .Where(x => x <= 0)
            .Subscribe(_ =>
            {
                // 몬스터가 죽었을 때 실행됩니다.
                collider.enabled = false;
            });

        Stats["Hp"].OnBeforeApplyModifierDelegate += Stats_HP_OnBeforeApplyModifierDelegate;

        SetupFSM();
    }

    void Update()
    {
        Fsm.OnLogic();

        int toTargetDirection =
            transform.position.x > detector.GetCurrentTargetActor().Go.transform.position.x ? -1 : 1;

        transform.localScale = new Vector2(
            Mathf.Abs(transform.localScale.x) * toTargetDirection,
            transform.localScale.y);
    }

    public void Resets()
    {
        Anim.Rebind();
        Anim.Update(0);
        Anim.SetFloat("AttackSpeed", Stats["AttackSpeed"].CurrentValue);
        Fsm.Init();
        collider.enabled = true;
        Stats["Hp"].Initialize();
    }

    void SetupComponents()
    {
        Anim = GetComponentInChildren<Animator>();
        rigidbody2D = GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>();

        EffectController = gameObject.AddComponent<GameplayEffectController>();
        EffectController.Initialize(this);

        detector = gameObject.AddComponent<EnemyTargetDetector>();
        detector.Owner = this;

        Stats.AddStat("Hp", new Attribute() { Cap = new(Data.HP) }.Initialize());

        var _attack = new Stat().Initialize();
        _attack.ApplyModifier(new StatModifier() { Magnitude = Data.ATK });
        Stats.AddStat("Attack", _attack);

        var _attackRange = new Stat().Initialize();
        _attackRange.ApplyModifier(new StatModifier() { Magnitude = Data.DetectionRange });
        Stats.AddStat("AttackRange", _attackRange);

        var _attackSpeed = new Stat().Initialize();
        _attackSpeed.ApplyModifier(new StatModifier() { Magnitude = Data.AttackSpeed });
        Stats.AddStat("AttackSpeed", _attackSpeed);

        var _moveSpeed = new Stat().Initialize();
        _moveSpeed.ApplyModifier(new StatModifier() { Magnitude = Data.Speed });
        Stats.AddStat("MoveSpeed", _moveSpeed);

        transform.tag = "Monster";
        gameObject.layer = LayerMask.NameToLayer("Actor");

        firePoint = gameObject.Descendants().FirstOrDefault(x => x.name == "FirePoint")?.transform;

        SetupUniqueAbility();
        SetupSpecialAbility();
        SetupEventCommand();
    }

    void SetupUniqueAbility()
    {
        if (Data.AttackType == 0)
        {
            MeleeCommands commands = (MeleeCommands)StageParser.Instance.GetUniqueAbilityObject(Data.Id, Data);
            MeleeCommandApply.Apply(this, commands);
        }
        else if (Data.AttackType == 1)
        {
            RangedCommand commands = (RangedCommand)StageParser.Instance.GetUniqueAbilityObject(Data.Id, Data);
            RangedCommandApply.Apply(this, commands);
        }
    }

    void SetupSpecialAbility()
    {
        SpecialsCommand command = StageParser.Instance.GetSpecialAbilityCommand(Data.Id);

        if (command != null)
        {
            SpecialsCommandApply.Apply(this, command);
        }
    }

    void SetupEventCommand()
    {
        EventCommand command = StageParser.Instance.GetEventCommand(Data.Id);

        if (command != null)
        {
            EventCommandApply.ApplyEvent(this, command);
        }
    }

    void SetupFSM()
    {
        Fsm = new StateMachine();
        Fsm.AddState(nameof(MonsterState_Move), new MonsterState_Move() { Owner = this });
        Fsm.AddState(nameof(MonsterState_Attack), new MonsterState_Attack() { Owner = this });
        Fsm.AddState(nameof(MonsterState_Dead), new MonsterState_Dead() { Owner = this });
        Fsm.AddTransitionFromAny(nameof(MonsterState_Dead), transition => Stats["Hp"].CurrentValue <= 0,
            null, null, true);

        Fsm.SetStartState(nameof(MonsterState_Move));
        Fsm.Init();
    }

    void OnDead()
    {
        GameManager.Instance.userDataManager.GetReward(
            Data.GetXP, Data.GetMagicStone, Data.GetGold, monsterType.Basic);
        Destroy(gameObject);
    }

    void OnAttack()
    {
        OnPassiveEvent?.Invoke();
        OnAttackEvent?.Invoke();
    }

    private IStatModifier Stats_HP_OnBeforeApplyModifierDelegate(IStatModifier statModifier)
    {
        switch (statModifier)
        {
            case Damage damage:
                return OnDamage(damage);

            default:
                return statModifier;
        }
    }

    private Damage OnDamage(Damage damage)
    {
        Damage mewDamage = damage;

        if (Stats.HasStat("Defense"))
        {
            mewDamage = new Damage()
            { Magnitude = Mathf.Clamp(damage.Magnitude + Stats["Defense"].CurrentValue, -float.MaxValue, 0) };
        }

        if (mewDamage.Magnitude < 0)
        {
            GameManager.Instance.appearTextManager.MonsterAppearText
                (CurrencyHelper.ToCurrencyString((int)mewDamage.Magnitude), Go.transform.position);
        }

        return mewDamage;
    }


    public void SetModel(GameObject model)
    {
        this.model = model;
    }

    public GameObject GetModel()
    {
        return model;
    }

    public bool IsBinding() 
    {
        return TagController.tags.ContainsKey("binding");
    }

    public bool IsCanNotAttack()
    {
        return TagController.tags.ContainsKey("cannot_attack");
    }
}

public enum MonsterType
{
    Melee = 0,
    Ranged = 1,
}

public enum MeleeAbility
{
    None = 0,
    WideArea,
    Delay,
}

public enum RangedAbility
{
    None = 0,
    ShotGun,
    Penetrate,
}

public enum SpecialAbility
{
    Null = 0,
    Heal,
    Restraint,
    DotDamage,
}


public enum EventCommandType
{
    Null = 0,
    FirstATK,
    StackATK,
    Resurrect
}

public enum AttributeCommand
{
    atk,
    hp,
}