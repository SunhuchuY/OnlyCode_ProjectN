using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Linq;
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

    [SerializeField] public float moveSpeed = 5f; // Adjust the speed as needed
    [SerializeField] public float maxHealth = 100f;

    [SerializeField] public string dropEXPString;
    [SerializeField] public string dropBloodStoneString;
    [SerializeField] public string dropGoldString;

    private BigInteger dropEXP = 50, dropBloodStone = 1250, dropGold = 10; // ���?

    [SerializeField] public float attackMount = 60;
    [SerializeField] public int wideShotNum = 3;
    [SerializeField] public AttackType attackType = AttackType.Melee;
    [SerializeField] public monsterType monsterType = monsterType.Basic;
    [SerializeField] public ArrowType arrowType = ArrowType.Basic;
    [SerializeField] public float attackRange = 2f;

    [SerializeField] public GameObject Arrow;
    [SerializeField] public bool isLookLeft = false;

    public Animator Anim { get; private set; }
    public Rigidbody2D rigidbody2D { get; private set; }
    public Attribute Health { get; private set; }
    public StateMachine Fsm { get; private set; }

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

    private void Awake()
    {
        Anim = GetComponentInChildren<Animator>();
        rigidbody2D = GetComponent<Rigidbody2D>();
        firePoint = gameObject.Descendants().FirstOrDefault(x => x.name == "FirePoint")?.transform;
        Health = new Attribute();
        Health.Cap = (int)maxHealth;
        Health.Initialize();
        originalmoveSpeed = moveSpeed;

        if (GetComponentInChildren<MonsterAnimationKeyframeEventReceiver>() is { } _keyframeEventReceiver)
        {
            // �ִϸ��̼� ���?�� Ű������ �ִϸ��̼��� �߻����� �� �����?�ݹ��� ����մϴ�?
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
            dropEXP = BigInteger.Parse(dropEXPString);
            dropGold = BigInteger.Parse(dropGoldString);
            dropBloodStone = BigInteger.Parse(dropBloodStoneString);
        }
        catch (FormatException ex)
        {
            Debug.LogError("drop EXP, Gold, BloodStone Parse Error: " + ex.Message);
        }
    }

    private void OnEnable()
    {
        player = GameManager.Instance.player.transform;
        targetObj = player; // temp: �ӽ������� ���� ������ �����?�÷��̾��?�����մϴ�.

        moveSpeed = originalmoveSpeed;
        moveSpeedStack.Clear();

        // �ִϸ��̼� ���?���¸� �ʱ��?�ǵ����ϴ�.
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

        // �� �������� �ٶ󺸵��� scale.x�� ������ flip ���θ� �����մϴ�.
        int lookDirection = isLookLeft ? -1 : 1;
        int toTargetDirection = transform.position.x > targetObj.transform.position.x ? -1 : 1;

        transform.localScale = new Vector2(
            Mathf.Abs(transform.localScale.x) * lookDirection * toTargetDirection,
            transform.localScale.y);

        // �������?��ƼŬ�� ��ġ�� �����մϴ�.
        if (followParticleTransform != null)
            followParticleTransform.position = transform.position - UnityEngine.Vector3.up;
    }

    private void OnAttack()
    {
        if (attackType == AttackType.Melee)
            Attack();
        else if (attackType == AttackType.Projectile)
            ShotArrow();
    }

    private void OnDead()
    {
        GameManager.Instance.userDataManager.GetReward(dropEXP, dropBloodStone, dropGold, monsterType);
        gameObject.DescendantsAndSelf().OfType<Collider2D>().ToList().ForEach(x => x.enabled = false);
        Destroy(gameObject, 0.3f);
    }

    private void Attack()
    {
        if (targetObj == null)
            return;

        if (targetObj.gameObject.activeSelf == false)
            return;

        int originalDamage = (int)attackMount;

        switch (monsterType)
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
        }
        else if (targetObj.GetComponent<Friend>() != null)
        {
            Friend friend = targetObj.GetComponent<Friend>();
            friend.GetDamage(amount);
        }
    }

    public void DealDamage(float amount)
    {
        if (targetObj.GetComponent<Player>() != null)
        {
            GameManager.Instance.playerScript.GetDamage((int)amount);
        }
        else if (targetObj.GetComponent<Friend>() != null)
        {
            Friend friend = targetObj.GetComponent<Friend>();
            friend.GetDamage(amount);
        }
    }

    // Overload -> GetDamage(float)
    public void GetDamage(float _damage, bool isSkil = false, appearTextEnum appearEnum = appearTextEnum.basic)
    {
        Health.ApplyModifier((int)-_damage);
        GameManager.Instance.appearTextManager.AppearText((int)_damage, transform, appearEnum);
    }


    string isAttack = "isAttack";
    string isFirst = "isFirst";
    string isDead = "isDead";

    private Coroutine m_AttackDelayCoroutine;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // �Ÿ��� ����� ����� �켱��
        if (Vector2.Distance(transform.position, targetObj.position) <
            Vector2.Distance(transform.position, collision.transform.position))
            return;

        if (!collision.CompareTag("Player"))
            return;

        // ��, �÷��̾�, Ÿ�� �ν� -> Ÿ�� ����
        targetObj = collision.transform;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("knockback") && !isKnockedBack)
        {
            // ���� ���?
            UnityEngine.Vector2 direction = (transform.position - collision.transform.position).normalized;

            // �˹� �ӵ� ����
            rigidbody2D.velocity = direction * knockbackForce;

            // �˹� ���� �ð� ����
            knockbackTimer = knockbackDuration;

            // �˹� �� ���·� ����
            isKnockedBack = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // ��, �÷��̾�, Ÿ���� ���� ��� Ÿ�� �ʱ�ȭ
        if (collision.transform == targetObj)
            targetObj = player;
    }


    public void MoveSpeedDown(float speed_declineAmount, float duration = 2f)
    {
        StartCoroutine(MoveSpeedDownCoroutine(speed_declineAmount, duration));
    }

    IEnumerator MoveSpeedDownCoroutine(float speed_declineAmount, float duration)
    {
        moveSpeedStack.Push(moveSpeed);
        moveSpeed -= speed_declineAmount;
        moveSpeed = Mathf.Min(0.3f);

        yield return new WaitForSeconds(duration);

        moveSpeed = moveSpeedStack.Peek();
    }

    void ShotArrow()
    {
        switch (arrowType)
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

    private void ShotArrowWide(float alpha = 0)
    {
        if (targetObj == null)
            return;

        GameObject _bullet = Instantiate(Arrow, firePoint.position, UnityEngine.Quaternion.identity);

        // �Ѿ��� ������ �����մϴ�.
        UnityEngine.Vector2 _bulletDirection = (targetObj.position - firePoint.position).normalized;

        // ���� �����?���� ���� ���͸� ������ ��ȯ�մϴ�.
        float _angle = Mathf.Atan2(_bulletDirection.y, _bulletDirection.x) * Mathf.Rad2Deg;

        // �Ѿ��� ȸ���� �����մϴ�.
        _bullet.transform.rotation = UnityEngine.Quaternion.Euler(new UnityEngine.Vector3(0, 0, _angle + alpha));

        // Rigidbody2D ������Ʈ�� �����ɴϴ�.
        Rigidbody2D _rb = _bullet.GetComponent<Rigidbody2D>();

        // ������ ����
        switch (monsterType)
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

        GameObject _bullet = Instantiate(Arrow, firePoint.position, UnityEngine.Quaternion.identity);

        // �Ѿ��� ������ �����մϴ�.
        UnityEngine.Vector2 _bulletDirection = (targetObj.position - firePoint.position).normalized;

        // ���� �����?���� ���� ���͸� ������ ��ȯ�մϴ�.
        float _angle = Mathf.Atan2(_bulletDirection.y, _bulletDirection.x) * Mathf.Rad2Deg;

        // �Ѿ��� ȸ���� �����մϴ�.
        _bullet.transform.rotation = Quaternion.Euler(new Vector3(0, 0, _angle + alpha));

        // Rigidbody2D ������Ʈ�� �����ɴϴ�.
        var _rb = _bullet.GetComponent<Rigidbody2D>();

        // ������ ����
        switch (monsterType)
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

    public void DotDamage(int dotDamage, string _particleAddress, float delay)
    {
        StartCoroutine(DotDeal_Corutine(dotDamage, _particleAddress, delay));
    }

    IEnumerator DotDeal_Corutine(int _dotDamage, string _particleAddress, float intervalDelay = 1f, int repeatCount = 4)
    {
        for (int i = 0; i < repeatCount; i++)
        {
            GetDamage(_dotDamage);
            GameManager.Instance.appearTextManager.AppearText(_dotDamage, transform);
            GameManager.Instance.objectPoolManager.PlayParticle(_particleAddress, transform.position);
            yield return new WaitForSeconds(intervalDelay);
        }
    }

    public void KnockDown(float duration)
    {
        StartCoroutine(KnockDown_Corutine(duration));
    }

    IEnumerator KnockDown_Corutine(float duration)
    {
        Anim.speed = 0f;
        GameManager.Instance.objectPoolManager.PlayParticle("Prefab/Particle/15", transform.position);
        yield return new WaitForSeconds(duration);
        Anim.speed = 1f;
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
}