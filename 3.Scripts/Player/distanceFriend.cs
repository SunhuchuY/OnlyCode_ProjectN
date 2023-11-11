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
                    // 발사
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

    private void BulletShoot() // 슛
    {
            GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);

        // 총알의 데미지를 설정
            bullet.GetComponent<Bullet>().towerAttackAmount = attackAmount;

            // 총알의 방향을 설정합니다.
            Vector2 bulletDirection = (transform.parent.GetComponent<Friend>().searchObj.transform.position - transform.position).normalized;

            // 각도 계산을 위해 방향 벡터를 각도로 변환합니다.
            float angle = Mathf.Atan2(bulletDirection.y, bulletDirection.x) * Mathf.Rad2Deg;

            // 총알의 회전을 설정합니다.
            bullet.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

            // Rigidbody2D 컴포넌트를 가져옵니다.
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();

            // 발사 방향으로 총알을 이동시킵니다.
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
