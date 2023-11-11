using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    Transform target;
    public Transform GetTarget() { return target; }
    public void SetTarget(Transform _target) { target = _target.transform; }

    public GameObject bulletPrefab; // �߻��� �Ѿ� ������
    private Transform firePoint; // �Ѿ� �߻� ��ġ

    private const float bulletSpeed = 5f; // �Ѿ� �̵� �ӵ�
    private const float _animationInitSpeed = 1;
    private const float InitcoolTime = 2.5f;
    public float animationSpeed
    {
        get { return _animationInitSpeed + (InitcoolTime - coolTime); } 
    }

    private float coolTime = InitcoolTime, coolTimeLevel = 1f , curTime; // �Ѿ� �߻�ð�����

    public void SetCoolTime(float input) { coolTime += input;}
    public float GetCoolTime() { return coolTime; }
    public List<Monster> monsters = new List<Monster>();







    private void Start()
    {
        firePoint = GameManager.player.transform;
        curTime = coolTime;
    }

    private void Update()
    {
        curTime += Time.deltaTime;
    }

    public void Shoot() // ��
    {
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);

            Vector3 targetPosition = target.GetComponent<PositionData>().AimPosition.position;

            // �Ѿ��� ������ �����մϴ�.
            Vector2 bulletDirection = (targetPosition - firePoint.position).normalized;

            // ���� ����� ���� ���� ���͸� ������ ��ȯ�մϴ�.
            float angle = Mathf.Atan2(bulletDirection.y, bulletDirection.x) * Mathf.Rad2Deg;

            // �Ѿ��� ȸ���� �����մϴ�.
            bullet.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

            // Rigidbody2D ������Ʈ�� �����ɴϴ�.
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();

            // �߻� �������� �Ѿ��� �̵���ŵ�ϴ�.
            rb.velocity = bulletDirection * bulletSpeed;

            curTime = 0f;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Monster") && collision != null)
        {
            monsters.Add(collision.GetComponent<Monster>());
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Monster") && (target == null || !target.gameObject.activeSelf))
        {
            // Ÿ���� ������
            target = collision.transform; 
        }
        else if (collision.CompareTag("Monster") && target != null)
        {
            // Ÿ���� ������
            if(curTime > coolTime && !GameManager.isGameStop)
                GameManager.playerScript.StatePlayer(PlayerStateEnum.Attack);
        }

    }

    public void RemoveMonster(GameObject toRemoveObject)
    {
        for (int i = 0; i < monsters.Count; i++)
        {
            if (toRemoveObject.GetComponent<Monster>() == monsters[i])
            {
                monsters.RemoveAt(i);
                break;
            }
        }
    }

    public Monster RandMonster()
    {
        if(monsters.Count != 0)
        {
            int rand = Random.Range(0, monsters.Count);
            return monsters[rand];
        }
        else
        {
            return null;
        }
    }

    public void SequenceShot(int shotNum, float intervalTime)
    {
        StartCoroutine(SequenceShot_Coroutine(shotNum, intervalTime));
    }

    IEnumerator SequenceShot_Coroutine(int _shotNum, float _intervalTime)
    {
        for (int i = 0; i < _shotNum; i++)
        {
            while (RandMonster() == null)
            {
                // RandMonster()�� null�� ���� ��ٸ�
                yield return null;
            }

            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);

            // �Ѿ��� ������ �����մϴ�.
            Vector2 bulletDirection = (RandMonster().transform.position - firePoint.position).normalized;

            // ���� ����� ���� ���� ���͸� ������ ��ȯ�մϴ�.
            float angle = Mathf.Atan2(bulletDirection.y, bulletDirection.x) * Mathf.Rad2Deg;

            // �Ѿ��� ȸ���� �����մϴ�.
            bullet.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

            // Rigidbody2D ������Ʈ�� �����ɴϴ�.
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();

            // �߻� �������� �Ѿ��� �̵���ŵ�ϴ�.
            rb.velocity = bulletDirection * bulletSpeed;


            yield return new WaitForSeconds(_intervalTime);
        }
    }
}
