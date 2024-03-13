using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UpgradableAttributeAccessor : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI attributeValueText;
    [SerializeField] private TextMeshProUGUI costText;
    [SerializeField] private TextMeshProUGUI levelText;

    [SerializeField] private RectTransform requirementRT;

    public TextMeshProUGUI TitleText => titleText;
    public Image Icon => icon;
    public TextMeshProUGUI AttributeValueText => attributeValueText;
    public TextMeshProUGUI CostText => costText;
    public TextMeshProUGUI LevelText => levelText;
    public RectTransform RequirementRT => requirementRT;
}