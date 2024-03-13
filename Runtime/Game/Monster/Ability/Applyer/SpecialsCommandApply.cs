using UnityEngine;

public static class SpecialsCommandApply
{
    public static void Apply(Monster owner, SpecialsCommand command)
    {
        switch (command.Ability)
        {
            // 팀원에 효과
            case SpecialAbility.Heal:
                { 
                    float radius = float.Parse(command.Parameters[0]);
                    int heal = (int)CalculateCommandReader.GetValue(command.Parameters[1], owner);

                    HealGiver giver = owner.gameObject.AddComponent<HealGiver>();

                    owner.OnPassiveEvent += (() =>
                    {
                        giver.ApplyHeal(radius, heal);
                    });
                }
                break;

            // 상대에게 줌
            case SpecialAbility.Restraint:
                {
                    float duration = float.Parse(command.Parameters[0]);

                    owner.OnPassiveEvent += (() =>
                    {
                        IGameActor targetActor = owner.detector.GetCurrentTargetActor();

                        if (targetActor.Go.GetComponent<RestraintReceiver>() == null)
                        {
                            RestraintReceiver restraintReceiver = targetActor.Go.AddComponent<RestraintReceiver>();
                            restraintReceiver.ApplyRestraint(duration);
                        }
                    });
                }
                break;

            // 상대에게 줌
            case SpecialAbility.DotDamage:
                {
                    float duration = float.Parse(command.Parameters[0]);
                    float interval = float.Parse(command.Parameters[1]);
                    int damage = int.Parse(command.Parameters[2]);

                    owner.OnPassiveEvent += (() =>
                    {
                        IGameActor targetActor = owner.detector.GetCurrentTargetActor();

                        // 중첩이 안되게끔 도트데미지를 적용합니다.
                        if (targetActor.Go.GetComponent<DotDamageReceiver>() == null)
                        {
                            DotDamageReceiver dotdamageReceiver = targetActor.Go.AddComponent<DotDamageReceiver>();
                            dotdamageReceiver.ApplyDotDamage(duration, interval, damage);
                        }
                    });
                }
                break;
        }
    }
}