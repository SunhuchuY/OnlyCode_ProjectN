using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class RangedCommandApply
{
    public static void Apply(Monster owner, RangedCommand command)
    {
        // 공격력 관련 연산을 처리합니다.
        if (!string.IsNullOrEmpty(command.AddDamage))
        {
            owner.attributes.ATK.AddModifier(CalculateCommandReader.GetValue(command.AddDamage, owner));
        }

        BaseRangedAbility ability = null;

        // 파라미터를 적용하고, 어빌리티 대상을 지정합니다.

        if (command.Ability == RangedAbility.None)
        {
            RangedAbilityNone temp = owner.gameObject.AddComponent<RangedAbilityNone>();
            temp.ParameterInitialize(command.PrefabSpeed);

            ability = temp;
        }
        else if (command.Ability == RangedAbility.ShotGun)
        {
            RangedAbilityShotGun temp = owner.gameObject.AddComponent<RangedAbilityShotGun>();

            int bulletCount = int.Parse(command.Parameters[0]);
            float addAngle = float.Parse(command.Parameters[1]);
            temp.ParameterInitialize(command.PrefabSpeed, bulletCount, 50f);

            ability = temp;
        }
        else if (command.Ability == RangedAbility.Penetrate)
        {
            RangedAbilityPenetrate temp = owner.gameObject.AddComponent<RangedAbilityPenetrate>();
            
            int bulletCount = int.Parse(command.Parameters[0]);
            float addAngle = float.Parse(command.Parameters[1]);
            temp.ParameterInitialize(command.PrefabSpeed, bulletCount, addAngle);

            ability = temp;
        }
        else
        {
            Debug.LogError("원거리 능력이 없습니다.");
            throw new Exception("not found ranged ability");
        }

        ability.BasicInitialize(owner, command.PrefabName);
        owner.AttackEvent += ability.Shoot;
    }
}
