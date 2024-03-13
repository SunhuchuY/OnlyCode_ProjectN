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
            Owner.Stats["Attack"].ApplyModifier(new StatModifier()
            {
                Magnitude = (float)CalculateCommandReader.GetValue(command.AddDamage, Owner)
            });
        }

        BaseMeleeAbility ability = null;

        switch (command.Ability)
        {
            case MeleeAbility.None:
            {
                MeleeAbilityNone temp = Owner.gameObject.AddComponent<MeleeAbilityNone>();
                ability = temp;
            }
            break;

            case MeleeAbility.WideArea:
            {
                MeleeAbilityWideArea temp = Owner.gameObject.AddComponent<MeleeAbilityWideArea>();
                float radius = float.Parse(command.Parameters[0]);
                temp.ParameterInitialize(radius);
                ability = temp;
            }
            break;
            
            case MeleeAbility.Delay:
            {
                MeleeAbilityDelay temp = Owner.gameObject.AddComponent<MeleeAbilityDelay>();
                ability = temp;
            }
            break;

            default:
                return;
        }

        ability.BasicInitialize(Owner);
        Owner.OnAttackEvent += ability.Attack;
    }
}