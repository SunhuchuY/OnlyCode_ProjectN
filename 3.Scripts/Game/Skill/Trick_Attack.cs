using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trick_Attack : Trick
{
    [SerializeField]
    Skill skill;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Monster>() != null)
        {
            collision.GetComponent<Monster>() .GetDamage(skill.GetattackAmount());
        }
    }
}
