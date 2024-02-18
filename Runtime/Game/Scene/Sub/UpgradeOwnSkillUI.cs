using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UniRx;

public class UpgradeOwnSkillUI : MonoBehaviour
{
    public ActiveSkillData skill;

    [SerializeField] private Button selectButton;
    [SerializeField] private Image iconImage, roundImage, skillCostImage;
    [SerializeField] private TMP_Text rateText;

    private void Awake()
    {
        selectButton.onClick.AddListener(() =>
        {
            GameManager.Instance.upgradeOwnSkillsPopup.SelectUpgradeSkillUI(skill);
        });

        skill.ObserveEveryValueChanged(x => x)
            .Subscribe(x => 
            {
                if (skill != null)
                {
                    iconImage.sprite = Resources.Load<Sprite>("Sprite/Icon/Skill" + "/" + skill.Name);
                    roundImage.sprite = Resources.Load<Sprite>($"Sprite/UI/SkillLevelRound/skill_round_{skill.Grade.ToString().ToLower()}");
                    skillCostImage.sprite = Resources.Load<Sprite>($"Sprite/UI/SkillCostRound/skill_cost_{skill.Grade.ToString().ToLower()}");
                    rateText.text = skill.Cost.ToString();
                }
            });
    }
}