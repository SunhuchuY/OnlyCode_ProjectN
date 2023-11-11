using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


[System.Serializable]
public enum type
{
    player,
    tower
}


public class Bullet : MonoBehaviour
{
    public int towerAttackAmount;

    const string isSuffer = "isSuffer";

    [SerializeField] type bulletType = type.player;

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("Monster"))
        {
            Monster monster=  collision.GetComponent<Monster>();

            //monster.hpBar.SetActive(monster.isShowHpBar);

            switch (bulletType)
            {
                case type.player:
                    if (GameManager.RandomRange((int)GameManager.playerScript.GetCriticalPercent(), 101))
                    {
                        monster.GetDamage(GameManager.playerScript.sumCriticalAttack(), false, appearTextEnum.critical);
                    }
                    else
                    {
                        monster.GetDamage(GameManager.playerScript.sumAttack());
                    }
                    break;
                case type.tower:
                    monster.GetDamage(towerAttackAmount ,true);
                    break;
            }

            GameManager.particleManager.PlayParticle(monster.transform, transform, 0);

            Destroy(gameObject); // ÃÑ¾Ë»èÁ¦
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
