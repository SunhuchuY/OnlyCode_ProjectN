using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using System.Threading.Tasks;
using UnityEngine.VFX;
using Random = UnityEngine.Random;  

public class Skill : MonoBehaviour
{

    [HideInInspector] public int skillNumber = 1, dotDamage = 5;
    [HideInInspector] public float attackAmount = 40f, debuffAmount, defenseAmount;
    [HideInInspector] private float slowSpeed = 0.5f;
    [HideInInspector] public float DestroyDelayTime = 4f;

    public float GetattackAmount()
    {
        return GameManager.Instance.uIManager.level * (attackAmount * GameManager.Instance.skillManager.skillLevel[skillNumber]);
    }


    public async Task SkillStartAsync(int _attackAmount, int skillNum, float destrpyDuration = 0)
    {
        attackAmount = _attackAmount;
        skillNumber = skillNum;

        await Task.Delay((int)(destrpyDuration * 1000));
        Destroy(gameObject);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision == null)
            return;

        if (collision.CompareTag("Monster"))
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
                    level = GameManager.Instance.skillTreeManager.CurCardStates[skillNumber].cardLevel;
                    debuff = GameManager.Instance.skillTreeManager.cardDatas[skillNumber].DebuffAmount(level);
                    collision_MonsterScript._MoveSpeedDown(debuff, DestroyDelayTime);
                    break;

                case 2:
                    collision_MonsterScript.GetDamage(GetattackAmount(), true);
                    gameObject.SetActive(false);
                    break;

                case 5:
                    collision_MonsterScript.GetDamage(GetattackAmount(), true);
                    break;

                case 6:
                    collision_MonsterScript.DotDamage((int)attackAmount, 7, 1);
                    break;

                case 7:
                    collision_MonsterScript.DotDamage((int)attackAmount, 7, 0.5f);
                    break;

                case 8:
                    gameObject.SetActive(false);
                    break;

            }
        }
    }
}


















