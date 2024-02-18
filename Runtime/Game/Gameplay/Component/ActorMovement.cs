using DG.Tweening;
using DG.Tweening.Core.Easing;
using UnityEngine;

public class ActorMovement : MonoBehaviour
{
    [HideInInspector] public Rigidbody2D RigidBody;

    private float knockBackDuration = .3f;
    private Vector3 knockBackForce;
    private bool isKnockBack;
    private float leftKnockBackTime;

    private void Awake()
    {
        RigidBody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (isKnockBack)
        {
            leftKnockBackTime -= Time.deltaTime;
            if (leftKnockBackTime < 0)
            {
                isKnockBack = false;
                leftKnockBackTime = 0;
            }
            else
            {
                float _elapsedDurationDelta = (knockBackDuration - leftKnockBackTime) / knockBackDuration;
                float _delta = Mathf.Lerp(1, 0, _elapsedDurationDelta);
                RigidBody.AddForce(knockBackForce * _delta);

                float _easeValue = EaseManager.ToEaseFunction(Ease.InExpo)
                    .Invoke(knockBackDuration - leftKnockBackTime, knockBackDuration, 0f, 0f);
            }
        }
    }

    public void AddKnockBack(Vector3 _force)
    {
        knockBackForce = _force;
        isKnockBack = true;
        leftKnockBackTime = knockBackDuration;
    }
}