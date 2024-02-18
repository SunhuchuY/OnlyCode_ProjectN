using System;
using UnityEngine;

public static class MeleeCommandApply
{
    public static void Apply(Monster Owner, MeleeCommands command)
    { 
        if (command == null)
        {
            Debug.LogError("커맨드가 NULL입니다.");
        }

        // 공격력 추가 연산을 처리합니다.
        if (!string.IsNullOrEmpty(command.AddDamage))
        {
            Owner.attributes.ATK.AddModifier(CalculateCommandReader.GetValue(command.AddDamage, Owner));
        }

        BaseMeleeAbility ability = null;

        if (command.Ability == MeleeAbility.None)
        {
            MeleeAbilityNone temp = Owner.gameObject.AddComponent<MeleeAbilityNone>();

            ability = temp;
        }
        else if (command.Ability == MeleeAbility.WideArea)
        {
            MeleeAbilityWideArea temp = Owner.gameObject.AddComponent<MeleeAbilityWideArea>();

            float radius = float.Parse(command.Parameters[0]);
            temp.ParameterInitialize(radius);

            ability = temp;
        }
        else if (command.Ability == MeleeAbility.Delay)
        {
            MeleeAbilityDelay temp = Owner.gameObject.AddComponent<MeleeAbilityDelay>();

            ability = temp;
        }
        else
        {
            Debug.LogError("근접 캐릭터 능력이 없습니다.");
            throw new Exception("not found melee ability");
        }

        ability.BasicInitialize(Owner);
        Owner.AttackEvent += ability.Attack;
    }
}