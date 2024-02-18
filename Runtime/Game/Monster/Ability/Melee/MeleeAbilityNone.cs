using UnityEngine;

public class MeleeAbilityNone : BaseMeleeAbility
{   
    private void Awake()
    {
        // 매개변수가 존재하지 않으므로 true로 바꿔둡니다.
        IsParameterInitialized = true;
    }

    public override void Attack()
    {

        if (!IsInitialized())
        {
            Debug.LogError("초기화 되지 않았으므로, 근접 공격을 수행하지 않습니다.");
            return;
        }

        Collider2D col = Owner.detector.GetCurrentTargetCollider();

        if (col.tag == "Player")
        {
            GameManager.Instance.playerScript.ApplyDamage((float)Owner.attributes.ATK.Value);
        }
        else if (col.tag == "Friend")
        {
            col.GetComponent<Friend>().GetDamage((float)Owner.attributes.ATK.Value);
        }
    }
}