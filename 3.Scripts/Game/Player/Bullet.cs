using System.Collections;
using UnityEngine;


public class Bullet : MonoBehaviour
{
    [HideInInspector] public int exptionAttackAmount = 0;
    [SerializeField] private type bulletType = type.player;

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("Monster"))
        {
            Monster monster=  collision.GetComponent<Monster>();

            switch (bulletType)
            {
                case type.player:
                    if (GameManager.RandomRange(GameManager.Instance.playerScript.CriticalMultiplier.CurrentValue, 101))
                    {
                        // fix: 이번 공격이 치명타인 경우, 치명타 공격력을 적용해야 합니다.
                        monster.GetDamage(GameManager.Instance.playerScript.Attack.CurrentValue, false, appearTextEnum.critical);
                    }
                    else
                    {
                        monster.GetDamage(GameManager.Instance.playerScript.Attack.CurrentValue);
                    }
                    break;
                case type.tower:
                    monster.GetDamage(GameManager.Instance.playerScript.Attack.CurrentValue, true);
                    break;
                case type.sequencs:
                    monster.GetDamage(exptionAttackAmount, true);
                    break;

            }

            GameManager.Instance.objectPoolManager.PlayParticle("Prefab/Particle/1", monster.transform.position);

            Destroy(gameObject); // 총알삭제
        }
    }

    private void OnEnable()
    {
        StartCoroutine(Bullet_Destroy());

        
    }

    IEnumerator Bullet_Destroy()
    {
        yield return new WaitForSeconds(5);

        if(gameObject != null)
            Destroy(gameObject);
    }
}
