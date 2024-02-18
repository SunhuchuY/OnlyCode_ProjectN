using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    private Transform target;

    public Transform GetTarget()
    {
        return target;
    }

    public void SetTarget(Transform _target)
    {
        target = _target;
    }

    public GameObject bulletPrefab; // 발사할 총알 프리팹
    private Transform firePoint; // 총알 발사 위치

    private const float bulletSpeed = 5f; // 총알 이동 속도


    private void Start()
    {
        firePoint = GameManager.Instance.player.transform;
    }

    public void Shoot() // 슛
    {
        GameObject bullet = BulletsObjectPool.Instance.GetGO("Bullet1");
        bullet.transform.position = GameManager.Instance.player.transform.position;

        // 총알의 방향을 설정합니다.
        Vector3 _targetPosition =
            target != null ? target.position : GameManager.Instance.playerScript.CachedTargetPosition;
        Vector2 bulletDirection = (_targetPosition - firePoint.position).normalized;

        // 각도 계산을 위해 방향 벡터를 각도로 변환합니다.
        float angle = Mathf.Atan2(bulletDirection.y, bulletDirection.x) * Mathf.Rad2Deg;

        // 총알의 회전을 설정합니다.
        bullet.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

        // Rigidbody2D 컴포넌트를 가져옵니다.
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();

        // 발사 방향으로 총알을 이동시킵니다.
        rb.velocity = bulletDirection * bulletSpeed;
    }

    public void SequenceShot(int shotNum, float intervalTime, int attackAmount)
    {
        StartCoroutine(SequenceShot_Coroutine(shotNum, intervalTime, attackAmount));
    }

    private IEnumerator SequenceShot_Coroutine(int _shotNum, float _intervalTime, int _attackAmount)
    {
        for (int i = 0; i < _shotNum; i++)
        {
            IGameActor _monsterActor = null;
            do
            {
                _monsterActor =
                    GameManager.Instance.world.Actors.Where(x => x.ActorType == ActorType.Monster)
                        .ToList().Random();
                yield return null;
            } while (_monsterActor == null);

            Monster _monster = _monsterActor.Go.GetComponent<Monster>();

            while (GameManager.Instance.world.Actors.Where(x => x.ActorType == ActorType.Monster).ToList().Random() ==
                   null)
            {
                // RandMonster()가 null일 동안 기다림
                yield return null;
            }

            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);

            bullet.GetComponent<Bullet>().exptionAttackAmount = _attackAmount;

            // 총알의 방향을 설정합니다.
            Vector2 bulletDirection = (_monster.transform.position - firePoint.position).normalized;

            // 각도 계산을 위해 방향 벡터를 각도로 변환합니다.
            float angle = Mathf.Atan2(bulletDirection.y, bulletDirection.x) * Mathf.Rad2Deg;

            // 총알의 회전을 설정합니다.
            bullet.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

            // Rigidbody2D 컴포넌트를 가져옵니다.
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();

            // 발사 방향으로 총알을 이동시킵니다.
            rb.velocity = bulletDirection * bulletSpeed;


            yield return new WaitForSeconds(_intervalTime);
        }
    }
}