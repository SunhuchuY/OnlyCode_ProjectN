using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class CounterAttack : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private List<Monster> monsters = new List<Monster>();

    private float maxFade = 0.6f, minFade = 0.2f, durationFade = 1f;
    private float coolTime = 3f, curTime = 0;


    private void Start()
    {
        spriteRenderer=GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        curTime += Time.deltaTime;

        if (curTime > coolTime)
        {
            curTime = 0;
            CounterAttack_Funtion();
        }
    }

    public void CounterAttack_Funtion()
    {
        StartCoroutine(alphaAnimation());

            for (int i = 0; i < monsters.Count; i++)
            {
                monsters[i].GetDamage(GameManager.Instance.playerScript.CounterAttack.CurrentValue);
            }

        GameManager.Instance.objectPoolManager.PlayParticle("Prefab/Particle/2", transform.position);
    }

    public void CounterAttack_Funtion(float damage)
    {
        StartCoroutine(alphaAnimation());

        for (int i = 0; i < monsters.Count; i++)
        {
            monsters[i].GetDamage(damage);
        }

        GameManager.Instance.objectPoolManager.PlayParticle("Prefab/Particle/2", transform.position);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
            if (collision.CompareTag("Monster") && collision != null)
            {
                monsters.Add(collision.GetComponent<Monster>());
            }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Monster") && monsters.Contains(collision.GetComponent<Monster>()))
        {
            monsters.Remove(collision.GetComponent<Monster>());
        }
    }

    IEnumerator alphaAnimation()
    {
        spriteRenderer.color = new Color(1, 0, 0, 0);

        spriteRenderer.DOFade(maxFade, durationFade);
        yield return new WaitForSeconds(durationFade);

        spriteRenderer.color = new Color(1, 1, 1, 0);
    }
}
