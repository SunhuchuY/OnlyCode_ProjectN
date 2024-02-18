using UnityEngine;

public class MonsterAnimationKeyframeEventReceiver : MonoBehaviour
{
    public event System.Action OnAttackEvent;
    public event System.Action OnDeadEvent;

    public void OnAttack() => OnAttackEvent?.Invoke();
    
    public void OnDead() => OnDeadEvent?.Invoke();
}