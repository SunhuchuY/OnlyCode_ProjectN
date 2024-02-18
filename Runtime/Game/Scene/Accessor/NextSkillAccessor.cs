using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NextSkillAccessor : MonoBehaviour
{
    public Image LevelRoundImage => levelRoundImage;
    public Image SkillCostImage => skillCostImage;
    public Image Icon => icon;
    public TextMeshProUGUI CostText => costText;
    public TextMeshProUGUI LeftTimeText => leftTimeText;
    
    [SerializeField] private Image levelRoundImage;
    [SerializeField] private Image skillCostImage;
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI costText;
    [SerializeField] private TextMeshProUGUI leftTimeText;
}
