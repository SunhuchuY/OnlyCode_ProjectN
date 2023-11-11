using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class skill20Assist : MonoBehaviour
{
   List<Monster> monsters = new List<Monster>();
    bool isDeed = false;

    private void OnEnable()
    {
        monsters.Clear();
        isDeed = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    { 

        if (collision.CompareTag("Monster") && !monsters.Contains(collision.GetComponent<Monster>()))
        {
            monsters.Add(collision.GetComponent<Monster>());
            StartCoroutine(DelayTrigger());
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Monster") && monsters.Contains(collision.GetComponent<Monster>()))
        {
            monsters.Remove(collision.GetComponent<Monster>());
        }
    }

    IEnumerator DelayTrigger()
    {
        yield return new WaitForSeconds(1f);
        transform.parent.GetComponent<Friend>().currentHealth = 0;

        for (int i = 0; i < monsters.Count; i++)
        {
            monsters[i].DotDamage(transform.parent.GetComponent<Skill>().dotDamage, 7);
        }

        GameManager.particleManager.PlayParticle(transform, GameManager.player.transform, 15);
        isDeed = true;
    }


}
