using System;
using System.Collections;
using UnityEngine;

public class DotDamageReceiver : MonoBehaviour
{
    public void ApplyDotDamage(float duration, float interval, int damage)
    {
        Collider2D col = GetComponent<Collider2D>();
        Action Dot = null;

        if (col.CompareTag("Player"))
        {
            Dot = (() => 
            {
                GameManager.Instance.playerScript.ApplyDamage(damage);
            });
        }
        else if (col.CompareTag("friend"))
        {
            // TODO: Friend 대상들에게 도트데미지 효과를 입히세요.
        }

        if (Dot == null)
        {
            Debug.LogError("도트데미지를 입히는 함수가 레퍼런싱 되지 않았습니다.");

        }

        StartCoroutine(DotDamage(duration, interval, Dot));
    }

    IEnumerator DotDamage(float duration, float interval, Action Dot)
    {
        float timePassed = 0;

        while (timePassed < duration)
        {
            Dot();
            yield return new WaitForSeconds(interval);
            timePassed += interval;
        }

        Destroy(this);
    }
}
