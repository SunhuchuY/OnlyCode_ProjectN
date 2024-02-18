using System.Numerics;
using UnityEngine;

public static class EventCommandApply
{
    public static void ApplyEvent(Monster owner, EventCommand command)
    {
        IEventCommand<BigInteger> eventcommand = null;

        switch (command.Type)
        {
            case EventCommandType.FirstATK:
                eventcommand = new FirstATKEvent(owner, command);
                break;

            case EventCommandType.StackATK:
                eventcommand = new StackATKEvent(owner, command);
                break;

            case EventCommandType.Resurrect:
                owner.gameObject.AddComponent<ResurrectEvent>();
                return;
        }

        if (eventcommand == null)
        {
            Debug.LogError("이벤트 대상이 등록되지 않았으므로, 처리를 하지 않고 리턴합니다.");
            return;
        }

        owner.PassiveEvent += eventcommand.Event;
    }
}