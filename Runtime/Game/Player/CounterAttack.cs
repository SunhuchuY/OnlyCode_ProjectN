using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UniRx;
using System.Linq;

public class CounterAttack : MonoBehaviour
{
    private const float MULTIPLY_SCALE = 0.35f;

    private SpriteRenderer spriteRenderer;

    private float maxFade = 0.6f, durationFade = 1f;
    private float coolTime = 3f, curTime = 0;
        
    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        SubscribeAttackRange();
    }

    private void Update()
    {
        curTime += Time.deltaTime;
    }

    private void SubscribeAttackRange()
    {
        GameManager.Instance.playerScript.Stats["AttackRange"].OnChangesCurrentValueAsObservable.Subscribe(newValue => 
        {
            float newScale = newValue * MULTIPLY_SCALE;
            transform.localScale = new Vector2(newScale, newScale); 
        }).AddTo(gameObject);
    }

    public void PlayCounterAttack()
    {
        if (curTime < coolTime)
        {
            return;
        }

        curTime -= coolTime;
        StartCoroutine(alphaAnimation());

        GameManager.Instance.world.Actors
            .Where(actor => actor.ActorType == ActorType.Monster
                    && Vector2.Distance(actor.Go.transform.position, GameManager.Instance.playerScript.Go.transform.position) < GameManager.Instance.playerScript.Stats["AttackRange"].CurrentValue)
            .Select(actor => actor)
            .ToList()
            .ForEach(actor =>
            {
                Damage damage = new Damage() { Magnitude = -GameManager.Instance.playerScript.Stats["CounterAttack"].CurrentValue };
                actor.Stats["Hp"].ApplyModifier(damage);
            });
    }

    IEnumerator alphaAnimation()
    {
        spriteRenderer.color = new Color(1, 0, 0, 0);

        spriteRenderer.DOFade(maxFade, durationFade);
        yield return new WaitForSeconds(durationFade);

        spriteRenderer.color = new Color(1, 1, 1, 1);
    }
}
