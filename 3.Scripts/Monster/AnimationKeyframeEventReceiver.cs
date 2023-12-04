using UnityEngine;

public class AnimationKeyframeEventReceiver : MonoBehaviour
{
    public event System.Action OnAttackEvent;
    public event System.Action OnDeadEvent;

    private void OnAttack() => OnAttackEvent?.Invoke();
    
    private void OnDead() => OnDeadEvent?.Invoke();
}