using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarAccessor : MonoBehaviour
{
    [SerializeField] private Slider m_Slider;
    [SerializeField] private Image m_fillImage;

    public float Ratio
    {
        get => m_Slider.value;
        set => m_Slider.value = value;
    }

    public void SetFillImageColor(Color color) 
    {
        m_fillImage.color = color;
    }
}


