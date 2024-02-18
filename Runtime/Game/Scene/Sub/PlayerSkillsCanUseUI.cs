using System;
using TMPro;
using UnityEngine;

public class PlayerSkillsCanUseUI : MonoBehaviour
{
    [SerializeField] private TMP_Text nextCardLefttimeText;

    [SerializeField] private PlayerSkillsCanUseAccessor Accessor;
    [SerializeField] private SkillManager SkillManager;

    private float scaleWhenDragging = 0.3f;

    public void Awake()
    {
        SkillManager.OnSkillsCanUseNumsChanged += OnSkillsCanUseNumsChanged;

        foreach (var x in Accessor.SkillAccessors)
        {
            var _draggable = x.GetComponent<Draggable>();
            if (_draggable != null)
            {
                _draggable.OnDragStart += () =>
                {
                    _draggable.transform.localScale = _draggable.transform.localScale * scaleWhenDragging;
                };
                _draggable.OnDragEnd += () =>
                {
                    // 드래그한 ui의 크기를 원래 크기로 되돌립니다.
                    _draggable.transform.localScale = _draggable.transform.localScale / scaleWhenDragging;
                };
                _draggable.OnDropInZone += (_dropZone, _mouseWorldPosition) =>
                {
                    // 스킬을 사용합니다.
                    int _slotIndex = _draggable.transform.GetSiblingIndex();
                    SkillManager.UseSkill(_slotIndex, _mouseWorldPosition);
                };
                _draggable.OnNotDropInZone += () =>
                {
                    // 스킬을 사용할 수 없는 곳에 드래그했을 때 안내 메세지를 출력합니다.
                    GameManager.Instance.commonUI.ToastMessage("해당 공간에는 스킬을 사용할 수 없습니다.");
                };
            }
        }
    }

    private void Update()
    {
        UpdateNextSkillSlot(Accessor.NextSkillAccessor, SkillManager.NextSkillId);
    }

    private void OnSkillsCanUseNumsChanged()
    {
        for (int i = 0; i < SkillManager.SkillSlotIds.Count; ++i)
        {
            SkillAccessor _skillAccessor = Accessor.SkillAccessors[i];
            var _id = SkillManager.SkillSlotIds[i];
            UpdateSkillSlot(_skillAccessor, _id);
        }
    }

    private void UpdateSkillSlot(SkillAccessor _accessor, int? _skillId)
    {
        if (_skillId == null)
        {
            _accessor.Icon.sprite = Resources.Load<Sprite>("Sprite/UI/skill_pan");
            _accessor.CostText.text = $"";
        }
        else
        {
            var _card = GameManager.Instance.skillTreeManager.CardDataContainer.cards[_skillId.Value];
            _accessor.Icon.sprite =
                GameManager.Instance.skillTreeManager.CardDataContainer.cards[_skillId.Value].cardSprite;
            _accessor.CostText.text =
                GameManager.Instance.skillTreeManager.CardDataContainer.cards[_skillId.Value].cardCost.ToString();
            _accessor.LevelRoundImage.sprite =
                Resources.Load<Sprite>(
                    $"Sprite/UI/SkillLevelRound/skill_round_{_card.cardRate.ToString().ToLower()}");
            _accessor.SkillCostImage.sprite =
                Resources.Load<Sprite>(
                    $"Sprite/UI/SkillCostRound/skill_cost_{_card.cardRate.ToString().ToLower()}");
        }
    }

    private void UpdateNextSkillSlot(NextSkillAccessor _accessor, int? _skillId)
    {
        if (_skillId == null)
        {
            _accessor.Icon.sprite = Resources.Load<Sprite>("Sprite/UI/skill_pan");
            _accessor.CostText.text = $"";
        }
        else
        {
            var _card = GameManager.Instance.skillTreeManager.CardDataContainer.cards[_skillId.Value];
            _accessor.Icon.sprite =
                GameManager.Instance.skillTreeManager.CardDataContainer.cards[_skillId.Value].cardSprite;
            _accessor.CostText.text =
                GameManager.Instance.skillTreeManager.CardDataContainer.cards[_skillId.Value].cardCost.ToString();
            _accessor.LevelRoundImage.sprite =
                Resources.Load<Sprite>(
                    $"Sprite/UI/SkillLevelRound/skill_round_{_card.cardRate.ToString().ToLower()}");
            _accessor.SkillCostImage.sprite =
                Resources.Load<Sprite>(
                    $"Sprite/UI/SkillCostRound/skill_cost_{_card.cardRate.ToString().ToLower()}");
            _accessor.LeftTimeText.text = Convert.ToInt32(SkillManager.NextSkillCooldown).ToString();
        }
    }
}