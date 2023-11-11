using DG.Tweening;
using System.Collections;
using System.Numerics;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

using Vector3 = UnityEngine.Vector3;

public class Player : MonoBehaviour
{
    public Image hpBarImage; // Reference to the SpriteRenderer for the HP bar

    private Animator animator;

    [SerializeField] StatisticScreen statisticScreen;

    [SerializeField] TMP_Text currentHp_Text;
    [SerializeField] TMP_Text manaText;

    [SerializeField] GameObject[] manaArray = new GameObject[10];

    double maxHealth = 3000; // Maximum health
    [HideInInspector] public int maxHealthLevel = 1; // Maximum health
    [HideInInspector] public double forsecondsUpAmount = 10; // Maximum health
    [HideInInspector] public int forsecondsUpAmountLevel = 1; // Maximum health


    float playerScale;

    bool isDead = false;

    public bool GetisDead() { {  return isDead; } }

    public double GetMaxHealth() { return maxHealth; }
    public void SetMaxHealth(double input) { maxHealth += input; }
    public CounterAttack counterAttack;

    double currentHealth = 100f; // Current health
    public double GetcurrentHealth() { return currentHealth; }
    public void SetcurrentHealth(double input) { if(currentHealth < maxHealth) currentHealth += input; }


    // mana
    float curManaRate = 0f, manaFill_curTime = 0, manaFill_CoolTime = 0.01f, manafill_Amount = 0.01f; // Rate는 충전, Mana는 현재마나
    const float maxMana = 10f;
    int curMana = 0;

    // critical
    float criticalPercent = 1; // %
    float criticalAddDamagePercent = 1; // %
    [HideInInspector] public int criticalPercentLevel = 1 , criticalAddDamagePercentLevel = 1;
    bool isTemporaryCriticalPersent_Up = false;

    // defense
    float defensePercent = 0; // %
    float defenseAmount = 0;
    [HideInInspector] public int defenseAmountLevel = 1;
     bool isTemporaryDeffensePersent_Up = false;

    // attack
    [HideInInspector] public float attack = 20f; // PER // 
    [HideInInspector] public float attackRankge = 1f;  // PER //
    [HideInInspector] public int attackLevel = 1, attackRankgeLevel = 1, attackSpeedLevel = 1;
    [HideInInspector] public float attackAmplify_Percent = 0; // % //


    private float _initRange_X;
    public float initRange_X
    {
        get { return _initRange_X; }
    }

    private float _initRange_Y;
    public float initRange_Y
    {
        get { return _initRange_Y; }
    }

    private float _upRange = 0.5f;
    public float upRange
    {
        get { return _upRange; }
    }   


    int counterAttack_Damage = 10; // PER //
    [HideInInspector] public int counterAttack_DamageLevel = 1;

    public void SetcounterAttack_Damage(int input) { counterAttack_Damage += input; }


    // quest

    private Tween fadeTween;
    float fadeDuration = 0.6f;
    float healthDeclineDuration = 0.6f;
    float emergencyHealthMount = 0.3f; // 피가 emegencyHealth 이하 비율이면 애니메이션
    private Color originalHpBarColor;
    private bool isEmergency = false;
    void SetisEmergency(bool value) { isEmergency = value; }

    private int sumCatchsOfMonsters = 0;
    float scaleXHp, scaleXMana;

    public int GetCatchsOfMonsters() { return sumCatchsOfMonsters; }
    public void SetCatchsOfMonsters(int input) { sumCatchsOfMonsters += input; }


    // mana
    public void SetMana(int input) { curManaRate += input; }
    public int GetMana() { return curMana; }

    public void FullManaSet() { curManaRate = 10; }
    public void FillManaSpeed_Up(float change_fillAmount, float duration) { StartCoroutine(Temporary_ManaFillSpeed_Up(change_fillAmount, duration)); }

    // critical
    public float GetCriticalAddDamage() { return criticalAddDamagePercent; }
    public float GetCriticalPercent() { return criticalPercent; }
    public void SetCriticalPercent(float input) { criticalPercent += input; }
    public void SetCriticalAddDamage(float input) { criticalAddDamagePercent += input; }
    public void TemporaryCriticalPersent_Up(float upDefenseMount, float duration)
    { StartCoroutine(TemporaryDeffensePersent_Up_Coroutine(upDefenseMount, duration)); }

    IEnumerator TemporaryCriticalPercent_Up_Coroutine(float upCriticalPercent, float duration)
    {

        if (isTemporaryCriticalPersent_Up== false)
        {
            isTemporaryCriticalPersent_Up = true;
            float originalDefenseMount = criticalPercent;
            criticalPercent += upCriticalPercent;

            yield return new WaitForSeconds(duration);

            criticalPercent = originalDefenseMount;
            isTemporaryCriticalPersent_Up = false;
        }

    }



    // Defense
    public void SetDefensePercent(float input) { defensePercent += input; }
    public float GetDefensePercent() { return defensePercent;  }
    public void SetDefenseAmount(float input) { defenseAmount += input; }
    public float GetDefenseAmount() { return defenseAmount; }




    public void TemporaryDeffensePersent_Up(float upDefenseMount, float duration) 
    { StartCoroutine(TemporaryDeffensePersent_Up_Coroutine(upDefenseMount,duration)); }

    IEnumerator TemporaryDeffensePersent_Up_Coroutine(float upDefenseMount ,float duration)
    {

        if(isTemporaryDeffensePersent_Up == false)
        {
            isTemporaryDeffensePersent_Up = true;
            float originalDefenseMount = defensePercent;
            defensePercent += upDefenseMount;
            hpBarImage.color = Color.gray;

            yield return new WaitForSeconds(duration);

            hpBarImage.color = originalHpBarColor;
            defensePercent = originalDefenseMount;
            isTemporaryDeffensePersent_Up = false;
        }

    }


    // mana
    IEnumerator Temporary_ManaFillSpeed_Up(float _change_fillAmount, float _duration)
    {
        float original_manafill_Amount = manafill_Amount;
        manafill_Amount = _change_fillAmount;
        yield return new WaitForSeconds(_duration);
        manafill_Amount = original_manafill_Amount;

    }

    // invinicible(무적)
    bool invinicible = false;
    public void Temporary_invinicibleTurnOn(float duration) { StartCoroutine(Temporary_invinicibleTurnOn_Corutine(duration)); } // 무적 On
    IEnumerator Temporary_invinicibleTurnOn_Corutine(float _duration)
    {
        hpBarImage.color = Color.blue;
        invinicible = true;
        yield return new WaitForSeconds(_duration);
        invinicible = false;
        hpBarImage.color = originalHpBarColor ;
    }

    // attack
    bool isAttackAmplify = false;
    public void TemporaryAttackAmplify(float replace_amountPercent, float duration) { StartCoroutine(TemporaryAttackAmplify_Coroutine(replace_amountPercent,duration)); }

    IEnumerator TemporaryAttackAmplify_Coroutine(float replace_amountPercent , float duration)
    {
        if(isAttackAmplify == false)
        {
            isAttackAmplify = true;
            float original_AttackAmplify = attackAmplify_Percent;
            attackAmplify_Percent = replace_amountPercent;
            yield return new WaitForSeconds(duration);
            attackAmplify_Percent = original_AttackAmplify;
            isAttackAmplify = false;
        }
    }

    // quest



    // per은 permanent 약자
    public float per_maxhp= 0, per_counter = 0, per_attack = 0, per_range = 0, per_speed = 0;

    public float sumAttack() 
    {
        float sum; 

        if(!isAttackAmplify) // 증폭 없음
            sum = attack+per_attack;
        else
        {
            sum = (int)((attack + per_attack) + ((attack + per_attack) * (attackAmplify_Percent / 100)));
        }       

        return sum * GameManager.uIManager.level;
    }

    public float sumRange() { return attackRankge + per_range; }
    public float sumCounter() { return counterAttack_Damage + per_counter; }
    public double sumMaxhp() { return maxHealth + per_maxhp; }
    public float sumSpeed() { return GameManager.bulletController.GetCoolTime() + per_speed; }

    public int sumCriticalAttack() { return (int)(sumAttack() + ( sumAttack() * (criticalAddDamagePercent / 100) ) - defenseAmount); }










    void Start()
    {
        animator = GetComponent<Animator>();
        _initRange_X = GameManager.bulletController.transform.localScale.x;
        _initRange_Y = GameManager.bulletController.transform.localScale.y;
        playerScale = transform.localScale.x;

        // Initialize the HP bar
        currentHealth = maxHealth;
        curManaRate = 0;
        GameManager.bulletController.gameObject.transform.localScale = new UnityEngine.Vector2(initRange_X, initRange_Y);
        originalHpBarColor = hpBarImage.color;

        criticalPercent = Mathf.Clamp(criticalPercent, 1f, 100f);
        defensePercent = Mathf.Clamp(defensePercent, 1f, 100f);

    }


    const float maxtime = 10f;
    float time = 0f;
    void Update()
    {
        UpdateHPBar();
        currentHp_Text.text = $"{(int)currentHealth}";
        UpdateManaBar();
        time += Time.deltaTime;

        if (time > maxtime)
            HealCurHp();
        

        // player Dead
        if(currentHealth <= 0 && !isDead)
        {
            StatePlayer(PlayerStateEnum.Dead);
            GameManager.statisticScreen.RestartBigStage();
        }
    }

    void HealCurHp()
    {
        SetcurrentHealth(forsecondsUpAmount);
        time = 0f;
    }

    void UpdateManaBar()
    {
        curManaRate = Mathf.Clamp(curManaRate, 0f, 10f);

        curManaRate += manafill_Amount;
        curMana = (int)Mathf.Floor(curManaRate);

        for (int i = 0; i < manaArray.Length; i++)
        {
            manaArray[i].SetActive(false);

            if (i < curMana)
            {
                manaArray[i].SetActive(true);
            }
        }

        manaText.text = $"cost : {curMana}";
        scaleXMana = curManaRate / maxMana;
    }
    
    void UpdateHPBar()
    {
        currentHealth = Mathf.Max((float)currentHealth, 0f);

        // Calculate the scale based on current health
        float scaleXHp = (float)(currentHealth / maxHealth);

        // Update the SpriteRenderer's scale
        hpBarImage.transform.DOScaleX(scaleXHp, healthDeclineDuration);

        if(scaleXHp < emergencyHealthMount && !isEmergency)
        {
            fadeTween = hpBarImage.DOFade(0.2f, fadeDuration + scaleXHp)
            .SetLoops(-1, LoopType.Yoyo)
            .SetEase(Ease.Linear);

            SetisEmergency(true);
        }
        else if(scaleXHp > emergencyHealthMount && isEmergency)
        {
            SetisEmergency(false);
            hpBarImage.color = originalHpBarColor;
            fadeTween.Pause();
        }
    }

   public void GetDamage(float Damage) // player damaged from monster
    {

        if(invinicible == false)
        {
            currentHealth -= ( Damage - (Damage * (defensePercent / 100)));
        }

        counterAttack.CounterAttack_Funtion();
    }


    IEnumerator emergencyHealth()
    {
        if (isEmergency)
        {
            hpBarImage.DOFade(originalHpBarColor.a - 0.5f, fadeDuration + scaleXHp).SetEase(Ease.OutQuad);
            yield return new WaitForSeconds(fadeDuration + scaleXHp);
            hpBarImage.DOFade(originalHpBarColor.a, fadeDuration + scaleXHp).SetEase(Ease.OutQuad);
            yield return new WaitForSeconds(fadeDuration + scaleXHp);
        }
        else
        {

        }
    }

    public IEnumerator FadeInOut()
    {
        float duration = 0.2f;

        hpBarImage.DOFade(0.2f, duration);
        yield return new WaitForSeconds(duration);
        hpBarImage.DOFade(1f, duration);
    }

    private void AllAnimationOff()
    {
        if (animator != null)
        {
            // Animator에 있는 모든 bool 변수를 false로 설정
            foreach (AnimatorControllerParameter parameter in animator.parameters)
            {
                if (parameter.type == AnimatorControllerParameterType.Bool)
                {
                    animator.SetBool(parameter.name, false);
                }
            }
        }
    }

    private void Shoot() // animator event
    {
        GameManager.bulletController.Shoot();
    }

    public void StatePlayer(PlayerStateEnum playerState)
    {
        switch (playerState)
        {
            case PlayerStateEnum.Dead:
                AllAnimationOff();
                isDead = true;
                animator.SetBool("isDead", true);
                GameManager.monsterControll.Getlist_MonsterParent().gameObject.SetActive(false);
                break;
            case PlayerStateEnum.Attack:
                if (GameManager.bulletController.GetTarget() == null)
                    return;

                var direction = transform.position.x - GameManager.bulletController.GetTarget().position.x;

                if(direction >= 0 ) // 몬스터가 플레이어 보다 왼쪽에 있는 경우
                    transform.localScale = new Vector3(-1* playerScale, playerScale, playerScale);
                else
                    transform.localScale = new Vector3(+1 * playerScale, playerScale, playerScale);


                AllAnimationOff();
                animator.SetBool("isAttack", true);
                animator.speed = GameManager.bulletController.animationSpeed;
                break;

            case PlayerStateEnum.Restart:
                AllAnimationOff();
                isDead = false;
                currentHealth = maxHealth;
                animator.SetBool("isDead", false);
                GameManager.monsterControll.Getlist_MonsterParent().gameObject.SetActive(true);
                break;
        }
    }

}
