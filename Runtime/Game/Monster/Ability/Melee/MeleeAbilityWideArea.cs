using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MeleeAbilityWideArea : BaseMeleeAbility
{
    const float MULTIPLY_RADIUS = 0.4f;

    float Radius;
     
    public void ParameterInitialize(float radius)
    {
        Radius = radius;
        IsParameterInitialized = true;
    }

    public override void Attack()
    {
        if (!IsInitialized())
        {
            Debug.LogError("초기화 되지 않았으므로, 근접 공격을 수행하지 않습니다.");
            return;
        }

        GameManager.Instance.world.Actors
            .Where(actor => actor.ActorType != ActorType.Monster 
                    && Vector2.Distance(actor.Go.transform.position, Owner.Go.transform.position) < Radius)
            .Select(actor => actor)
            .ToList()
            .ForEach(actor =>
            {
                Damage damage = new Damage() { Magnitude = -Owner.Stats["Attack"].CurrentValue };
                actor.Stats["Hp"].ApplyModifier(damage);
                GameManager.Instance.objectPoolManager.PlayParticle("Prefab/Particle/19", actor.Go.transform.position, Radius * MULTIPLY_RADIUS);
            });
     }
}
