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
            IGameActor monster =  collision.GetComponent<Monster>();

            // TODO: 리팩토링이 필요합니다.
            Damage damage = new Damage() { Magnitude = -GameManager.Instance.playerScript.GetAttack() };
            monster.Stats["Hp"].ApplyModifier(damage);

            GameManager.Instance.objectPoolManager.PlayParticle("Prefab/Particle/17", monster.Go.transform.position);
            
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

