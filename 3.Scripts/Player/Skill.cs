using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.VFX;

public class Skill : MonoBehaviour
{
    bool isAttack = true; // �Ϲ������� ������ ��������...�̼Ӱ��Ұ��� ������ �Ȱ��ϰ� �Ҽ��ִ� bool


    private ParticleSystem particleSystem;

    private Animator animator;

    [HideInInspector] public int skillNumber = 1, dotDamage = 5;
    [HideInInspector] public float attackAmount = 40f, debuffAmount;
    [HideInInspector] private float slowSpeed = 0.5f;
    [HideInInspector] public float DestroyDelayTime = 4f;

    public int callParticle_Index = -1;


    public float GetattackAmount()
    {
        return GameManager.uIManager.level * (attackAmount * GameManager.skillManager.skillLevel[skillNumber]);
    }

    private void Awake()
    {
        if (animator != null)
            animator.GetComponent<Animator>();

        for (int i = 0; i < transform.childCount; i++)
        {
            if(transform.GetChild(i).GetComponent<ParticleSystem>() != null)
            {
                particleSystem = transform.GetChild(i).GetComponent<ParticleSystem>();
                break;
            }
        }
    }

    public void SkillStart(int _attackAmount,int skillNum ,float destrpyDuration = 0)
    {
        attackAmount = _attackAmount;
        destrpyDuration = DestroyDelayTime;
        skillNumber = skillNum;
        StartCoroutine(DestroyDelay(destrpyDuration));
    }

    IEnumerator skill15_Coroutine(float delay , int N)
    { // skill 21번도 
        for (int i = 0; i < N; i++)
        {
            SpriteRenderer spriteRenderer = GameManager.bulletController.GetComponent<SpriteRenderer>();

            if (spriteRenderer != null)
            {
                // SpriteRenderer의 bounds를 이용하여 Sprite의 크기 가져오기
                Bounds spriteBounds = spriteRenderer.bounds;

                // 랜덤한 위치 계산
                Vector3 randomPosition = new Vector3(
                    Random.Range(spriteBounds.min.x, spriteBounds.max.x),
                    Random.Range(spriteBounds.min.y, spriteBounds.max.y),
                    0f);

                transform.GetChild(i).position = randomPosition;
                transform.GetChild(i).gameObject.SetActive(true);
            }
            yield return new WaitForSeconds(delay);
        }
    }

    IEnumerator particleOnEnable()
    {
        if(particleSystem != null)
        {
            particleSystem.Play();
            yield return new WaitForSeconds(DestroyDelayTime);
        }
    }

    private void Update()
    {
        Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position);

        // ȭ�� ������ ������ ������Ʈ�� ����
        if (screenPos.x < 0 || screenPos.x > Screen.width || screenPos.y < 0 || screenPos.y > Screen.height)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Monster") && collision != null)
        {
            Monster collision_MonsterScript = collision.GetComponent<Monster>();
                collision_MonsterScript.GetDamage(GetattackAmount(), true); // �⺻ ���ݵ�����

            int level;
            float debuff;

                switch (skillNumber)
                {
                case 0:
                    collision_MonsterScript.GetDamage(GetattackAmount(), true); // �⺻ ���ݵ�����
                    break;

                    case 1:
                    level = GameManager.skillTreeManager.CurCardStates[skillNumber].cardLevel;
                    debuff = GameManager.skillTreeManager.cardDatas[skillNumber].DebuffAmount(level);
                        collision_MonsterScript._MoveSpeedDown(debuff, DestroyDelayTime);
                        break;
                case 2:
                    collision_MonsterScript.GetDamage(GetattackAmount(), true);
                    gameObject.SetActive(false);
                    break;
                }

        }
    }

    IEnumerator DestroyDelay(float delay)
    {

        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }

    void DestroySkill() // anim event 
    {
        Destroy(gameObject);
    }

}
