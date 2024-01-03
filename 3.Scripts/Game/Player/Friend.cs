using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System;

public class Friend : MonoBehaviour
{
    public Monster searchObj;
    Monster targetObj;
    Transform hpBar;
    public SpriteRenderer hpBarSprite; // Reference to the SpriteRenderer for the HP bar

    public float maxHealth = 100f, moveSpeed = 2f; // Maximum health
    public float currentHealth = 100f; // Current health

    public float attackAmount = 30f;

    Animator animator;

    public bool isExeption = false;

    bool isStop = false;

    public float showHpBar_coolTime = 2f;
    public float showHpBar_Time = 0;

    float tempOriginalCurHp;
    Stack<float> attackSpeedStack = new Stack<float>();

    public type friendType = type.tower;


    private void Start()
    {
        hpBar = hpBarSprite.transform.parent;
        currentHealth = maxHealth;

        if(GetComponent<Animator>() != null)
            animator = GetComponent<Animator>();


        switch (friendType)
        {
            case type.tower:
                isStop = true;
                isExeption = true;
                break;
        }
    }

    private void Update()
    {

        if (!isStop)
        {
            MoveTowardsPlayer();
        }

        if (currentHealth <= 0)// 친구 사망
        {
            gameObject.SetActive(false);
        }

        if (tempOriginalCurHp != currentHealth)
        {
            showHpBar_Time += Time.deltaTime;
            hpBar.gameObject.SetActive(true);

            if (showHpBar_Time > showHpBar_coolTime)
            {
                showHpBar_Time = 0;
                hpBar.gameObject.SetActive(false);
                tempOriginalCurHp = currentHealth;
            }
        }

        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);

        // Update the HP bar
        UpdateHPBar();
    }

    void MoveTowardsPlayer()
    {
        // Calculate the direction from monster to player
        // bulletcontroller 가 잡고있는 target
        if(searchObj != null)
        {
            if(searchObj.gameObject.activeSelf == false)
            {
                searchObj = null;
            }
            else
            {
                Vector3 direction = (searchObj.transform.position - transform.position).normalized;
                transform.Translate(direction * moveSpeed * Time.deltaTime);
            }
        }
        else
        {
            searchObj = GameManager.Instance.bulletController.RandMonster();
        }


    }

    public void Attack()
    {
        if(targetObj !=null)
            targetObj.GetDamage(attackAmount,true);
    }

    public void Suffer() // event
    {
        OffAnimator(isSuffer);
    }

    public void GetDamage(float Damage)
    {
        if (Damage < 0)
            Damage = 1;

        currentHealth -= Damage;
        GameManager.Instance.appearTextManager.AppearText((int)Damage, transform);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        switch (collision.tag)
        {
            case "Monster":
                if(collision != null && currentHealth != 0)
                {
                    targetObj = collision.GetComponent<Monster>();
                    OnAnimator(isAttack);
                    isStop = true;
                }
                break;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        switch (collision.tag)
        {
            case "Monster":
                if (collision != null)
                {
                    OffAnimator(isAttack);
                    targetObj = null;
                }
                break;
        }
    }

    string isAttack = "isAttack";
    string isSuffer = "isSuffer";
    void OnAnimator(string animName)
    {
        if (animator != null && !isExeption)
        {
            animator.SetBool(animName, true);
            isStop = true;
        }
    }

    void OffAnimator(string animName)
    {
        if (animator != null && !isExeption)
        {
            animator.SetBool(animName, false);
        }
    }

    void stopFalse() // Animation Event
    {
        isStop = false;
    }

    float healthDeclineDuration = 0.3f;
    void UpdateHPBar()
    {
        float scaleX = currentHealth / maxHealth;

        if (hpBarSprite != null)
            hpBarSprite.transform.DOScaleX(scaleX, healthDeclineDuration);
    }

    public async UniTask ChangeAttackSpeed(float amount, float duration)
    {
        attackSpeedStack.Push(animator.speed);

        animator.speed += amount;
        await UniTask.Delay(TimeSpan.FromSeconds(duration));
        animator.speed = attackSpeedStack.Peek();
    }

}
