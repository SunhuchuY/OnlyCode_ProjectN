using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class CounterAttack : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    float originalAlpha;

    float maxFade = 0.6f, minFade = 0.2f, durationFade = 1f;
    float coolTime = 3f, curTime = 0;

    List<Monster> monsters = new List<Monster>();

    private void Start()
    {
        spriteRenderer=GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        curTime += Time.deltaTime;
    }

    public void CounterAttack_Funtion()
    {
        if(curTime > coolTime)
        {
            StartCoroutine(alphaAnimation());

            for (int i = 0; i < monsters.Count; i++)
            {
                monsters[i].GetDamage((int)GameManager.playerScript.sumCounter());
                monsters[i].CounterAttack_Suffer();
            }

            GameManager.particleManager.PlayParticle(transform, transform, 1, durationFade);

            curTime = 0;
        }

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
