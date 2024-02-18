using System.Numerics;
using UnityEngine;

public class FirstATKEvent : IEventCommand<BigInteger>
{
    private bool isFirst = false;
    public Monster Owner { get; private set; }
    public BigInteger Add { get; private set; }

    public FirstATKEvent(Monster owner, EventCommand command)
    {
        Owner = owner;
        Add = CalculateCommandReader.GetValue(command.Parameters[0], owner);
    }
    
    public void Event()
    {
        if (isFirst)
        {
            // 첫 번째 공격은 한 상황이므로, 이벤트를 제거합니다.

            Owner.attributes.ATK.RemoveModifier(Add);
            Owner.PassiveEvent -= this.Event;
            return;
        }

        Owner.attributes.ATK.AddModifier(Add);
        isFirst = true;
    }
}