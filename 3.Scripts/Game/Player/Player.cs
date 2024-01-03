using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Linq;
using UnityEngine;
using UnityHFSM;

public class Player : MonoBehaviour
{
    public void Temporary_invinicibleTurnOn(float duration)
    {
        StartCoroutine(Temporary_invinicibleTurnOn_Corutine(duration));
    }

    private IEnumerator Temporary_invinicibleTurnOn_Corutine(float _duration)
    {
        IsInvincible = true;
        OnChangeInvincible?.Invoke(true);
        yield return new WaitForSeconds(_duration);
        IsInvincible = false;
        OnChangeInvincible?.Invoke(false);
    }

    public event System.Action<float> OnChangeDefense;
    public event System.Action<bool> OnChangeInvincible;

    public Animator Anim { get; private set; }
    public StateMachine Fsm { get; private set; }

    public Stat MaxHealth { get; private set; }
    public Attribute Health { get; private set; }
    public Attribute Mana { get; private set; }

    public Stat HealthAdditionPerSeconds { get; private set; }

    public Stat Attack { get; private set; }
    public Stat AttackMultiplier { get; private set; }
    public Stat AttackRange { get; private set; }
    public Stat AttackSpeed { get; private set; }

    public Stat Defense { get; private set; }
    public Stat DefenseMultiplier { get; private set; }

    public Stat CriticalMultiplier { get; private set; }

    public Stat CounterAttack { get; private set; }

    // todo: level attribute에 의하여 stat이 변하도록 수정해야 합니다.
    //   이를 위해 Stat에 level attribute로 인한 modifier를 적용하도록 수정해야 합니다.
    public Attribute MaxHealthLevel { get; private set; }
    public Attribute HealthAdditionPerSecondsLevel { get; private set; }
    public Attribute AttackLevel { get; private set; }
    public Attribute AttackMultiplierLevel { get; private set; }
    public Attribute AttackRangeLevel { get; private set; }
    public Attribute AttackSpeedLevel { get; private set; }
    public Attribute DefenseLevel { get; private set; }
    public Attribute DefenseMultiplierLevel { get; private set; }
    public Attribute CriticalMultiplierLevel { get; private set; }
    public Attribute CounterAttackLevel { get; private set; }

    public Dictionary<int, Attribute> SkillLevels { get; private set; } = new Dictionary<int, Attribute>();

    public bool IsInvincible { get; private set; }
    public bool IsDefenseUp { get; private set; }
    public Vector2 CachedTargetPosition;

    public CounterAttack counterAttack;
    const float maxMana = 10f;
    [SerializeField] private bool isLookLeft = false;
    [SerializeField] private int initialMaxHealth = 3000;
    [SerializeField] private float initialAttack = 20f;
    [SerializeField] private float initialAttackRange = 2.9f;
    [SerializeField] private float initialAttackSpeed = 1f;
    [SerializeField] private int initialCounterAttack = 10;
    [SerializeField] private int initialHealthAdditionPerSeconds = 20;

    [SerializeField] private float manaReFillCoolTime = 1f;
    [SerializeField] private float manaReFillCurrentTime = 0;

    private Coroutine refillManaCoroutine;

    private void Awake()
    {
        Anim = GetComponentInChildren<Animator>();
        Fsm = new StateMachine();
        Fsm.AddState(nameof(PlayerState_Idle), new PlayerState_Idle() { Owner = this });
        Fsm.AddState(nameof(PlayerState_Attack), new PlayerState_Attack() { Owner = this });
        Fsm.AddState(nameof(PlayerState_Dead), new PlayerState_Dead() { Owner = this });
        Fsm.AddTransitionFromAny(nameof(PlayerState_Dead), transition => Health.CurrentValue <= 0,
            null, null, true);

        Fsm.SetStartState(nameof(PlayerState_Idle));
        Fsm.Init();

        MaxHealth = new Stat() { Cap = (int)initialMaxHealth };
        MaxHealth.Initialize();
        MaxHealth.ApplyModifier(MaxHealth.Cap);
        Health = new Attribute() { Cap = MaxHealth.CurrentValue };
        Health.Initialize();
        Mana = new Attribute() { Cap = (int)maxMana };
        Mana.Initialize();
        HealthAdditionPerSeconds = new Stat() { Cap = -1 };
        HealthAdditionPerSeconds.Initialize();
        HealthAdditionPerSeconds.ApplyModifier(initialHealthAdditionPerSeconds);

        Attack = new Stat() { Cap = -1 };
        Attack.Initialize();
        Attack.ApplyModifier((int)initialAttack);
        AttackMultiplier = new Stat() { Cap = -1 };
        AttackMultiplier.Initialize();
        AttackMultiplier.ApplyModifier(1);
        AttackRange = new Stat() { Cap = -1 };
        AttackRange.Initialize();
        AttackRange.ApplyModifier((int)initialAttackRange);
        AttackSpeed = new Stat() { Cap = -1 };
        AttackSpeed.Initialize();
        AttackSpeed.ApplyModifier((int)initialAttackSpeed);

        Defense = new Stat() { Cap = -1 };
        Defense.Initialize();
        Defense.ApplyModifier(0);
        DefenseMultiplier = new Stat() { Cap = -1 };
        DefenseMultiplier.Initialize();
        DefenseMultiplier.ApplyModifier(1);

        CriticalMultiplier = new Stat() { Cap = -1 };
        CriticalMultiplier.Initialize();
        CriticalMultiplier.ApplyModifier(1);

        CounterAttack = new Stat() { Cap = -1 };
        CounterAttack.Initialize();
        CounterAttack.ApplyModifier(initialCounterAttack);

        MaxHealthLevel = new Attribute() { Cap = -1 };
        MaxHealthLevel.Initialize();
        MaxHealthLevel.ApplyModifier(1);
        HealthAdditionPerSecondsLevel = new Attribute() { Cap = -1 };
        HealthAdditionPerSecondsLevel.Initialize();
        HealthAdditionPerSecondsLevel.ApplyModifier(1);
        AttackLevel = new Attribute() { Cap = -1 };
        AttackLevel.Initialize();
        AttackLevel.ApplyModifier(1);
        AttackMultiplierLevel = new Attribute() { Cap = -1 };
        AttackMultiplierLevel.Initialize();
        AttackMultiplierLevel.ApplyModifier(1);
        AttackRangeLevel = new Attribute() { Cap = -1 };
        AttackRangeLevel.Initialize();
        AttackRangeLevel.ApplyModifier(1);
        AttackSpeedLevel = new Attribute() { Cap = -1 };
        AttackSpeedLevel.Initialize();
        AttackSpeedLevel.ApplyModifier(1);
        DefenseLevel = new Attribute() { Cap = -1 };
        DefenseLevel.Initialize();
        DefenseLevel.ApplyModifier(1);
        DefenseMultiplierLevel = new Attribute() { Cap = -1 };
        DefenseMultiplierLevel.Initialize();
        DefenseMultiplierLevel.ApplyModifier(1);
        CriticalMultiplierLevel = new Attribute() { Cap = -1 };
        CriticalMultiplierLevel.Initialize();
        CriticalMultiplierLevel.ApplyModifier(1);
        CounterAttackLevel = new Attribute() { Cap = -1 };
        CounterAttackLevel.Initialize();
        CounterAttackLevel.ApplyModifier(1);

        if (GetComponentInChildren<PlayerAnimationKeyframeEventReceiver>() is { } _keyframeEventReceiver)
        {
            _keyframeEventReceiver.OnAttackEvent += OnAttack;
            _keyframeEventReceiver.OnDeadEvent += OnDead;
        }

        Respawn();
    }

    private void Update()
    {
        Fsm.OnLogic();
    }

    private void Respawn()
    {
        Anim.Rebind();
        Anim.Update(0);

        Health.ApplyModifier(Health.Cap);

        Fsm.SetStartState(nameof(PlayerState_Idle));
        Fsm.Init();

        if (refillManaCoroutine != null)
            StopCoroutine(refillManaCoroutine);

        refillManaCoroutine = StartCoroutine(RefillManaCoroutine());
    }

    private IEnumerator RefillManaCoroutine()
    {
        while (true)
        {
            if (Health.CurrentValue <= 0)
                break;

            ManaReFill(1);
            yield return new WaitForSeconds(manaReFillCoolTime);
        }
    }

    public void GetDamage(int _damage)
    {
        if (IsInvincible == false)
        {
            int _amount = -(int)(_damage + _damage * ((float)DefenseMultiplier.CurrentValue / 100));
            Health.ApplyModifier(_amount);
        }

        counterAttack.CounterAttack_Funtion();
    }

    private void OnAttack()
    {
        GameManager.Instance.bulletController.Shoot();
    }

    private void OnDead()
    {
        if (refillManaCoroutine != null)
            StopCoroutine(refillManaCoroutine);

        Respawn();
        GameManager.Instance.waveManager.Restart();
    }

    public void ManaReFill(int amount)
    {
        Mana.ApplyModifier(amount);
    }

    public bool IsCanUseMana(int needOfManaAmount)
    {
        if (Mana.CurrentValue >= needOfManaAmount)
            return true;
        else
            return false;
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
            Gizmos.DrawWireSphere(_pivot.transform.position, AttackRange.CurrentValue);

            if (_target != null)
            {
                // draw line to target for debug
                Gizmos.color = _color;
                Gizmos.DrawLine(_pivot.transform.position, _target.transform.position);
            }
        }
    }
}