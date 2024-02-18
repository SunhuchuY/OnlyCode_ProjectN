using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistanceFriend_Bullet : distanceFriend
{
    [SerializeField] GameObject bulletPrefab;

    void Update()
    {
        curTime += Time.deltaTime;

        if (curTime > coolTime && transform.parent.GetComponent<Friend>().searchObj != null)
        {
            BulletShoot();
            curTime = 0;
        }
    }

    void BulletShoot() // 슛
    {
        GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);

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
}
