using System;
using System.Collections;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class InventoryCard : MonoBehaviour
{
    [SerializeField] private Button assignButton;
    [SerializeField] private RectTransform costTf;
    [SerializeField] public Image iconImage, roundImage, skillCostImage;
    [SerializeField] private TMP_Text rateText;

    private event Action<int> initializeSlotEvent;

    private InventoryCard()
    {
        initializeSlotEvent += SubscribeSlot;
        initializeSlotEvent += ButtonAddListener;
    }

    public void InitializeSlot(int slot)
    {
        initializeSlotEvent.Invoke(slot);
    }

    private void ButtonAddListener(int slot)
    {
        assignButton.onClick.AddListener(() =>
        {
            GameManager.Instance.upgradeOwnSkillsPopup.accessor.gameObject.SetActive(true);
            GameManager.Instance.upgradeOwnSkillsPopup.selectSlotID = slot;
        });
    }

    private void SubscribeSlot(int slot)
    {
        GameManager.Instance.userDataManager.userData.SkillsInUseList.ObserveEveryValueChanged(x => x[slot])
            .Subscribe(skillID =>
            {
                ActiveSkillData curSkill = DataTable.ActiveSkillDataTable.ContainsKey(skillID)
                        ? DataTable.ActiveSkillDataTable[skillID]
                        : null;

                Initialize(curSkill);
            });
    }

    private void Initialize(ActiveSkillData curSkill)
    {
        if (curSkill == null)
        {
            IsEmptyToActive(true);
          
            iconImage.sprite = Resources.Load<Sprite>("Sprite/UI/skill_plus");
        }
        else
        {
            IsEmptyToActive(false);

            iconImage.sprite = Resources.Load<Sprite>("Sprite/Icon/Skill" + "/" + curSkill.Name);
            roundImage.sprite = Resources.Load<Sprite>($"Sprite/UI/SkillLevelRound/skill_round_{curSkill.Grade.ToString().ToLower()}");
            skillCostImage.sprite = Resources.Load<Sprite>($"Sprite/UI/SkillCostRound/skill_cost_{curSkill.Grade.ToString().ToLower()}");
            rateText.text = $"{curSkill.Cost}";
        }
    }   

    private void IsEmptyToActive(bool isEmpty)
    {
        if (isEmpty)
        {
            roundImage.gameObject.SetActive(false); 
            costTf.gameObject.SetActive(false); 
        }
        else
        {
            roundImage.gameObject.SetActive(true);
            costTf.gameObject.SetActive(true);
        }
    }
}
