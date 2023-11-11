using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class skill28Assist : MonoBehaviour
{

    int count = 0;


    [SerializeField] Skill skill;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Monster"))
        {
            collision.GetComponent<Monster>().GetDamage(skill.attackAmount, true);
        }
    }

    void CountUp()
    {
        count++;

        if(count == 4)
        {
            skill.gameObject.SetActive(false);
        }
    }
}
