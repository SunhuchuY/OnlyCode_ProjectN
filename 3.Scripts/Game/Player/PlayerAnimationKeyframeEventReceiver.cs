using UnityEngine;

public class PlayerAnimationKeyframeEventReceiver : MonoBehaviour
{
    public event System.Action OnAttackEvent;
    public event System.Action OnDeadEvent;

    private void OnAttack() => OnAttackEvent?.Invoke();

    private void OnDead() => OnDeadEvent?.Invoke();
}