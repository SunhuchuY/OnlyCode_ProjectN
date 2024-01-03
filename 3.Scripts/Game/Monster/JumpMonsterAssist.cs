using System.Collections.Generic;
using UnityEngine;

public class JumpMonsterAssist : MonoBehaviour
{
    public List<Transform> targets = new List<Transform>();
    Monster monster;

    private void Start()
    {
        monster = transform.parent.GetComponent<Monster>();
    }

    private void OnEnable()
    {
        targets.Clear();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() != null || collision.GetComponent<Friend>() != null)
        {
            targets.Add(collision.transform);
        }
    }

    public void Attack()
    {
        for (int i = 0; i < targets.Count; i++)
        {
            if(targets[i].GetComponent<Player>() != null)
            {
                targets[i].GetComponent<Player>().GetDamage((int)monster.attackMount);
            }
            else if (targets[i].GetComponent<Friend>() != null)
            {
                targets[i].GetComponent<Friend>().GetDamage(monster.attackMount);
            }
        }
    }
}
