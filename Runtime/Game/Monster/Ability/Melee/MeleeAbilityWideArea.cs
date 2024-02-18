using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAbilityWideArea : BaseMeleeAbility
{
    float Radius;

    public void ParameterInitialize(float radius)
    {
        Radius = radius;
        IsParameterInitialized = true;
    }

    public override void Attack()
    {
        if (!IsInitialized())
        {
            Debug.LogError("초기화 되지 않았으므로, 근접 공격을 수행하지 않습니다.");
            return;
        }

        Collider2D[] targets = Physics2D.OverlapCircleAll(Owner.transform.position, Radius, LayerMask.NameToLayer("Default"));
        foreach (var tar in targets)
        {
            if (tar.tag == "Player")
            {
                GameManager.Instance.playerScript.ApplyDamage((float)Owner.attributes.ATK.Value);
            }
            else if (tar.tag == "Friend")
            {
                tar.GetComponent<Friend>().GetDamage((float)Owner.attributes.ATK.Value);
            }
        }
    }
}