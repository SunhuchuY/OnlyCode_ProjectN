using System.Numerics;
using UnityEngine;

public class FirstATKEvent : IEventCommand<BigInteger>
{
    private bool isFirst = false;
    public Monster Owner { get; private set; }
    public BigInteger Add { get; private set; }
    private StatModifier modifier;

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

            Owner.Stats["Attack"].RemoveModifier(modifier);
            Owner.OnPassiveEvent -= this.Event;
            return;
        }

        modifier = new StatModifier() { Magnitude = (float)Add };
        Owner.Stats["Attack"].ApplyModifier(modifier);
        GameManager.Instance.objectPoolManager.PlayParticle("Prefab/Particle/22", Owner.detector.GetCurrentTargetActor().Go.transform.position);
        isFirst = true;
    }
}