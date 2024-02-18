using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionSkill_Search : MonoBehaviour
{
    [SerializeField]
    ExplosionSkill skill;

    [SerializeField]
    float moveDuration = 1;

    bool isTarget = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isTarget)
            return;

        if (collision.CompareTag("Monster"))
        {
            isTarget = true;

            var ease = Ease.InQuad;

            skill.transform.DOScale(Vector3.one * 0.3f, moveDuration)
            .SetEase(ease);

            skill.transform.DOMove(collision.transform.position, moveDuration)
                .SetEase(ease)
                .OnComplete(() => { skill.Explosion(); });
        }
    }
}
