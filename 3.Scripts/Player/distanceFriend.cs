using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public enum TowerType
{
    Basic,
    Flamethrowe
}


public class distanceFriend : MonoBehaviour
{
    public int attackAmount = 30;

    float curTime = 0f;
    float bulletSpeed = 7f;
    const float coolTime = 3f;


    [SerializeField] GameObject bulletPrefab;
    [SerializeField] GameObject emptyChildObj;  

    [SerializeField] TowerType towerType = TowerType.Basic;

    private void OnEnable()
    {
        if(emptyChildObj != null)
            emptyChildObj.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Monster") && transform.parent.GetComponent<Friend>().searchObj == null)
        {
            transform.parent.GetComponent<Friend>().searchObj = collision.GetComponent<Monster>();   
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Monster") && 
            transform.parent.GetComponent<Friend>().searchObj == collision.GetComponent<Monster>())
        {
            transform.parent.GetComponent<Friend>().searchObj = null;
        }
    }

    private void Update()
    {
        switch (towerType)
        {
            case TowerType.Basic:
                curTime += Time.deltaTime;

                if (curTime > coolTime && transform.parent.GetComponent<Friend>().searchObj != null)
                {
                    // �߻�
                    BulletShoot();
                    curTime = 0;
                }
                break;
            case TowerType.Flamethrowe:
                if (transform.parent.GetComponent<Friend>().searchObj != null)
                    FlamethroweShoot();
                else
                    FlamethroweOff();
                break;
        }

    }

    private void BulletShoot() // ��
    {
            GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);

        // �Ѿ��� �������� ����
            bullet.GetComponent<Bullet>().towerAttackAmount = attackAmount;

            // �Ѿ��� ������ �����մϴ�.
            Vector2 bulletDirection = (transform.parent.GetComponent<Friend>().searchObj.transform.position - transform.position).normalized;

            // ���� ����� ���� ���� ���͸� ������ ��ȯ�մϴ�.
            float angle = Mathf.Atan2(bulletDirection.y, bulletDirection.x) * Mathf.Rad2Deg;

            // �Ѿ��� ȸ���� �����մϴ�.
            bullet.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

            // Rigidbody2D ������Ʈ�� �����ɴϴ�.
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();

            // �߻� �������� �Ѿ��� �̵���ŵ�ϴ�.
            rb.velocity = bulletDirection * bulletSpeed;
    }

    private void FlamethroweShoot()
    {
        emptyChildObj.SetActive(true);

        Vector2 FlameDirection = (transform.parent.GetComponent<Friend>().searchObj.transform.position - transform.position).normalized;

        float angle = Mathf.Atan2(FlameDirection.y, FlameDirection.x) * Mathf.Rad2Deg;

        emptyChildObj.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle - 90));
    }

    private void FlamethroweOff()
    {
        emptyChildObj.SetActive(false);
    }

}
