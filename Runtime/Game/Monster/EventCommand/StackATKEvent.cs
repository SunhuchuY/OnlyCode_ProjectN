using System.Numerics;
using UnityEngine;

public class StackATKEvent : IEventCommand<BigInteger>
{
    private readonly int StackMaxCount;
    public Monster Owner { get; private set; }

    // ATK 추가데미지
    public BigInteger Add { get; private set; }
    private bool isOnRemoveEvent = false;
    private StatModifier modifier;

    private int m_stackCurrentCount = 0;

    private int stackCurrentCount
    {
        get
        {
            return m_stackCurrentCount;
        }

        set
        {
            m_stackCurrentCount = Mathf.Clamp(value, 0, StackMaxCount);
        }
    }


    public StackATKEvent(Monster owner, EventCommand command)
    {
        StackMaxCount = int.Parse(command.Parameters[1]);
        Owner = owner;
        Add = CalculateCommandReader.GetValue(command.Parameters[0], owner);
    }

    public void Event()
    {
        ++stackCurrentCount;

        // 스택이 다 쌓이면 ATK에 추가데미지를 넣습니다.
        if (stackCurrentCount == StackMaxCount)
        {
            modifier = new StatModifier() { Magnitude = (float)Add };
            Owner.Stats["Attack"].ApplyModifier(modifier);
            stackCurrentCount = 0;
            isOnRemoveEvent = true;
            GameManager.Instance.objectPoolManager.PlayParticle("Prefab/Particle/22", Owner.transform.position);
            return;
        }

        // 스택이 다 쌓이고, 다음턴에 ATK에 추가데미지를 제거합니다.
        if (isOnRemoveEvent)
        {
            Owner.Stats["Attack"].RemoveModifier(modifier);
        }
    }
}