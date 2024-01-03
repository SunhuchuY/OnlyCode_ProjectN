using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillButton : MonoBehaviour
{
    private int _pickSkillNum = -1;

    [HideInInspector]
    public int pickSkillNum
    {
        get { return _pickSkillNum; }
        set
        {
            _pickSkillNum = value;
            CardSetting();
        }
    }

    [SerializeField] public Image iconImage, roundImage;
    [SerializeField] TMP_Text rateText;

    private void CardSetting()
    {
        if (pickSkillNum < 0)
        {
            rateText.text = $"";
            iconImage.sprite = GameManager.Instance.skillTreeManager.empryIconSprite;
            return;
        }

        rateText.text = $"{GameManager.Instance.skillTreeManager.CardDataContainer.cards[pickSkillNum].cardCost}";
        roundImage.sprite = GameManager.Instance.skillTreeManager.roundSprite[(int)GameManager.Instance.skillTreeManager.CardDataContainer.cards[pickSkillNum].cardRate];
        iconImage.sprite = GameManager.Instance.skillTreeManager.CardDataContainer.cards[pickSkillNum].cardSprite;

    }
}