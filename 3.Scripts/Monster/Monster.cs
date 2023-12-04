using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityHFSM;
using BigInteger = System.Numerics.BigInteger;

public class Monster : MonoBehaviour
{
    public enum AttackType
    {
        Melee,
        Projectile
    }

    public float moveSpeed = 5f; // Adjust the speed as needed

    // knock back
    private float knockbackForce = 7f;
    private float knockbackDuration = 0.2f;

    private bool isKnockedBack = false;
    private float knockbackTimer;

    public Animator Anim { get; private set; }
    public Rigidbody2D rigidbody2D { get; private set; }

    public int wideShotNum = 3;

    public Transform player;
    [HideInInspector] public Transform targetObj;

    public float maxHealth = 100f; // Maximum health

    float healthDeclineDuration = 0.5f;

    [SerializeField] private string _dropEXP, _dropBloodStone, _dropGold; // ���
    public BigInteger dropEXP = 50, dropBloodStone = 1250, dropGold = 10; // ���
    public float attackMount = 60;

    public AttackType attackType = AttackType.Melee;
    public monsterType _monsterType = monsterType.Basic;
    public ArrowType _arrowType = ArrowType.Basic;

    [HideInInspector] public float showHpBar_coolTime = 2f;
    [HideInInspector] public float showHpBar_Time = 0;
    private double tempOriginalCurHp;
    public bool isShowHpBar = false;

    public GameObject Arrow;

    bool isKnockDown = false;
    bool isFirstAttack = false;

    public bool isReverseScale = false;

    public float AttackRange = 2f;

    public Health Health { get; private set; }
    public StateMachine Fsm { get; private set; }

    private Transform followParticleTransform;
    private Queue<float> callBackSpeed = new Queue<float>();

    private void Awake()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        Health = new Health();
        Health.Cap = (int)maxHealth;
        Health.Initialize();

        if (GetComponentInChildren<AnimationKeyframeEventReceiver>() is { } _keyframeEventReceiver)
        {
            // �ִϸ��̼� ��� �� Ű������ �ִϸ��̼��� �߻����� �� ����� �ݹ��� ����մϴ�.
            _keyframeEventReceiver.OnAttackEvent += OnAttack;
            _keyframeEventReceiver.OnDeadEvent += OnDead;
        }

        Fsm = new StateMachine();
        Fsm.AddState(nameof(MonsterState_Move), new MonsterState_Move() { Owner = this });
        Fsm.AddState(nameof(MonsterState_Attack), new MonsterState_Attack() { Owner = this });
        Fsm.AddState(nameof(MonsterState_Dead), new MonsterState_Dead() { Owner = this });
        Fsm.AddTransitionFromAny(nameof(MonsterState_Dead), transition => Health.CurrentValue <= 0,
            null, null, true);

        Fsm.SetStartState(nameof(MonsterState_Move));
        Fsm.Init();

        try
        {
            dropEXP = BigInteger.Parse(_dropEXP);
            dropGold = BigInteger.Parse(_dropGold);
            dropBloodStone = BigInteger.Parse(_dropBloodStone);
        }
        catch (FormatException ex)
        {
            Debug.LogError("drop EXP, Gold, BloodStone Parse Error: " + ex.Message);
        }
    }

    private void OnEnable()
    {
        player = GameManager.Instance.player.transform;
        targetObj = player; // temp: �ӽ������� ���� ������ ����� �÷��̾�� �����մϴ�.

        Anim = GetComponentInChildren<Animator>();
        tempOriginalCurHp = maxHealth;

        AllOfAnimation_False();

        // �ִϸ��̼� ��� ���¸� �ʱ�� �ǵ����ϴ�.
        Anim.Rebind();
        Anim.Update(0);

        Health.Initialize();
        Fsm.Init();
    }

    private void OnDisable()
    {
        GameManager.Instance.bulletController.RemoveMonster(gameObject);
    }

    void Update()
    {
        Fsm.OnLogic();

        // knock back
        if (isKnockedBack)
        {
            if (knockbackTimer > 0f)
            {
                // �˹� ���� ��
                knockbackTimer -= Time.deltaTime;
            }
            else
            {
                // �˹��� ������ ��, �� ����
                rigidbody2D.velocity = UnityEngine.Vector2.zero;
                isKnockedBack = false;
            }
        }

        if (followParticleTransform != null)
            followParticleTransform.position = transform.position - UnityEngine.Vector3.up;
    }

    private void FixedUpdate()
    {
        Vector2 targetPositionData = Vector2.zero;
        Vector2 thisPositionData = gameObject.GetComponent<PositionData>().AimPosition.position;

        if (targetObj == null || targetObj.gameObject.activeSelf == false)
            targetPositionData = GameManager.Instance.player.GetComponent<PositionData>().AimPosition.position;
        else
            targetPositionData = targetObj.GetComponent<PositionData>().AimPosition.position;


        if (isReverseScale == false) // ����
        {
            if (thisPositionData.x > targetPositionData.x)
            {
                transform.localScale = new Vector2(-1 * Math.Abs(transform.localScale.x), transform.localScale.y);
            }
            else if (thisPositionData.x < targetPositionData.x)
            {
                transform.localScale = new Vector2(1 * Math.Abs(transform.localScale.x), transform.localScale.y);
            }
        }
        else // �ݴ�
        {
            if (thisPositionData.x > targetPositionData.x)
            {
                transform.localScale = new Vector2(1 * Math.Abs(transform.localScale.x), transform.localScale.y);
            }
            else if (thisPositionData.x < targetPositionData.x)
            {
                transform.localScale = new Vector2(-1 * Math.Abs(transform.localScale.x), transform.localScale.y);
            }
        }

        if (targetObj != null && targetObj.gameObject.activeSelf == false)
            targetObj = null;
    }

    private void OnAttack()
    {
        if (attackType == AttackType.Melee)
            Attack();
        else if (attackType == AttackType.Projectile)
            ShotArrow();
    }

    private void Attack() // �ִϸ��̼� �̺�Ʈ �Լ�
    {
        if (targetObj == null)
            return;

        if (targetObj.gameObject.activeSelf == false)
            return;

        int originalDamage = (int)attackMount;

        switch (_monsterType)
        {
            case monsterType.FirstAttack:

                FirstAttackMonster firstAttackMonster = GetComponent<FirstAttackMonster>();

                if (firstAttackMonster.isAlreadyFirstAttack == false)
                {
                    DealDamage(firstAttackMonster.firstAttack_Mount);
                    return;
                }

                break;
        }

        DealDamage(attackMount);
    }

    public void DealDamage(int amount)
    {
        if (targetObj.GetComponent<Player>() != null)
        {
            GameManager.Instance.playerScript.GetDamage(amount);
            GameManager.Instance.bulletController.SetTarget(gameObject.transform);
        }
        else if (targetObj.GetComponent<Friend>() != null)
        {
            Friend friend = targetObj.GetComponent<Friend>();
            friend.GetDamage(amount);
            GameManager.Instance.bulletController.SetTarget(gameObject.transform);
        }
    }

    public void DealDamage(float amount)
    {
        if (targetObj.GetComponent<Player>() != null)
        {
            GameManager.Instance.playerScript.GetDamage(amount);
            GameManager.Instance.bulletController.SetTarget(gameObject.transform);
        }
        else if (targetObj.GetComponent<Friend>() != null)
        {
            Friend friend = targetObj.GetComponent<Friend>();
            friend.GetDamage(amount);
            GameManager.Instance.bulletController.SetTarget(gameObject.transform);
        }
    }

    // Overload -> GetDamage(float) 
    public void GetDamage(float Damage, bool isSkil = false, appearTextEnum appearEnum = appearTextEnum.basic)
    {
        Health.ApplyModifier((int)-Damage);
        GameManager.Instance.appearTextManager.AppearText((int)Damage, transform, appearEnum);
    }


    string isAttack = "isAttack";
    string isFirst = "isFirst";
    string isDead = "isDead";

    private Coroutine m_AttackDelayCoroutine;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("knockback") && !isKnockedBack)
        {
            // ���� ���
            UnityEngine.Vector2 direction = (transform.position - collision.transform.position).normalized;

            // �˹� �ӵ� ����
            rigidbody2D.velocity = direction * knockbackForce;

            // �˹� ���� �ð� ����
            knockbackTimer = knockbackDuration;

            // �˹� �� ���·� ����
            isKnockedBack = true;
        }
    }


    public void _MoveSpeedDown(float speed_declineAmount, float duration = 2f)
    {
        GameManager.Instance.particleManager.PlayParticle(transform, transform, 10);
        StartCoroutine(MoveSpeedDown(speed_declineAmount, duration));
    }

    IEnumerator MoveSpeedDown(float speed_declineAmount, float duration)
    {
        callBackSpeed.Enqueue(moveSpeed);
        moveSpeed -= speed_declineAmount;
        moveSpeed = Mathf.Min(0.3f);

        yield return new WaitForSeconds(duration);

        moveSpeed = callBackSpeed.Dequeue();
    }

    void ShotArrow() // animation event
    {
        switch (_arrowType)
        {
            case ArrowType.Basic:

                ShotArrowStatic();
                break;

            case ArrowType.wideShot:

                ShotArrowStatic();
                const int angleAlpha = 10;

                for (int i = 1; i <= wideShotNum / 2; i++)
                {
                    ShotArrowWide(angleAlpha * i);
                }

                break;
        }
    }

    IEnumerator SequenceShot_Coroutine()
    {
        yield return new WaitForSeconds(0.3f);

        ShotArrowStatic();
    }

    private void ShotArrowWide(float alpha = 0)
    {
        if (targetObj == null)
            return;

        UnityEngine.Vector3 _targetPosition = targetObj.GetComponent<PositionData>().AimPosition.position;
        UnityEngine.Vector3 _firePosition = gameObject.GetComponent<PositionData>().firePosition.position;

        GameObject _bullet = Instantiate(Arrow, _firePosition, UnityEngine.Quaternion.identity);

        // �Ѿ��� ������ �����մϴ�.
        UnityEngine.Vector2 _bulletDirection = (_targetPosition - _firePosition).normalized;

        // ���� ����� ���� ���� ���͸� ������ ��ȯ�մϴ�.
        float _angle = Mathf.Atan2(_bulletDirection.y, _bulletDirection.x) * Mathf.Rad2Deg;

        // �Ѿ��� ȸ���� �����մϴ�.
        _bullet.transform.rotation = UnityEngine.Quaternion.Euler(new UnityEngine.Vector3(0, 0, _angle + alpha));

        // Rigidbody2D ������Ʈ�� �����ɴϴ�.
        Rigidbody2D _rb = _bullet.GetComponent<Rigidbody2D>();

        // ������ ����
        switch (_monsterType)
        {
            case monsterType.Basic:
                _bullet.GetComponent<ArrowScript>().damage = (int)attackMount;
                break;
            case monsterType.FirstAttack:
                FirstAttackMonster firstAttackMonster = GetComponent<FirstAttackMonster>();

                if (firstAttackMonster.isAlreadyFirstAttack == false)
                    _bullet.GetComponent<ArrowScript>().damage = firstAttackMonster.firstAttack_Mount;
                else
                    _bullet.GetComponent<ArrowScript>().damage = (int)attackMount;

                break;
        }

        // �߻� �������� �Ѿ��� �̵���ŵ�ϴ�.
        _rb.velocity = Vector3.forward * 5;
    }

    private void ShotArrowStatic(float alpha = 0)
    {
        if (targetObj == null)
            return;

        var _targetPosition = targetObj.GetComponent<PositionData>().AimPosition.position;
        var _firePosition = gameObject.GetComponent<PositionData>().firePosition.position;

        var _bullet = Instantiate(Arrow, _firePosition, Quaternion.identity);

        // �Ѿ��� ������ �����մϴ�.
        var _bulletDirection = (_targetPosition - _firePosition).normalized;

        // ���� ����� ���� ���� ���͸� ������ ��ȯ�մϴ�.
        float _angle = Mathf.Atan2(_bulletDirection.y, _bulletDirection.x) * Mathf.Rad2Deg;

        // �Ѿ��� ȸ���� �����մϴ�.
        _bullet.transform.rotation = Quaternion.Euler(new Vector3(0, 0, _angle + alpha));

        // Rigidbody2D ������Ʈ�� �����ɴϴ�.
        var _rb = _bullet.GetComponent<Rigidbody2D>();

        // ������ ����
        switch (_monsterType)
        {
            case monsterType.Basic:
                _bullet.GetComponent<ArrowScript>().damage = (int)attackMount;
                break;
            case monsterType.FirstAttack:
                FirstAttackMonster firstAttackMonster = GetComponent<FirstAttackMonster>();

                if (firstAttackMonster.isAlreadyFirstAttack == false)
                    _bullet.GetComponent<ArrowScript>().damage = firstAttackMonster.firstAttack_Mount;
                else
                    _bullet.GetComponent<ArrowScript>().damage = (int)attackMount;

                break;
        }

        // �߻� �������� �Ѿ��� �̵���ŵ�ϴ�.
        _rb.velocity = _bulletDirection * 5;
    }

    public void DotDamage(int dotDamage, int particleIndex, float delay)
    {
        StartCoroutine(DotDeal_Corutine(dotDamage, particleIndex, delay));
    }

    IEnumerator DotDeal_Corutine(int _dotDamage, int _particleIndex, float intervalDelay = 1f, int repeatCount = 4)
    {
        for (int i = 0; i < repeatCount; i++)
        {
            GetDamage(_dotDamage);
            GameManager.Instance.appearTextManager.AppearText(_dotDamage, transform);
            GameManager.Instance.particleManager.PlayParticle(transform,
                GameManager.Instance.particleManager.particleObjects[_particleIndex].transform
                , _particleIndex, 2);
            yield return new WaitForSeconds(intervalDelay);
        }
    }

    public void KnockDown(float duration)
    {
        StartCoroutine(KnockDown_Corutine(duration));
    }

    IEnumerator KnockDown_Corutine(float duration)
    {
        isKnockDown = true;
        // isStop = true;
        AllOfAnimation_False();
        Anim.speed = 0f;
        GameManager.Instance.particleManager.PlayParticle(transform, transform, 14, duration);
        yield return new WaitForSeconds(duration);
        Anim.speed = 1f;
        // isStop = false;
        isKnockDown = false;
    }

    public void trickOn(float duration, Transform activeFalseToTransform = null)
    {
        StartCoroutine(trick_Coroutine(duration, activeFalseToTransform));
    }

    private IEnumerator trick_Coroutine(float duration, Transform t)
    {
        // isStop = true;
        Anim.speed = 0f;

        yield return new WaitForSeconds(duration);

        if (t != null)
            t.gameObject.SetActive(false);

        Anim.speed = 1f;
        // isStop = false;
    }

    private void AllOfAnimation_False()
    {
        // Animator�� ��� Bool �Ķ���� �̸� ��������
        AnimatorControllerParameter[] parameters = Anim.parameters;
        foreach (var parameter in parameters)
        {
            if (parameter.type == AnimatorControllerParameterType.Bool)
            {
                // �� Bool �Ķ���͸� false�� ����
                Anim.SetBool(parameter.name, false);
            }
        }
    }


    public void isJumpMoveStart() // anim event
    {
        if (transform.GetChild(1).GetComponent<JumpMonster>() != null)
        {
            transform.DOMove(targetObj.position, 0.8f);
        }
    }

    public void isJumpMoveStop() // anim event
    {
        if (transform.GetChild(1).GetComponent<JumpMonster>() != null)
        {
            transform.GetChild(1).GetComponent<JumpMonster>().isJumpMoveStop = true;
        }
    }

    public void OnDead()
    {
        GameManager.Instance.uIManager.DropReward(dropEXP, dropBloodStone, dropGold, _monsterType);
        GameManager.Instance.playerScript.SetCatchsOfMonsters(1);
        GameManager.Instance.monsterControll.nuberofCatchs++;

        gameObject.SetActive(false);
    }
}