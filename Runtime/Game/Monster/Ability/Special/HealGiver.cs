using System.Linq;
using UnityEngine;


public class HealGiver : MonoBehaviour
{
    public void ApplyHeal(float radius, int healAmount)
    {
        Collider2D[] targets = Physics2D.OverlapCircleAll(transform.position, 100, 1 << LayerMask.NameToLayer("Actor"));

        GameManager.Instance.objectPoolManager.PlayParticle("Prefab/Particle/15", transform.position);

        foreach (var tar in targets)
        {
            if (tar.CompareTag("Monster"))
            {
                Monster mon = tar.GetComponent<Monster>();
                StatModifier modifier = new StatModifier();
                modifier.Magnitude = healAmount;
                mon.Stats["Hp"].ApplyModifier(modifier);
            }
        }
    }
}