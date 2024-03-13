using System;
using System.Collections;
using UnityEngine;

public class DotDamageReceiver : MonoBehaviour
{
    IGameActor gameActor; 

    public void ApplyDotDamage(float duration, float interval, int damage)
    {
        gameActor = GetComponent<IGameActor>();
        StartCoroutine(DotDamage(duration, interval, damage));
    }

    IEnumerator DotDamage(float duration, float interval, int damage)
    {
        float timePassed = 0;

        while (timePassed < duration)
        {
            Dot(damage);
            yield return new WaitForSeconds(interval);
            timePassed += interval;
        }

        Destroy(this);
    }

    private void Dot(int damage)
    {
        gameActor.Stats["Hp"].ApplyModifier(
            new Damage { Magnitude = -damage } );

        GameManager.Instance.objectPoolManager.PlayParticle("Prefab/Particle/21", gameActor.Go.transform.position);
    }
}
