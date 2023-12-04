using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
                    if (GameManager.RandomRange((int)GameManager.Instance.playerScript.GetCriticalPercent(), 101))
                    {
                        monster.GetDamage(GameManager.Instance.playerScript.sumCriticalAttack(), false, appearTextEnum.critical);
                    }
                    else
                    {
                        monster.GetDamage(GameManager.Instance.playerScript.sumAttack());
                    }
                    break;
                case type.tower:
                    monster.GetDamage(GameManager.Instance.playerScript.sumAttack(), true);
                    break;
                case type.sequencs:
                    monster.GetDamage(exptionAttackAmount, true);
                    break;

            }

            GameManager.Instance.particleManager.PlayParticle(monster.transform, transform, 0);

            Destroy(gameObject); // �Ѿ˻���
        }
    }

    private void OnEnable()
    {
        StartCoroutine(Bullet_Destroy());

        if(bulletType == type.sequencs)
        {
            transform.position = GameManager.Instance.player.GetComponent<PositionData>().firePosition.position;

            Vector3 targetPosition = GameManager.Instance.bulletController.GetTarget().position;

            // �Ѿ��� ������ �����մϴ�.
            Vector2 bulletDirection = (targetPosition - GameManager.Instance.player.transform.position).normalized;

            // ���� ����� ���� ���� ���͸� ������ ��ȯ�մϴ�.
            float angle = Mathf.Atan2(bulletDirection.y, bulletDirection.x) * Mathf.Rad2Deg;

            // �Ѿ��� ȸ���� �����մϴ�.
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

            // Rigidbody2D ������Ʈ�� �����ɴϴ�.
            var rb = GetComponent<Rigidbody2D>();

            // �߻� �������� �Ѿ��� �̵���ŵ�ϴ�.
            rb.velocity = bulletDirection * 5;
        }
    }

    IEnumerator Bullet_Destroy()
    {
        yield return new WaitForSeconds(5);

        if(gameObject != null)
            Destroy(gameObject);
    }
}
