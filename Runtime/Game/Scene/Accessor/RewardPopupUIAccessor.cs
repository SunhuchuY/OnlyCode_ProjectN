using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RewardPopupUIAccessor : MonoBehaviour
{
    [SerializeField] private Button skipButton;
    [SerializeField] private TMP_Text rewardText;
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private Image rewardImage;

    public Button SkipButton => skipButton;
    public TMP_Text RewardText => rewardText;
    public TMP_Text TitleText => titleText;
    public Image RewardImage => rewardImage;
}