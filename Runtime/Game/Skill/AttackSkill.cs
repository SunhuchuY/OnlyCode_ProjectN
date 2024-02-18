using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackSkill : MonoBehaviour
{
    [SerializeField]
    protected Skill skill;

    protected float attackAmount;
    protected Monster monster;


    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        attackAmount = skill.GetattackAmount();
        monster = collision.GetComponent<Monster>();
    }
}
