using System;
using System.Collections;
using UnityEngine;

public class MonsterUI : MonoBehaviour
{
    [SerializeField] UICanvas_HealthBar m_UICanvas_HealthBar;
    [SerializeField] private float m_ShowDuration = 2f;

    private Monster m_Owner;
    private Coroutine m_ShowHpBarCoroutine;

    private void ShowHpBar()
    {
        if (m_ShowHpBarCoroutine != null)
            StopCoroutine(m_ShowHpBarCoroutine);

        m_ShowHpBarCoroutine = StartCoroutine(ShowHpBar_Coroutine());
    }

    private IEnumerator ShowHpBar_Coroutine()
    {
        m_UICanvas_HealthBar.gameObject.SetActive(true);
        yield return new WaitForSeconds(m_ShowDuration);
        m_UICanvas_HealthBar.gameObject.SetActive(true);
    }

    private void Awake()
    {
        m_Owner = GetComponent<Monster>();

        m_UICanvas_HealthBar.Ratio = (float)m_Owner.Health.CurrentValue / m_Owner.Health.Cap;

        m_Owner.Health.OnCurrentValueChanged += (_newValue) =>
        {
            m_UICanvas_HealthBar.Ratio = (float)m_Owner.Health.CurrentValue / m_Owner.Health.Cap;
            ShowHpBar();
        };
        m_Owner.Health.OnCapChanged += (_newValue) =>
        {
            m_UICanvas_HealthBar.Ratio = (float)m_Owner.Health.CurrentValue / m_Owner.Health.Cap;
            ShowHpBar();
        };
    }

    private void OnEnable()
    {
        m_UICanvas_HealthBar.Ratio = (float)m_Owner.Health.CurrentValue / m_Owner.Health.Cap;
    }
}