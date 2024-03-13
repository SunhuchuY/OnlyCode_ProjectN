using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using Unity.Linq;
using UnityEngine;
using UnityHFSM;

public class Player : MonoBehaviour, IGameActor
{
    const int INIT_MAX_HP = 3000;
    const float MAX_ATTACK_RANGE = 7;
    const float MULTIPLYER_ATTACK_RANGE = 1f;

    public GameObject Go => gameObject;
    public Stats Stats { get; private set; } = new();
    public GameplayEffectController EffectController { get; private set; }
    public TagController TagController { get; private set; } = new();
    public ActiveSkillController SkillController { get; private set; }
    public Animator Anim { get; private set; }
    public StateMachine Fsm { get; private set; }
    public ActorType ActorType => ActorType.Player;
    public bool IsDefenseUp { get; private set; }
    public Vector2 CachedTargetPosition;

    public CounterAttack counterAttack;

    [SerializeField] private bool isLookLeft = false;
    [SerializeField] private float manaReFillCoolTime = 1f;

    private Coroutine refillManaCoroutine;
    private Coroutine addiPerSecondsHPCoroutine;
    private Dictionary<string, StatModifier> cachedStatModifiersByLevel = new();

    public event System.Action<float> OnChangeDefense;
    public event System.Action<bool> OnChangeInvincible;

    private void Awake()
    {
        Anim = GetComponentInChildren<Animator>();

        EffectController = gameObject.AddComponent<GameplayEffectController>();
        EffectController.Initialize(this);

        SkillController = gameObject.AddComponent<ActiveSkillController>();

        Fsm = new StateMachine();
        Fsm.AddState(nameof(PlayerState_Idle), new PlayerState_Idle() { Owner = this });
        Fsm.AddState(nameof(PlayerState_Attack), new PlayerState_Attack() { Owner = this });
        Fsm.AddState(nameof(PlayerState_Dead), new PlayerState_Dead() { Owner = this });
        Fsm.AddTransitionFromAny(nameof(PlayerState_Dead), transition => Stats["Hp"].CurrentValueInt <= 0,
            null, null, true);

        Fsm.SetStartState(nameof(PlayerState_Idle));
        Fsm.Init();

        if (GetComponentInChildren<AnimKeyframeEventReceiver>() is { } _keyframeEventReceiver)
        {
            _keyframeEventReceiver.OnAttackEvent += OnAttack;
            _keyframeEventReceiver.OnDeadEvent += OnDead;
        }

        AddStats();
    }

    private void Start()
    {
        InitStats();

        Respawn();

        GameManager.Instance.world.AddActor(this);
        Anim.SetFloat("AttackSpeed", Stats["AttackSpeed"].CurrentValue);
    }

    private void Update()
    {
        Fsm.OnLogic();
        LookDirectionUpdate();
    }

    private void LookDirectionUpdate()
    {
        Transform targetTf = GameManager.Instance.bulletController.GetTarget();
        if (targetTf != null)
        {
            int lookDirection = isLookLeft ? -1 : 1;
            int toTargetDirection =
                transform.position.x > targetTf.position.x ? -1 : 1;

            transform.localScale = new Vector2(
                Mathf.Abs(transform.localScale.x) * lookDirection * toTargetDirection,
                transform.localScale.y);
        }
    }

    private void AddStats()
    {
        // stat을 추가합니다.
        Stats.AddStat("MaxHp", new Stat().Initialize());
        Stats.AddStat("Hp", new Attribute().Initialize());
        Stats.AddStat("Mp", new Attribute().Initialize());
        Stats.AddStat("Recovery", new Stat().Initialize());
        Stats.AddStat("Attack", new Stat().Initialize());
        Stats.AddStat("AttackMultiplier", new Stat().Initialize());
        Stats.AddStat("AttackRange", new Stat().Initialize());
        Stats.AddStat("AttackSpeed", new Stat().Initialize());
        Stats.AddStat("Defense", new Stat().Initialize());
        Stats.AddStat("DefenseMultiplier", new Stat().Initialize());
        Stats.AddStat("CriticalChance", new Stat().Initialize());
        Stats.AddStat("CriticalMultiplier", new Stat().Initialize());
        Stats.AddStat("CounterAttack", new Stat().Initialize());
    }

    private void InitStats()
    {
        void TryRemoveCachedModifier(string _statName)
        {
            if (cachedStatModifiersByLevel.ContainsKey(_statName))
            {
                var _modifierToRemove = cachedStatModifiersByLevel[_statName];
                cachedStatModifiersByLevel.Remove(_statName);
                Stats[_statName].RemoveModifier(_modifierToRemove);
            }
        }

        void ApplyModifier_Attack()
        {
            TryRemoveCachedModifier("Attack");

            var _magnitude = PlayerTapParser.Instance.attack[GameManager.Instance.userDataManager.userData.AttackLevel]
                .Attack.Value;
            var _modifier = new StatModifier() { Magnitude = _magnitude };
            Stats["Attack"].ApplyModifier(_modifier);
            cachedStatModifiersByLevel.Add("Attack", _modifier);
        }

        void ApplyModifier_AttackRange()
        {
            TryRemoveCachedModifier("AttackRange");

            float _newAttackRange = PlayerTapParser.Instance.attack[GameManager.Instance.userDataManager.userData.AttackRangeLevel].AttackDistance.Value;
            float _adjustedAttackRange = Mathf.Sqrt(_newAttackRange);
            float _multiply = (_adjustedAttackRange / Mathf.Sqrt(PlayerTapParser.Instance.MAX_STAT_ATTACK_RANGE));
            float _magnitude = MAX_ATTACK_RANGE * _multiply;
            var _modifier = new StatModifier() { Magnitude = _magnitude };
            Stats["AttackRange"].ApplyModifier(_modifier); 
            cachedStatModifiersByLevel.Add("AttackRange", _modifier);
        }

        void ApplyModifier_AttackSpeed()
        {
            TryRemoveCachedModifier("AttackSpeed");

            var _magnitude = PlayerTapParser.Instance
                .attack[GameManager.Instance.userDataManager.userData.AttackSpeedLevel].AttackSpeed.Value;
            var _modifier = new StatModifier() { Magnitude = _magnitude };
            Stats["AttackSpeed"].ApplyModifier(_modifier);
            cachedStatModifiersByLevel.Add("AttackSpeed", _modifier);
        }

        void ApplyModifier_CriticalChance()
        {
            TryRemoveCachedModifier("CriticalChance");

            var _magnitude = PlayerTapParser.Instance
                .attack[GameManager.Instance.userDataManager.userData.CriticalChanceLevel].AttackCritical.Value;
            var _modifier = new StatModifier() { Magnitude = _magnitude };
            Stats["CriticalChance"].ApplyModifier(_modifier);
            cachedStatModifiersByLevel.Add("CriticalChance", _modifier);
        }

        void ApplyModifier_CriticalMultiplier()
        {
            TryRemoveCachedModifier("CriticalMultiplier");

            var _magnitude = PlayerTapParser.Instance
                .attack[GameManager.Instance.userDataManager.userData.CriticalChanceLevel].AttackCriticalDamage.Value;
            var _modifier = new StatModifier() { Magnitude = _magnitude };
            Stats["CriticalMultiplier"].ApplyModifier(_modifier);
            cachedStatModifiersByLevel.Add("CriticalMultiplier", _modifier);
        }

        void ApplyModifier_Defense()
        {
            TryRemoveCachedModifier("Defense");

            var _magnitude = PlayerTapParser.Instance
                .defense[GameManager.Instance.userDataManager.userData.DefenseLevel].Defense.Value;
            var _modifier = new StatModifier() { Magnitude = _magnitude };
            Stats["Defense"].ApplyModifier(_modifier);
            cachedStatModifiersByLevel.Add("Defense", _modifier);
        }

        void ApplyModifier_CounterAttack()
        {
            TryRemoveCachedModifier("CounterAttack");

            // todo: 반격 레벨별 수치가 적용되어야 합니다.
            var _magnitude = PlayerTapParser.Instance
                .defense[GameManager.Instance.userDataManager.userData.CounterAttackLevel].Counter.Value;
            var _modifier = new StatModifier() { Magnitude = _magnitude };
            Stats["CounterAttack"].ApplyModifier(_modifier);
            cachedStatModifiersByLevel.Add("CounterAttack", _modifier);
        }

        void ApplyModifier_Recovery()
        {
            TryRemoveCachedModifier("Recovery");

            var _magnitude = PlayerTapParser.Instance
                .health[GameManager.Instance.userDataManager.userData.RecoveryLevel].Recovery.Value;
            var _modifier = new StatModifier() { Magnitude = _magnitude };
            Stats["Recovery"].ApplyModifier(_modifier);
            cachedStatModifiersByLevel.Add("Recovery", _modifier);
        }

        void ApplyModifier_MaxHp()
        {
            TryRemoveCachedModifier("MaxHp");

            var _magnitude = INIT_MAX_HP +
                             PlayerTapParser.Instance.health
                                 .Where(x => x.Key >= 1 &&
                                             x.Key <= GameManager.Instance.userDataManager.userData.MaxHealthLevel)
                                 .Select(x => x.Value.MaxHP.Value)
                                 .ToList()
                                 .Sum(x => x);
            var _modifier = new StatModifier() { Magnitude = _magnitude };
            Stats["MaxHp"].ApplyModifier(_modifier);
            cachedStatModifiersByLevel.Add("MaxHp", _modifier);
        }

        // 레벨에 알맞은 수치를 Stat에 적용합니다.
        // ApplyModifier 함수 내에서 magnitude를 계산할 때 사용한 속성들이 변경되었을 때, 수치 재적용이 일어날 수 있도록 변경 이벤트에 구독해주어야 합니다.
        ApplyModifier_Attack();
        GameManager.Instance.userDataManager.userData
            .ObserveEveryValueChanged(x => x.AttackLevel)
            .Subscribe(_ => ApplyModifier_Attack());

        ApplyModifier_AttackRange();
        GameManager.Instance.userDataManager.userData
            .ObserveEveryValueChanged(x => x.AttackRangeLevel)
            .Subscribe(_ => ApplyModifier_AttackRange());

        ApplyModifier_AttackSpeed();
        GameManager.Instance.userDataManager.userData
            .ObserveEveryValueChanged(x => x.AttackSpeedLevel)
            .Subscribe(_ => ApplyModifier_AttackSpeed());

        ApplyModifier_CriticalChance();
        GameManager.Instance.userDataManager.userData
            .ObserveEveryValueChanged(x => x.CriticalChanceLevel)
            .Subscribe(_ => ApplyModifier_CriticalChance());

        ApplyModifier_CriticalMultiplier();
        GameManager.Instance.userDataManager.userData
            .ObserveEveryValueChanged(x => x.CriticalMultiplierLevel)
            .Subscribe(_ => ApplyModifier_CriticalMultiplier());

        ApplyModifier_Defense();
        GameManager.Instance.userDataManager.userData
            .ObserveEveryValueChanged(x => x.DefenseLevel)
            .Subscribe(_ => ApplyModifier_Defense());

        ApplyModifier_CounterAttack();
        GameManager.Instance.userDataManager.userData
            .ObserveEveryValueChanged(x => x.CounterAttackLevel)
            .Subscribe(_ => ApplyModifier_CounterAttack());

        ApplyModifier_Recovery();
        GameManager.Instance.userDataManager.userData
            .ObserveEveryValueChanged(x => x.RecoveryLevel)
            .Subscribe(_ => ApplyModifier_Recovery());

        ApplyModifier_MaxHp();
        GameManager.Instance.userDataManager.userData
            .ObserveEveryValueChanged(x => x.MaxHealthLevel)
            .Subscribe(_ => ApplyModifier_MaxHp());


        Stats["Hp"].Cap.Value = Stats["MaxHp"].CurrentValueInt;
        Stats["Hp"].Initialize();
        Stats["Hp"].OnBeforeApplyModifierDelegate = Stats_HP_OnBeforeApplyModifierDelegate;
        Stats["MaxHp"].OnChangesCurrentValueIntAsObservable.Subscribe(_value => Stats["Hp"].Cap.Value = _value);

        Stats["Mp"].Cap.Value = 10;
        Stats["Mp"].Initialize();
    }

    private void ResetCoroutine()
    {
        if (refillManaCoroutine != null)
            StopCoroutine(refillManaCoroutine);

        if (addiPerSecondsHPCoroutine != null)
            StopCoroutine(addiPerSecondsHPCoroutine);

        refillManaCoroutine = StartCoroutine(RefillManaCoroutine());
        addiPerSecondsHPCoroutine = StartCoroutine(AddPerSecondsHPCoroutine());
    }

    private void Respawn()
    {
        if (refillManaCoroutine != null)
            StopCoroutine(refillManaCoroutine);

        if (addiPerSecondsHPCoroutine != null)
            StopCoroutine(addiPerSecondsHPCoroutine);

        Anim.Rebind();
        Anim.Update(0);
        Anim.SetFloat("AttackSpeed", Stats["AttackSpeed"].CurrentValue);

        Stats["Hp"].Initialize();

        Fsm.SetStartState(nameof(PlayerState_Idle));
        Fsm.Init();

        ResetCoroutine();
    }

    private IEnumerator RefillManaCoroutine()
    {
        while (true)
        {
            Stats["Mp"].ApplyModifier(new StatModifier() { Magnitude = 1, Instigator = this });
            yield return new WaitForSeconds(manaReFillCoolTime);
        }   
    }

    private IEnumerator AddPerSecondsHPCoroutine()
    {
        while (true)
        {
            Stats["Hp"].ApplyModifier(new StatModifier() { Magnitude = Stats["Recovery"].CurrentValueInt });
            yield return new WaitForSeconds(10);
        }
    }

    private void OnAttack()
    {
        GameManager.Instance.bulletController.Shoot();
    }

    private void OnDead()
    {
        ResetCoroutine();

        Respawn();
        GameManager.Instance.waveManager.Restart();
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
        if (TagController.Contains("invincible"))
            return new Damage() { Magnitude = 0 };

        counterAttack.PlayCounterAttack();
        
        Damage newDamage = new Damage() 
            { Magnitude = Mathf.Clamp(damage.Magnitude + Stats["Defense"].CurrentValue, -float.MaxValue, 0) };

        if (damage.Magnitude < 0)
        {
            GameManager.Instance.appearTextManager.PlayerAppearText
                (CurrencyHelper.ToCurrencyString((int)damage.Magnitude), transform.position);
        }

        return newDamage;
    }

    public int GetAttack()
    {
        int attack = Stats["Attack"].CurrentValueInt;
        int roll = UnityEngine.Random.Range(0, 100);

        // 치명타를 적용합니다.
        if (roll < Stats["CriticalChance"].CurrentValueInt)
        {
            // 크리티컬 데미지를 계산합니다.
            int critical = (int)(Stats["Attack"].CurrentValue * (Stats["CriticalMultiplier"].CurrentValue * 0.01f));
        }

        return attack * GameManager.Instance.userDataManager.userData.CurrentLevel;
    }

    public bool GetIsInvincible() 
    {
        return TagController.Contains("invincible");   
    }


    // public void OnDrawGizmos()
    // {
    //     if (Application.isPlaying == false)
    //         return;
    //
    //     if (gameObject.DescendantsAndSelf().First(x => x.name == "Pivot") is { } _pivot)
    //     {
    //         var _target = GameManager.Instance.bulletController.GetTarget();
    //         Color _color = _target ? Color.red : Color.green;
    //
    //         // draw 2d circle for debug
    //         Gizmos.color = _color;
    //         Gizmos.DrawWireSphere(_pivot.transform.position, Stats["AttackRange"].CurrentValue);
    //
    //         if (_target != null)
    //         {
    //             // draw line to target for debug
    //             Gizmos.color = _color;
    //             Gizmos.DrawLine(_pivot.transform.position, _target.transform.position);
    //         }
    //     }
    // }
}