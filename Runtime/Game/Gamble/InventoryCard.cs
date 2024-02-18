using System.Collections;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class InventoryCard : MonoBehaviour
{
    public int slot = 1; // 만약 정상적으로 작동하지 않으면 OutOfRange가 발생합니다.

    [SerializeField] private Button assignButton;
    [SerializeField] private RectTransform costTf;
    [SerializeField] public Image iconImage, roundImage, skillCostImage;
    [SerializeField] private TMP_Text rateText;

    private IEnumerator Start()
    {
        yield return null;
        yield return null;

        SubscribeSlot();
        ButtonAddListener();
    }

    private void ButtonAddListener()
    {
        assignButton.onClick.AddListener(() =>
        {
            GameManager.Instance.upgradeOwnSkillsPopup.accessor.gameObject.SetActive(true);
            GameManager.Instance.upgradeOwnSkillsPopup.selectSlotID = slot;
        });
    }

    private void SubscribeSlot()
    {
        if (BackEnd.Backend.IsLogin)
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
