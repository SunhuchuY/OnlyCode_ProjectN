using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bomb : MonoBehaviour
{
    List<Monster> monsters = new List<Monster>();

    [SerializeField] ParticleSystem particleSyste;


    private void OnEnable()
    {
        monsters.Clear();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<Monster>() != null)
        {
            monsters.Add(collision.GetComponent<Monster>());
            StartCoroutine(Explosion_Coroutine());
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<Monster>() != null)
        {
            monsters.Add(collision.GetComponent<Monster>());
        }
    }

    IEnumerator Explosion_Coroutine()
    {
        particleSyste.Play();

        for (int i = 0; i < monsters.Count; i++)
        {
            //monsters[i].GetDamage(transform.parent.GetComponent<Skill>().GetattackAmount());
        }

        yield return null;
        gameObject.SetActive(false);


    }
}
