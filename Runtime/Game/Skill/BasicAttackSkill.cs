using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicAttackSkill : AttackSkill
{

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Monster"))
            return;


        base.OnTriggerEnter2D(collision);
        //monster.GetDamage(attackAmount);
    }   

}
