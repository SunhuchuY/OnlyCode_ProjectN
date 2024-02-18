using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using Unity.Linq;
using UnityEngine;
using UnityHFSM;

public class Player : MonoBehaviour, IGameActor
{
    public Dictionary<int, Attribute> SkillLevels { get; private set; } = new Dictionary<int, Attribute>();

    public GameObject Go => gameObject;
    public Stats Stats { get; private set; } = new();
    public GameplayEffectController EffectController { get; set; }
    public TagController TagController { get; set; } = new();
    public ActiveSkillController SkillController { get; private set; }
    public ActorType ActorType => ActorType.Player;

    public bool IsInvincible { get; private set; }
    public bool IsDefenseUp { get; private set; }
    public Vector2 CachedTargetPosition;

    public CounterAttack counterAttack;

    const float maxMana = 10f;
    const float attackRangeMultiplier = 0.1f;
    const int initialMaxHealth = 3000;
    const float initialAttackRange = 2f;

    [SerializeField] private bool isLookLeft = false;

    [SerializeField] private float manaReFillCoolTime = 1f;
    [SerializeField] private float manaReFillCurrentTime = 0;

    private Coroutine refillManaCoroutine;
    private Coroutine addiPerSecondsHPCoroutine;
    private Dictionary<string, StatModifier> cachedStatModifiersByLevel = new();

    public event System.Action<float> OnChangeDefense;
    public event System.Action<bool> OnChangeInvincible;

    public Animator Anim { get; private set; }
    public StateMachine Fsm { get; private set; }

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

        if (GetComponentInChildren<PlayerAnimationKeyframeEventReceiver>() is { } _keyframeEventReceiver)
        {
            _keyframeEventReceiver.OnAttackEvent += OnAttack;
            _keyframeEventReceiver.OnDeadEvent += OnDead;
        }

        AddStats();
    }

    private void Start()
    {
        InitStatLevelsFromSaveData();
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
        Stats.AddStat("MaxHealth", new Stat().Initialize());
        Stats.AddStat("Hp", new Attribute().Initialize());
        Stats.AddStat("Mana", new Attribute().Initialize());
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

        // level attribute를 추가합니다.
        Stats.AddStat("MaxHealthLevel", new Attribute().Initialize());
        Stats.AddStat("RecoveryLevel", new Attribute().Initialize());
        Stats.AddStat("AttackLevel", new Attribute().Initialize());
        Stats.AddStat("AttackMultiplierLevel", new Attribute().Initialize());
        Stats.AddStat("AttackRangeLevel", new Attribute());
        Stats.AddStat("AttackSpeedLevel", new Attribute().Initialize());
        Stats.AddStat("DefenseLevel", new Attribute().Initialize());
        // Stats.AddStat("DefenseMultiplierLevel", new Attribute().Initialize());
        Stats.AddStat("CriticalChanceLevel", new Attribute().Initialize());
        Stats.AddStat("CriticalMultiplierLevel", new Attribute().Initialize());
        Stats.AddStat("CounterAttackLevel", new Attribute().Initialize());
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

        void ApplyModifier_MaxHealth()
        {
            TryRemoveCachedModifier("MaxHealth");

            var _magnitude = initialMaxHealth + 
                PlayerTapParser.Instance.health
                .Where(x => x.Key >= 1 && x.Key <= Stats["MaxHealthLevel"].CurrentValueInt)
                .Select(x => x.Value.MaxHP.Value)
                .ToList()
                .Sum(x => x);
            var _modifier = new StatModifier() { Magnitude = _magnitude };
            Stats["MaxHealth"].ApplyModifier(_modifier);
            cachedStatModifiersByLevel.Add("MaxHealth", _modifier);
        }

        void ApplyModifier_Recovery()
        {
            TryRemoveCachedModifier("Recovery");

            var _magnitude = PlayerTapParser.Instance.health[Stats["RecoveryLevel"].CurrentValueInt].Recovery.Value;
            var _modifier = new StatModifier() { Magnitude = _magnitude };
            Stats["Recovery"].ApplyModifier(_modifier);
            cachedStatModifiersByLevel.Add("Recovery", _modifier);
        }

        void ApplyModifier_Attack()
        {
            TryRemoveCachedModifier("Attack");

            var _magnitude = PlayerTapParser.Instance.attack[Stats["AttackLevel"].CurrentValueInt].Attack.Value;
            var _modifier = new StatModifier() { Magnitude = _magnitude };
            Stats["Attack"].ApplyModifier(_modifier);
            cachedStatModifiersByLevel.Add("Attack", _modifier);
        }

        void ApplyModifier_AttackRange()
        {
            TryRemoveCachedModifier("AttackRange");

            // todo: 공격 범위 레벨별 수치가 적용되어야 합니다.
            var _magnitude = Stats["AttackRangeLevel"].CurrentValueInt;
            var _modifier = new StatModifier() { Magnitude = _magnitude };
            Stats["AttackRange"].ApplyModifier(_modifier);
            cachedStatModifiersByLevel.Add("AttackRange", _modifier);
        }

        void ApplyModifier_AttackSpeed()
        {
            TryRemoveCachedModifier("AttackSpeed");

            var _magnitude = PlayerTapParser.Instance.attack[Stats["AttackSpeedLevel"].CurrentValueInt].AttackSpeed.Value;
            var _modifier = new StatModifier() { Magnitude = _magnitude };
            Stats["AttackSpeed"].ApplyModifier(_modifier);
            cachedStatModifiersByLevel.Add("AttackSpeed", _modifier);
        }

        void ApplyModifier_Defense()
        {
            TryRemoveCachedModifier("Defense");

            var _magnitude = PlayerTapParser.Instance.defense[Stats["DefenseLevel"].CurrentValueInt].Defense.Value;
            var _modifier = new StatModifier() { Magnitude = _magnitude };
            Stats["Defense"].ApplyModifier(_modifier);
            cachedStatModifiersByLevel.Add("Defense", _modifier);
        }

        void ApplyModifier_CriticalChance()
        {
            TryRemoveCachedModifier("CriticalChance");

            var _magnitude = PlayerTapParser.Instance.attack[Stats["CriticalChanceLevel"].CurrentValueInt].AttackCritical.Value;
            var _modifier = new StatModifier() { Magnitude = _magnitude };
            Stats["CriticalChance"].ApplyModifier(_modifier);
            cachedStatModifiersByLevel.Add("CriticalChance", _modifier);
        }

        void ApplyModifier_CriticalMultiplier()
        {
            TryRemoveCachedModifier("CriticalMultiplier");

            var _magnitude = PlayerTapParser.Instance.attack[Stats["CriticalChanceLevel"].CurrentValueInt].AttackCriticalDamage.Value;
            var _modifier = new StatModifier() { Magnitude = _magnitude };
            Stats["CriticalMultiplier"].ApplyModifier(_modifier);
            cachedStatModifiersByLevel.Add("CriticalMultiplier", _modifier);
        }

        void ApplyModifier_CounterAttack()
        {
            TryRemoveCachedModifier("CounterAttack");

            // todo: 반격 레벨별 수치가 적용되어야 합니다.
            var _magnitude = Stats["CounterAttackLevel"].CurrentValueInt;
            var _modifier = new StatModifier() { Magnitude = _magnitude };
            Stats["CounterAttack"].ApplyModifier(_modifier);
            cachedStatModifiersByLevel.Add("CounterAttack", _modifier);
        }

        // 레벨에 알맞은 수치를 Stat에 적용합니다.
        // ApplyModifier 함수 내에서 magnitude를 계산할 때 사용한 속성들이 변경되었을 때, 수치 재적용이 일어날 수 있도록 변경 이벤트에 구독해주어야 합니다.
        ApplyModifier_MaxHealth();
        Stats["MaxHealthLevel"].OnChangesCurrentValueIntAsObservable.Subscribe(_ => ApplyModifier_MaxHealth());

        ApplyModifier_Recovery();
        Stats["RecoveryLevel"].OnChangesCurrentValueIntAsObservable.Subscribe(_ => ApplyModifier_Recovery());

        ApplyModifier_Attack();
        Stats["AttackLevel"].OnChangesCurrentValueIntAsObservable.Subscribe(_ => ApplyModifier_Attack());

        ApplyModifier_AttackRange();
        Stats["AttackRangeLevel"].OnChangesCurrentValueIntAsObservable.Subscribe(_ => ApplyModifier_AttackRange());

        ApplyModifier_AttackSpeed();
        Stats["AttackSpeedLevel"].OnChangesCurrentValueIntAsObservable.Subscribe(_ => ApplyModifier_AttackSpeed());

        ApplyModifier_Defense();
        Stats["DefenseLevel"].OnChangesCurrentValueIntAsObservable.Subscribe(_ => ApplyModifier_Defense());

        ApplyModifier_CriticalChance();
        Stats["CriticalChanceLevel"].OnChangesCurrentValueIntAsObservable.Subscribe(_ => ApplyModifier_CriticalChance());

        ApplyModifier_CriticalMultiplier();
        Stats["CriticalMultiplierLevel"].OnChangesCurrentValueIntAsObservable.Subscribe(_ => ApplyModifier_CriticalMultiplier());

        ApplyModifier_CounterAttack();
        Stats["CounterAttackLevel"].OnChangesCurrentValueIntAsObservable.Subscribe(_ => ApplyModifier_CounterAttack());


        Stats["Hp"].Cap.Value = Stats["MaxHealth"].CurrentValueInt;
        Stats["Hp"].Initialize();
        Stats["MaxHealth"].OnChangesCurrentValueIntAsObservable.Subscribe(_value => Stats["Hp"].Cap.Value = _value);

        Stats["Mana"].Cap.Value = 10;
        Stats["Mana"].Initialize();
    }

    public void InitStatLevelsFromSaveData()
    {
        var _userData = GameManager.Instance.userDataManager.userData;

        Stats["MaxHealthLevel"].ApplyModifier(new StatModifier() { OperationType = ModifierOperationType.Override, Magnitude = _userData.MaxHealthLevel });
        Stats["RecoveryLevel"].ApplyModifier(new StatModifier() { OperationType = ModifierOperationType.Override, Magnitude = _userData.RecoveryLevel });
        Stats["AttackLevel"].ApplyModifier(new StatModifier() { OperationType = ModifierOperationType.Override, Magnitude = _userData.AttackLevel });
        Stats["AttackMultiplierLevel"].ApplyModifier(new StatModifier() { OperationType = ModifierOperationType.Override, Magnitude = _userData.AttackMultiplierLevel });
        Stats["AttackRangeLevel"].ApplyModifier(new StatModifier() { OperationType = ModifierOperationType.Override, Magnitude = _userData.AttackRangeLevel });
        Stats["AttackSpeedLevel"].ApplyModifier(new StatModifier() { OperationType = ModifierOperationType.Override, Magnitude = _userData.AttackSpeedLevel });
        Stats["DefenseLevel"].ApplyModifier(new StatModifier() { OperationType = ModifierOperationType.Override, Magnitude = _userData.DefenseLevel });
        // Stats["DefenseMultiplierLevel"].ApplyModifier(new StatModifier() { OperationType = ModifierOperationType.Override,Magnitude = defense.Value });
        Stats["CriticalChanceLevel"].ApplyModifier(new StatModifier() { OperationType = ModifierOperationType.Override, Magnitude = _userData.CriticalChanceLevel });
        Stats["CriticalMultiplierLevel"].ApplyModifier(new StatModifier() { OperationType = ModifierOperationType.Override, Magnitude = _userData.CriticalMultiplierLevel });
        Stats["CounterAttackLevel"].ApplyModifier(new StatModifier() { Magnitude = _userData.CounterAttackLevel });
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
            Stats["Mana"].ApplyModifier(new StatModifier() { Magnitude = 1, Instigator = this });
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

    public void ApplyDamage(float damage)
    {
        if (IsInvincible)
            return;

        // 방어력 관련 연산을 합니다.
        damage -= Stats["Defense"].CurrentValue;
        counterAttack.PlayCounterAttack();

        // 데미지가 0보다 작거나 같으면, 데미지를 입히지 않습니다.
        if (damage <= 0)
            return;

        Stats["Hp"].ApplyModifier(new Damage() { Magnitude = -damage });
        GameManager.Instance.appearTextManager.PlayerAppearText(CurrencyHelper.ToCurrencyString((int)damage),
            transform.position);
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

    public void OnDrawGizmos()
    {
        if (Application.isPlaying == false)
            return;

        if (gameObject.DescendantsAndSelf().First(x => x.name == "Pivot") is { } _pivot)
        {
            var _target = GameManager.Instance.bulletController.GetTarget();
            Color _color = _target ? Color.red : Color.green;

            // draw 2d circle for debug
            Gizmos.color = _color;
            Gizmos.DrawWireSphere(_pivot.transform.position, Stats["AttackRange"].CurrentValue);

            if (_target != null)
            {
                // draw line to target for debug
                Gizmos.color = _color;
                Gizmos.DrawLine(_pivot.transform.position, _target.transform.position);
            }
        }
    }
}