using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using Unity.Linq;
using UnityEngine;
using UnityHFSM;
using BigInteger = System.Numerics.BigInteger;

public class MonsterStat<T> where T : struct
{
    private List<T> Modifiers = new List<T>();
    private Func<T, T, T> addOperation;

    public MonsterStat(T baseValue, Func<T, T, T> addOperation)
    {
        m_baseValue = baseValue;
        this.addOperation = addOperation;
    }

    public void AddModifier(T modifier)
    {
        Modifiers.Add(modifier);
    }

    public void RemoveModifier(T modifier)
    {
        Modifiers.Remove(modifier);
    }

    private readonly T m_baseValue;

    public T Value
    {
        get
        {
            T final = m_baseValue;
            // Modifier 적용 로직
            foreach (var modifier in Modifiers)
            {
                final = addOperation(final, modifier);
            }

            return final;
        }
    }
}

public class MonsterAttributes
{
    private const float DestroyTime = 60 * 10;
    private const float AdjustDetection = 1;

    public readonly Attribute HP;
    public MonsterStat<BigInteger> ATK;
    public readonly float ATKSpeed;
    public readonly float MoveSpeed;
    public readonly float Detection; // 탐지거리

    public readonly BigInteger DropMagicStone;
    public readonly BigInteger DropEXP;
    public readonly BigInteger DropGold;

    public MonsterAttributes(MonsterProfile profile)
    {
        HP = new Attribute();
        HP.Cap = new UniRx.ReactiveProperty<float>(profile.HP);
        HP.Initialize();

        ATK = new MonsterStat<BigInteger>(profile.ATK, (a, b) => a + b);
        ATKSpeed = profile.AttackSpeed;
        Detection = profile.DetectionRange - AdjustDetection;
        MoveSpeed = profile.Speed;

        DropMagicStone = profile.GetMagicStone;
        DropEXP = profile.GetXP;
        DropMagicStone = profile.GetGold;
    }
}

public class Monster : MonoBehaviour
{
    [SerializeField] public bool isLookLeft = false;
    public Animator Animator { get; private set; }
    public Rigidbody2D rigidbody2D { get; private set; }
    public Collider2D collider { get; private set; }
    public StateMachine Fsm { get; private set; }

    public GameObject Go => gameObject;
    public Stats Stats { get; private set; } = new Stats();
    public GameplayEffectController EffectController { get; set; }
    public TagController TagController { get; private set; } = new();
    public ActiveSkillController SkillController { get; private set; }
    public ActorType ActorType => ActorType.Monster;

    public Transform player;
    public Transform targetObj;

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

    public MonsterAttributes attributes { get; private set; }
    public EnemyTargetDetector detector { get; private set; }
    public MonsterAnimationKeyframeEventReceiver keyframeEventReceiver { get; private set; }
    int _id = -1;

    private void Awake()
    {
        SetupComponents();

        keyframeEventReceiver.OnAttackEvent += OnAttack;
        keyframeEventReceiver.OnDeadEvent += OnDead;

        attributes.HP.OnChangesCurrentValueIntAsObservable
            .Where(x => x <= 0)
            .Subscribe(_ =>
            {
                // 몬스터가 죽었을 때 실행됩니다.
                collider.enabled = false;
            });

        SetupFSM();
    }

    public event Action AttackEvent;
    public event Action PassiveEvent;

    void OnEnable()
    {
        Resets();
    }

    void OnDisable()
    {
        attributes.HP.Initialize();
    }

    void Update()
    {
        Fsm.OnLogic();

        int lookDirection = isLookLeft ? -1 : 1;
        int toTargetDirection =
            transform.position.x > detector.GetCurrentTargetTransform().transform.position.x ? -1 : 1;

        transform.localScale = new Vector2(
            Mathf.Abs(transform.localScale.x) * lookDirection * toTargetDirection,
            transform.localScale.y);
    }

    public void Resets()
    {
        Animator.Rebind();
        Animator.Update(0);
        Animator.SetFloat("AttackSpeed", attributes.ATKSpeed);
        Fsm.Init();
        collider.enabled = true;
    }

    void SetupComponents()
    {
        _id = int.Parse(transform.parent.name);
        MonsterProfile _profile = StageParser.Instance.monsterProfiles[_id];
        attributes = new(_profile);

        Animator = GetComponentInChildren<Animator>();
        rigidbody2D = GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>();

        detector = gameObject.AddComponent<EnemyTargetDetector>();
        detector.Owner = this;

        keyframeEventReceiver = GetComponentInChildren<MonsterAnimationKeyframeEventReceiver>();

        transform.tag = "Monster";
        gameObject.layer = LayerMask.NameToLayer("Actor");

        firePoint = gameObject.Descendants().FirstOrDefault(x => x.name == "FirePoint")?.transform;

        SetupUniqueAbility(_profile);
        SetupSpecialAbility();
        SetupEventCommand();
    }

    void SetupUniqueAbility(MonsterProfile _profile)
    {
        if (_profile.AttackType == 0)
        {
            MeleeCommands commands = (MeleeCommands)StageParser.Instance.GetUniqueAbilityObject(_id, _profile);
            MeleeCommandApply.Apply(this, commands);
        }
        else if (_profile.AttackType == 1)
        {
            RangedCommand commands = (RangedCommand)StageParser.Instance.GetUniqueAbilityObject(_id, _profile);
            RangedCommandApply.Apply(this, commands);
        }
    }

    void SetupSpecialAbility()
    {
        SpecialsCommand command = StageParser.Instance.GetSpecialAbilityCommand(_id);

        if (command != null)
        {
            SpecialsCommandApply.Apply(this, command);
        }
    }

    void SetupEventCommand()
    {
        EventCommand command = StageParser.Instance.GetEventCommand(_id);

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
        Fsm.AddTransitionFromAny(nameof(MonsterState_Dead), transition => attributes.HP.CurrentValue <= 0,
            null, null, true);

        Fsm.SetStartState(nameof(MonsterState_Move));
        Fsm.Init();
    }

    void OnDead()
    {
        Resets();
        GameManager.Instance.userDataManager.GetReward(attributes.DropEXP, attributes.DropMagicStone,
            attributes.DropGold, monsterType.Basic);
        MonstersObjectPool.Instance.ReleaseGO(_id, gameObject);
    }

    void OnAttack()
    {
        PassiveEvent?.Invoke();
        AttackEvent?.Invoke();
    }

    public void ApplyDamage(int damage)
    {
        Damage mod = new Damage();
        mod.Magnitude = -damage;
        attributes.HP.ApplyModifier(mod);
        GameManager.Instance.appearTextManager.MonsterAppearText(CurrencyHelper.ToCurrencyString(damage), transform.position);
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