using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillAccessor : MonoBehaviour
{
    public Image LevelRoundImage => levelRoundImage;
    public Image Icon => icon;
    public TextMeshProUGUI CostText => costText;
    
    [SerializeField] private Image levelRoundImage;
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI costText;
}