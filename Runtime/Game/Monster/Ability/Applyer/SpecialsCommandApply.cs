using UnityEngine;

public static class SpecialsCommandApply
{
    public static void Apply(Monster owner, SpecialsCommand command)
    {
        // 팀원에 효과
        if (command.Ability == SpecialAbility.Heal)
        {
            float radius = float.Parse(command.Parameters[0]);
            int heal = (int)CalculateCommandReader.GetValue(command.Parameters[1], owner);

            HealGiver giver = owner.gameObject.AddComponent<HealGiver>();

            owner.PassiveEvent += (() =>
            {
                giver.ApplyHeal(radius, heal);  
            });
        }
        // 상대에게 줌
        else if (command.Ability == SpecialAbility.Restraint)
        {
            float duration = float.Parse(command.Parameters[0]);

            owner.PassiveEvent += (() => 
            {
                Collider2D col = owner.detector.GetCurrentTargetCollider();
                
                if (col.gameObject.GetComponent<RestraintReceiver>() == null)
                {
                    RestraintReceiver receiver = col.gameObject.AddComponent<RestraintReceiver>();
                    receiver.ApplyRestraint(duration);
                }
            });
        }
        // 상대에게 줌
        else if (command.Ability == SpecialAbility.DotDamage)
        {
            float duration = float.Parse(command.Parameters[0]);
            float interval = float.Parse(command.Parameters[1]);
            int damage = int.Parse(command.Parameters[2]);

            owner.PassiveEvent += (() =>    
            {
                Collider2D col = owner.detector.GetCurrentTargetCollider();

                // 중첩이 안되게끔 도트데미지를 적용합니다.
                if (col.gameObject.GetComponent<DotDamageReceiver>() == null)
                {
                    DotDamageReceiver dotdamage = col.gameObject.AddComponent<DotDamageReceiver>();
                    dotdamage.ApplyDotDamage(duration, interval, damage);
                }
            });
        }
    }
}