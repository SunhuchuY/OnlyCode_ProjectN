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
        m_UICanvas_HealthBar.gameObject.SetActive(false);
    }

    private void Awake()
    {
        m_Owner = GetComponent<Monster>();
        
        m_UICanvas_HealthBar.Ratio = (float)m_Owner.attributes.HP.CurrentValue / (float)m_Owner.attributes.HP.Cap.Value;

        m_Owner.attributes.HP.OnCurrentValueChange += (_newValue) =>
        {
            m_UICanvas_HealthBar.Ratio = (float)m_Owner.attributes.HP.CurrentValue / m_Owner.attributes.HP.Cap.Value;
            ShowHpBar();
        };
        m_Owner.attributes.HP.OnCapChanged += (_newValue) =>
        {
            m_UICanvas_HealthBar.Ratio = (float)m_Owner.attributes.HP.CurrentValue / (float)m_Owner.attributes.HP.Cap.Value;
            ShowHpBar();
        };
    }

   

    private void OnEnable()
    {
        m_UICanvas_HealthBar.Ratio = (float)m_Owner.attributes.HP.CurrentValue / (float)m_Owner.attributes.HP.Cap.Value;
    }
}