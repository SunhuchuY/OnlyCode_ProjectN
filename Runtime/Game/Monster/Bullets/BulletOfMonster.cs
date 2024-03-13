using System;
using System.Collections;
using UnityEngine;

public class BulletOfMonster : MonoBehaviour
{
    private int atk;
    public Rigidbody2D rb { get; private set; }
    public Collider2D col { get; private set; }

    public event Action OnAttackEvent;

    private void Awake()
    {
        // 컴포넌트를 미리 캐싱합니다.
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
    }

    private void OnEnable()
    {
        StartCoroutine(DelayActiveFalse());
        col.enabled = false;
    }
    
    public void Initialize(int atk)
    {
        this.atk = atk;
        col.enabled = true;
        OnAttackEvent = null;
    }

    public void Initialize(int atk, string particlePath)
    {
        Initialize(atk);
        OnAttackEvent += () => GameManager.Instance.objectPoolManager.PlayParticle(particlePath, transform.position);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!col.enabled)
        {
            return;
        }

        if (other.gameObject.layer == LayerMask.NameToLayer("Actor")) 
        {
            if (other.TryGetComponent<IGameActor>(out IGameActor targetActor) 
                && targetActor.ActorType != ActorType.Monster) 
            {
                Damage damage = new Damage() { Magnitude = -atk };
                targetActor.Stats["Hp"].ApplyModifier(damage);

                Dispose();
            }
        }
    }

    private void Dispose()
    {
        OnAttackEvent?.Invoke();
        col.enabled = false;
        BulletsObjectPool.Instance.ReleaseGO(transform.parent.name, gameObject);
    }

    public void DelayColliderEnabled(float delay)
    {
        // 새로운 DelayEnabled 코루틴을 시작합니다.
        StartCoroutine(DelayEnabled(delay));
    }

    private IEnumerator DelayEnabled(float delay)
    {
        col.enabled = false;
        yield return new WaitForSeconds(delay);
        col.enabled = true;
    }

    private IEnumerator DelayActiveFalse()
    {
        yield return new WaitForSeconds(4);
        
        if (gameObject.activeSelf)
        {
            BulletsObjectPool.Instance.ReleaseGO(transform.parent.name, gameObject);
        }
    }
}