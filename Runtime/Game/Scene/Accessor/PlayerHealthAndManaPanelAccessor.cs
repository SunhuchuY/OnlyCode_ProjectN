using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthAndManaPanelAccessor : MonoBehaviour
{
    public TextMeshProUGUI HealthText => healthText;
    public Slider HealthSlider => healthSlider;
    public Image HealthFillImage => healthFillImage;
    public TextMeshProUGUI ManaText => manaText;
    public Transform ManaSlots => manaSlots;

    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private Slider healthSlider;
    [SerializeField] private Image healthFillImage;
    [SerializeField] private TextMeshProUGUI manaText;
    [SerializeField] private Transform manaSlots;
}