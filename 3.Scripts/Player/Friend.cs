using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Friend : MonoBehaviour
{
    public Monster searchObj;
    private Monster targetObj;
    private Transform hpBar;
    public SpriteRenderer hpBarSprite; // Reference to the SpriteRenderer for the HP bar

    public float maxHealth = 100f, moveSpeed = 2f; // Maximum health
    public float currentHealth = 100f; // Current health

    public float attackAmount = 30f;

    Animator animator;

    bool isStop = false;
    public bool isExeption = false;

    public float showHpBar_coolTime = 2f;
    public float showHpBar_Time = 0;
    private float tempOriginalCurHp;

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

        if (currentHealth <= 0)// ģ�� ���
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
        // bulletcontroller �� ����ִ� target
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
            searchObj = GameManager.bulletController.RandMonster();
        }


    }

    public void Attack()
    {
        targetObj.GetDamage(attackAmount,true);
    }

    public void Suffer() // event
    {
        OffAnimator(isSuffer);
    }

    public void GetDamage(float Damage)
    {
        currentHealth -= Damage;
        OnAnimator(isSuffer);
        GameManager.appearTextManager.AppearText((int)Damage, transform);
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

    void stopFalse()
    {
        isStop = false;
    }

    float healthDeclineDuration = 0.3f;
    void UpdateHPBar()
    {
        // Calculate the scale based on current health
        float scaleX = currentHealth / maxHealth;

        // Update the SpriteRenderer's scale
        if (hpBarSprite != null)
            hpBarSprite.transform.DOScaleX(scaleX, healthDeclineDuration);

    }
}
