using UnityEngine;

public class MeleeAbilityNone : BaseMeleeAbility
{   
    private void Awake()
    {
        IsParameterInitialized = true;
    }

    public override void Attack()
    {
        if (!IsInitialized())
        {
            Debug.LogError("초기화 되지 않았으므로, 근접 공격을 수행하지 않습니다.");
            return;
        }

        IGameActor targetActor = Owner.detector.GetCurrentTargetActor();
        targetActor.Stats["Hp"].ApplyModifier(
            new Damage { Magnitude = -Owner.Stats["Attack"].CurrentValue });
    }
}