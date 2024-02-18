using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knockback_OnlyOne : KnockbackTower
{
    [SerializeField]
    Skill skill;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.GetComponent<Monster>())
            return;

        var monster = collision.GetComponent<Monster>();
        //monster.GetDamage(skill.GetattackAmount());

        gameObject.SetActive(false);
    }
}
