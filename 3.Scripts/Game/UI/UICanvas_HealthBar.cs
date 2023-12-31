using UnityEngine;
using UnityEngine.UI;

public class UICanvas_HealthBar : MonoBehaviour
{
    [SerializeField] private Slider m_Slider;

    public float Ratio
    {
        get => m_Slider.value;
        set => m_Slider.value = value;
    }
}