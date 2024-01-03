using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopDailyItemAccessor : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI buttonText;
    [SerializeField] private Button button;

    [SerializeField] private RectTransform rewardBox;

    public TextMeshProUGUI TitleText => titleText;
    public TextMeshProUGUI ButtonText => buttonText;
    public Button Button => button;

    public RectTransform RewardBox => rewardBox;
}