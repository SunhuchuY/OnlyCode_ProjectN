using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Linq;

public class CounterAttack : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private List<Monster> monsters;

    private float maxFade = 0.6f, durationFade = 1f;
    private float coolTime = 3f, curTime = 0;

    private void Start()
    {
        monsters = new List<Monster>(); 
        spriteRenderer=GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        curTime += Time.deltaTime;
    }

    public void PlayCounterAttack()
    {
        if (curTime < coolTime)
        {
            return;
        }

        curTime -= coolTime;
        StartCoroutine(alphaAnimation());

        List<Monster> temp = new(monsters);
        temp
            .ForEach(m => 
            { 
                m.ApplyDamage((int)GameManager.Instance.playerScript.Stats["CounterAttack"].CurrentValue);
            });

        GameManager.Instance.objectPoolManager.PlayParticle("Prefab/Particle/2", transform.position);
    }

    public void CounterAttack_Funtion(float damage)
    {
        StartCoroutine(alphaAnimation());

        for (int i = 0; i < monsters.Count; i++)
        {
            //monsters[i].GetDamage(damage);
        }

        //GameManager.Instance.objectPoolManager.PlayParticle("Prefab/Particle/2", transform.position);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Monster"))
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
