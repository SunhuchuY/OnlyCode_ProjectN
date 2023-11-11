using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;
using Vector2 = UnityEngine.Vector2;

public class Monster : MonoBehaviour
{
    public float moveSpeed = 5f; // Adjust the speed as needed

    // knock back
    private float knockbackForce = 7f;
    private float knockbackDuration = 0.2f;

    private bool isKnockedBack = false;
    private float knockbackTimer;

    Animator animator;
    Rigidbody2D rigidbody2D;

    public int wideShotNum = 3;

    float rangeX = 12f;
    float rangeY = 12f;

    [HideInInspector] public bool isStop = false; // ex) ���� �ǰݴ�������, �����ϴ����϶�

    private Transform player;
    [HideInInspector] public Transform targetObj;

    public GameObject hpBar;
    public SpriteRenderer hpBarSprite; // Reference to the SpriteRenderer for the HP bar
    public float maxHealth = 100f; // Maximum health
    public float currentHealth = 100f; // Current health

    float healthDeclineDuration = 0.5f;

    [SerializeField] private string _dropEXP, _dropBloodStone, _dropGold; // ���
    public BigInteger dropEXP = 50, dropBloodStone = 1250, dropGold = 10; // ���
    public float attackMount=60;

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
    


    private void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();

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
        currentHealth = maxHealth;
        // Find the player GameObject by tag
        player = GameManager.player.transform;
        animator = GetComponent<Animator>();
        GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
        tempOriginalCurHp = maxHealth;
        GetComponent<BoxCollider2D>().enabled = true;

        AllOfAnimation_False();
        isStop = false;

    }

    private void OnDisable()
    {
        GameManager.bulletController.RemoveMonster(gameObject);
    }

    void Update()
    {
        // Move the monster towards the player
        if (!isStop)
        {
            MoveTowardsPlayer();
        }

        if (currentHealth <= 0)// ���� ���
        {
            
            GetComponent<BoxCollider2D>().enabled = false;
            OnAnimator(isDead);
        }


        if (tempOriginalCurHp != currentHealth)
        {
            showHpBar_Time += Time.deltaTime;
            hpBar.SetActive(true);


            if (showHpBar_Time > showHpBar_coolTime)
            {
                showHpBar_Time = 0;
                hpBar.SetActive(false);
                tempOriginalCurHp = currentHealth;
            }
        }


        // Ensure health doesn't go below zero
        currentHealth = Mathf.Clamp(currentHealth, 0f,maxHealth);

        // Update the HP bar
        UpdateHPBar();

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

        if(followParticleTransform != null)
            followParticleTransform.position = transform.position - UnityEngine.Vector3.up;

    }

    private void FixedUpdate()
    {
        Vector2 targetPositionData = Vector2.zero;
        Vector2 thisPositionData = gameObject.GetComponent<PositionData>().AimPosition.position;

        if (targetObj == null || targetObj.gameObject.activeSelf == false)
            targetPositionData = GameManager.player.GetComponent<PositionData>().AimPosition.position;
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
        else    // �ݴ�
        {
            if (thisPositionData.x > targetPositionData.x)
            {
                transform.localScale = new Vector2(1 *Math.Abs(transform.localScale.x), transform.localScale.y);
            }
            else if (thisPositionData.x < targetPositionData.x)
            {
                transform.localScale = new Vector2(-1 * Math.Abs(transform.localScale.x), transform.localScale.y);
            }
        }

        if (targetObj != null && targetObj.gameObject.activeSelf == false)
            targetObj = null;
    }

    void MoveTowardsPlayer()
    {
        // Calculate the direction from monster to player
        UnityEngine.Vector3 direction = (player.position - transform.position).normalized;

        // Move the monster towards the player
        transform.Translate(direction * moveSpeed * Time.deltaTime);
    }

    void Attack() // �ִϸ��̼� �̺�Ʈ �Լ�
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
            GameManager.playerScript.GetDamage(amount);
            GameManager.bulletController.SetTarget(gameObject.transform);
        }
        else if (targetObj.GetComponent<Friend>() != null)
        {
            Friend friend = targetObj.GetComponent<Friend>();
            friend.GetDamage(amount);
            GameManager.bulletController.SetTarget(gameObject.transform);
        }
    }

    public void DealDamage(float amount)
    {
        if (targetObj.GetComponent<Player>() != null)
        {
            GameManager.playerScript.GetDamage(amount);
            GameManager.bulletController.SetTarget(gameObject.transform);
        }
        else if (targetObj.GetComponent<Friend>() != null)
        {
            Friend friend = targetObj.GetComponent<Friend>();
            friend.GetDamage(amount);
            GameManager.bulletController.SetTarget(gameObject.transform);
        }
    }

    // Overload -> GetDamage(float) 
    public void GetDamage(float Damage, bool isSkil = false , appearTextEnum appearEnum = appearTextEnum.basic)
    {
        currentHealth -= Damage;
        GameManager.appearTextManager.AppearText((int)Damage, transform , appearEnum);
        ShowHpBar();
    }


    public void ShowHpBar()
    {
        isShowHpBar = true;
        hpBar.SetActive(isShowHpBar);
    }



    string isAttack= "isAttack";
    string isFirst = "isFirst";
    string isDead = "isDead";

    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.tag)
        {
        }
    }
    

    private void OnTriggerStay2D(Collider2D collision)
    {
        switch (collision.tag)
        {
            case "Player":
                OnAnimator(isAttack);
                targetObj = collision.transform;
                isStop = true;
                break;

        }

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

    public void OnAnimator(string animName, bool isStop_state= true)
    {
        if (animator == null)
            return;

        if (isKnockDown == true)
            return;

        animator.SetBool(animName, true);
        isStop = isStop_state;
    }

    public void OffAnimator(string animName)
    {
        if (animator != null)
        {
            animator.SetBool(animName, false);
        }
    }

    void stopFalse()
    {
        if (!isKnockDown && targetObj == null)
        {
            AllOfAnimation_False();
            isStop = false;
        }
    }


    void UpdateHPBar()
    {
        // Calculate the scale based on current health
        float scaleX = currentHealth / maxHealth;

        // Update the SpriteRenderer's scale
        if(hpBarSprite!= null)
        hpBarSprite.transform.DOScaleX(scaleX, healthDeclineDuration);

    }

    public void CounterAttack_Suffer()
    {
        if (animator != null)
        {
            isShowHpBar = true;
            hpBar.SetActive(isShowHpBar);
        }
    }


    private Transform followParticleTransform;
    public void _MoveSpeedDown(float speed_declineAmount, float duration = 2f)
    {
        GameManager.particleManager.PlayParticle(transform, transform, 10);
        StartCoroutine(MoveSpeedDown(speed_declineAmount, duration));
    }
    IEnumerator MoveSpeedDown(float speed_declineAmount ,float duration )
    {
        followParticleTransform = GameManager.particleManager.GetparticleTransform();

        float originalMoveSpeed = moveSpeed;
        moveSpeed -= speed_declineAmount;

        yield return new WaitForSeconds(duration);

        moveSpeed = originalMoveSpeed;
        followParticleTransform = null;
    }

    void ShotArrow() // animation event
    {
        switch (_arrowType)
        {
            case ArrowType.Basic:

                ShotArrowStatic();
                break;

            case ArrowType.wideShot:

                ShotArrowStatic(90);

                for (int i = 1; i < wideShotNum; i++)
                {
                    StartCoroutine(SequenceShot_Coroutine());
                }
                break;
        }
    }

    IEnumerator SequenceShot_Coroutine()
    {


        yield return new WaitForSeconds(0.3f);

        ShotArrowStatic();
    }

    private void ShotArrowStatic(float alpha = 0)
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

                if(firstAttackMonster.isAlreadyFirstAttack == false)
                    _bullet.GetComponent<ArrowScript>().damage = firstAttackMonster.firstAttack_Mount;
                else
                    _bullet.GetComponent<ArrowScript>().damage = (int)attackMount;

                break;
        }

        // �߻� �������� �Ѿ��� �̵���ŵ�ϴ�.
        _rb.velocity = _bulletDirection * 5;
    }

    public void DotDamage(int dotDamage, int particleIndex )
    {
        StartCoroutine(DotDeal_Corutine(dotDamage, particleIndex));
    }

    IEnumerator DotDeal_Corutine(int _dotDamage ,int _particleIndex ,int repeatCount = 4)
    {
        float intervalDelay = 1f;

        for (int i = 0; i < repeatCount; i++)
        {
            GetDamage(_dotDamage);
            GameManager.appearTextManager.AppearText(_dotDamage, transform);
            GameManager.particleManager.PlayParticle(transform, GameManager.particleManager.particleObjects[_particleIndex].transform
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
        isStop = true;
        AllOfAnimation_False();
        animator.speed = 0f;
        GameManager.particleManager.PlayParticle(transform, transform, 14,duration);
        yield return new WaitForSeconds(duration);
        animator.speed = 1f;
        isStop = false;
        isKnockDown = false;
    }

    public void trickOn(float duration, Transform activeFalseToTransform = null)
    {
        StartCoroutine(trick_Coroutine(duration, activeFalseToTransform));
    }

    IEnumerator trick_Coroutine(float duration , Transform t)
    {
        isStop = true;
        animator.speed = 0f;

        yield return new WaitForSeconds(duration);

        if (t != null)
            t.gameObject.SetActive(false);

        animator.speed = 1f;
        isStop = false;

    }



    IEnumerator OffAnimatorDelay(float delay, string animName)
    {
        yield return new WaitForSeconds(delay);
        OffAnimator(animName);
    }

    void AllOfAnimation_False()
    {
        // Animator�� ��� Bool �Ķ���� �̸� ��������
        AnimatorControllerParameter[] parameters = animator.parameters;
        foreach (var parameter in parameters)
        {
            if (parameter.type == AnimatorControllerParameterType.Bool)
            {
                // �� Bool �Ķ���͸� false�� ����
                animator.SetBool(parameter.name, false);
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

    public void Dead() // anim event
    {
        GameManager.uIManager.DropReward(dropEXP, dropBloodStone, dropGold, _monsterType);
        GameManager.playerScript.SetCatchsOfMonsters(1);
        GameManager.monsterControll.nuberofCatchs++;

        gameObject.SetActive(false);
    }

}