using System.Collections;
using UnityEngine;


public class Bullet : MonoBehaviour
{
    [HideInInspector] public int exptionAttackAmount = 0;
    [SerializeField] private type bulletType = type.player;

    bool k;

    void OnEnable()
    {
        k = false;

        StartCoroutine(DelayActiveFalse());
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (k)
            return;

        if (collision.CompareTag("Monster"))
        {
            k = true;
            Monster monster =  collision.GetComponent<Monster>();

            switch (bulletType)
            {
                case type.player:
                     monster.ApplyDamage(GameManager.Instance.playerScript.GetAttack());
                    break;
                case type.tower:
                    //monster.GetDamage(GameManager.Instance.playerScript.Attack.CurrentValue, true);
                    break;
                case type.sequencs:
                    //monster.GetDamage(exptionAttackAmount, true);
                    break;

            }

            GameManager.Instance.objectPoolManager.PlayParticle("Prefab/Particle/17", monster.transform.position);
            BulletsObjectPool.Instance.ReleaseGO("Bullet1", gameObject);
        }
    }

    IEnumerator DelayActiveFalse()
    {
        yield return new WaitForSeconds(4);

        if (gameObject.activeSelf)
        {
            BulletsObjectPool.Instance.ReleaseGO("Bullet1", gameObject);
        }
    }
}

