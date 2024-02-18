using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;

public class Stat
{
    public ReactiveProperty<float> Cap = new(-1);
    public float CurrentValue => m_CurrentValue;
    public int CurrentValueInt => m_CurrentValueInt;
    public IObservable<float> OnChangesCurrentValueAsObservable => m_OnChangesCurrentValueSubject;
    public IObservable<int> OnChangesCurrentValueIntAsObservable => m_OnChangesCurrentValueIntSubject;

    protected float m_CurrentValue = 0f;
    protected int m_CurrentValueInt = 0;
    protected Subject<float> m_OnChangesCurrentValueSubject = new();
    protected Subject<int> m_OnChangesCurrentValueIntSubject = new();
    protected float m_BaseValue;
    protected List<IStatModifier> m_Modifiers = new();

    public virtual Stat Initialize()
    {
        return this;
    }

    public virtual void ApplyModifier(IStatModifier _modifier)
    {
        // 덧셈 > 곱셈 순서로 계산합니다.
        // note: stat은 override 타입의 modifier를 적용받지 않습니다.
        m_Modifiers.Add(_modifier);
        m_Modifiers = m_Modifiers.OrderBy(x => x.OperationType).ToList();

        CalculateValue();
    }

    public virtual void RemoveModifier(IStatModifier _modifier)
    {
        m_Modifiers.Remove(_modifier);
        CalculateValue();
    }

    private void CalculateValue()
    {
        float _newValue = m_BaseValue;

        foreach (var _modifier in m_Modifiers)
        {
            if (_modifier.OperationType == ModifierOperationType.Add)
            {
                _newValue += _modifier.Magnitude;
            }
            else if (_modifier.OperationType == ModifierOperationType.Multiply)
            {
                _newValue *= _modifier.Magnitude;
            }
        }

        if (Cap.Value > 0)
        {
            // 값의 범위는 [0 - cap] 입니다.
            _newValue = Mathf.Clamp(_newValue, 0, Cap.Value);
        }

        if (Mathf.Approximately(_newValue, m_CurrentValue) == false)
        {
            m_CurrentValue = _newValue;
            m_CurrentValueInt = Mathf.RoundToInt(_newValue);
            m_OnChangesCurrentValueSubject.OnNext(m_CurrentValue);
            m_OnChangesCurrentValueIntSubject.OnNext(m_CurrentValueInt);
        }
    }
}

public class Attribute : Stat
{
    public event Action OnCurrentValueIsZero;
    public event Action<float> OnCurrentValueIsZeroOrLess;
    public event Action<float> OnCurrentValueChange;
    public event Action<float> OnCapChanged;

    public override Stat Initialize()
    {
        if (Cap.Value > 0)
        {
            m_CurrentValue = Cap.Value;
            m_CurrentValueInt = Mathf.RoundToInt(Cap.Value);
            m_OnChangesCurrentValueSubject.OnNext(m_CurrentValue);
            m_OnChangesCurrentValueIntSubject.OnNext(m_CurrentValueInt);
        }

        return this;
    }

    public override void ApplyModifier(IStatModifier _modifier)
    {
        float _newValue = m_CurrentValue;

        switch (_modifier.OperationType)
        {
            case ModifierOperationType.Add:
                _newValue += _modifier.Magnitude;
                break;
            case ModifierOperationType.Multiply:
                _newValue *= _modifier.Magnitude;
                break;
            case ModifierOperationType.Override:
                _newValue = _modifier.Magnitude;
                break;
        }

        if (OnCurrentValueIsZeroOrLess != null && Cap.Value > 0 && _newValue <= 0)
        {
            // ex: 300 - (-10)
            OnCurrentValueIsZeroOrLess.Invoke(Cap.Value - _newValue);
        }

        if (Cap.Value > 0)
        {
            // 값의 범위는 [0 - cap] 입니다.
            _newValue = Mathf.Clamp(_newValue, 0, Cap.Value);
        }

        if (Mathf.Approximately(_newValue, m_CurrentValue) == false)
        {
            m_CurrentValue = _newValue;
            m_CurrentValueInt = Mathf.RoundToInt(_newValue);
            m_OnChangesCurrentValueSubject.OnNext(m_CurrentValue);
            m_OnChangesCurrentValueIntSubject.OnNext(m_CurrentValueInt);

            OnCurrentValueChange?.Invoke(_newValue);
        }

        if (m_CurrentValue <= 0)
        {
            OnCurrentValueIsZero?.Invoke();
        }
    }
}