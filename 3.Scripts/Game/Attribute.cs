using UnityEngine;

public class Attribute
{
    public int Cap
    {
        get => m_Cap;
        set
        {
            if (m_Cap == value)
                return;

            m_Cap = value;
            OnCapChanged?.Invoke(value);
        }
    }

    public int CurrentValue => m_CurrentValue;
    public event System.Action<int> OnCapChanged;
    public event System.Action<int> OnCurrentValueChanged;
    public event System.Action OnCurrentValueIsZero;

    private int m_Cap;
    private int m_CurrentValue;

    public void Initialize()
    {
        m_CurrentValue = m_Cap;
    }

    public void ApplyModifier(int _modifier)
    {
        int _newValue = m_CurrentValue;
        _newValue += _modifier;

        // 값의 범위는 [0 - cap] 입니다.
        _newValue = Mathf.Clamp(_newValue, 0, m_Cap);

        if (_newValue != m_CurrentValue)
        {
            m_CurrentValue = _newValue;
            OnCurrentValueChanged?.Invoke(_newValue);

            if (_newValue == 0)
                OnCurrentValueIsZero?.Invoke();
        }
    }
}