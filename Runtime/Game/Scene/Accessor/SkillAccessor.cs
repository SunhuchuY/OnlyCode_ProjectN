using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillAccessor : MonoBehaviour
{
    public RectTransform NotSetRect => notSetRect;
    public RectTransform SetRect => setRect;
    public Button Button => button;
    public Image LevelRoundImage => levelRoundImage;
    public Image SkillCostImage => skillCostImage;
    public Image Icon => icon;
    public TextMeshProUGUI CostText => costText;

    [SerializeField] private RectTransform notSetRect;
    [SerializeField] private RectTransform setRect;
    [SerializeField] private Button button;
    [SerializeField] private Image levelRoundImage;
    [SerializeField] private Image skillCostImage;
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI costText;
}