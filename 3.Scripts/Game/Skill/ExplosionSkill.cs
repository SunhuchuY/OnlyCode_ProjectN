using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class ExplosionSkill : MonoBehaviour
{
    [SerializeField]
    Skill skill;

    public void Explosion()
    {

        Debug.Log("Explosion");
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 1);
        Debug.Log("Explosion1");

        foreach (var collider in colliders)
        {
            if (!collider.CompareTag("Monster"))
                return;

            var monster = collider.GetComponent<Monster>();
            monster.GetDamage(skill.GetattackAmount());
            Debug.Log("Ing");
        }

        GameManager.Instance.objectPoolManager.PlayParticle("Prefab/Particle/16", transform.position);
        Destroy(gameObject);
    }


}
