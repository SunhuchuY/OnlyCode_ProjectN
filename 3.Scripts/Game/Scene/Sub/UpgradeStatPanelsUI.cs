using System;
using System.Collections;
using System.Linq;
using Unity.Linq;
using UnityEngine;

public class UpgradeStatPanelsUI : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private UpgradeStatPanelsAccessor accessor;

    private GameObject skillGoTemplate;
    private GameObject attackGoTemplate;
    private GameObject defenseGoTemplate;
    private GameObject healthGoTemplate;

    private void Awake()
    {
        BackupTemplates();
        Cleanup();

        DisableAllButtons();
        DisableAllPanels();
    }

    private IEnumerator Start()
    {
        // 다른 스크립트보다 나중에 실행시키기 위해 2프레임 기다립니다.
        yield return null;
        yield return null;

        CreateSkills();
        CreateAttackAttributes();
        CreateDefenseAttributes();
        CreateHealthAttributes();
    }

    private void OnEnable()
    {
        accessor.UpgradeSkillsMenuButton.onClick.AddListener(OnClickSkillsMenuButton);
        accessor.UpgradeAttackMenuButton.onClick.AddListener(OnClickAttackMenuButton);
        accessor.UpgradeDefenseMenuButton.onClick.AddListener(OnClickDefenseMenuButton);
        accessor.UpgradeHealthMenuButton.onClick.AddListener(OnClickHealthMenuButton);

        OnClickSkillsMenuButton();
    }

    private void OnDisable()
    {
        accessor.UpgradeSkillsMenuButton.onClick.RemoveListener(OnClickSkillsMenuButton);
        accessor.UpgradeAttackMenuButton.onClick.RemoveListener(OnClickAttackMenuButton);
        accessor.UpgradeDefenseMenuButton.onClick.RemoveListener(OnClickDefenseMenuButton);
        accessor.UpgradeHealthMenuButton.onClick.RemoveListener(OnClickHealthMenuButton);
    }

    private void OnClickSkillsMenuButton()
    {
        DisableAllPanels();
        DisableAllButtons();
        accessor.UpgradableSkillsPanel.gameObject.SetActive(true);
        ToggleEnable(accessor.UpgradeSkillsMenuButton.GetComponent<RectTransform>(), true);
    }

    private void OnClickAttackMenuButton()
    {
        DisableAllPanels();
        DisableAllButtons();
        accessor.UpgradableAttackAttributesPanel.gameObject.SetActive(true);
        ToggleEnable(accessor.UpgradeAttackMenuButton.GetComponent<RectTransform>(), true);
    }

    private void OnClickDefenseMenuButton()
    {
        DisableAllPanels();
        DisableAllButtons();
        accessor.UpgradableDefenseAttributesPanel.gameObject.SetActive(true);
        ToggleEnable(accessor.UpgradeDefenseMenuButton.GetComponent<RectTransform>(), true);
    }

    private void OnClickHealthMenuButton()
    {
        DisableAllPanels();
        DisableAllButtons();
        accessor.UpgradableHealthAttributesPanel.gameObject.SetActive(true);
        ToggleEnable(accessor.UpgradeHealthMenuButton.GetComponent<RectTransform>(), true);
    }

    private void BackupTemplates()
    {
        skillGoTemplate =
            Instantiate(accessor.UpgradableSkillsPlaceholder.gameObject.Children().First().gameObject);
        skillGoTemplate.hideFlags = HideFlags.HideInHierarchy;

        attackGoTemplate =
            Instantiate(accessor.UpgradableAttackAttributesPlaceholder.gameObject.Children().First().gameObject);
        attackGoTemplate.hideFlags = HideFlags.HideInHierarchy;

        defenseGoTemplate =
            Instantiate(accessor.UpgradableDefenseAttributesPlaceholder.gameObject.Children().First().gameObject);
        defenseGoTemplate.hideFlags = HideFlags.HideInHierarchy;

        healthGoTemplate =
            Instantiate(accessor.UpgradableHealthAttributesPlaceholder.gameObject.Children().First().gameObject);
        healthGoTemplate.hideFlags = HideFlags.HideInHierarchy;
    }

    private void Cleanup()
    {
        accessor.UpgradableSkillsPlaceholder.gameObject.Children().Destroy();
        accessor.UpgradableAttackAttributesPlaceholder.gameObject.Children().Destroy();
        accessor.UpgradableDefenseAttributesPlaceholder.gameObject.Children().Destroy();
        accessor.UpgradableHealthAttributesPlaceholder.gameObject.Children().Destroy();
    }

    private void DisableAllPanels()
    {
        accessor.UpgradableSkillsPanel.gameObject.SetActive(false);
        accessor.UpgradableAttackAttributesPanel.gameObject.SetActive(false);
        accessor.UpgradableDefenseAttributesPanel.gameObject.SetActive(false);
        accessor.UpgradableHealthAttributesPanel.gameObject.SetActive(false);
    }

    private void DisableAllButtons()
    {
        ToggleEnable(accessor.UpgradeSkillsMenuButton.GetComponent<RectTransform>(), false);
        ToggleEnable(accessor.UpgradeAttackMenuButton.GetComponent<RectTransform>(), false);
        ToggleEnable(accessor.UpgradeDefenseMenuButton.GetComponent<RectTransform>(), false);
        ToggleEnable(accessor.UpgradeHealthMenuButton.GetComponent<RectTransform>(), false);
    }

    private void ToggleEnable(RectTransform _button, bool _enable)
    {
        // 활성화 여부에 따라 Enabled, Disabled 오브젝트를 활성화/비활성화합니다.
        _button.gameObject.Descendants().First(x => x.name == "Enabled").SetActive(_enable);
        _button.gameObject.Descendants().First(x => x.name == "Disabled").SetActive(!_enable);
    }

    private void CreateSkills()
    {
        foreach (var _pair in player.SkillLevels)
        {
            var _skillData = GameManager.Instance.skillTreeManager.CardDataContainer.cards[_pair.Key];
            var _skillGo = Instantiate(skillGoTemplate, accessor.UpgradableSkillsPlaceholder);
            var _skillAccessor = _skillGo.GetComponent<SkillAccessor>();
            _skillAccessor.Icon.sprite = _skillData.cardSprite;
            _skillAccessor.CostText.text = _skillData.cardCost.ToString();
            _skillAccessor.LevelRoundImage.sprite = Resources.Load<Sprite>(
                $"Sprite/UI/SkillLevelRound/skill_round_{_skillData.cardRate.ToString().ToLower()}");
        }
    }

    private void CreateAttackAttributes()
    {
        // 1. 공격력 증가
        // 2. 공격속도 증가
        // 3. 공격범위 증가
        // 4. 치명타 확률 증가
        // 5. 치명타 배율 증가

        var _attributes = new[]
        {
            (attribute: player.AttackLevel,
                name: "공격력 증가",
                icon: Resources.Load<Sprite>("Sprite/Icon/Attribute/i_att01")),
            (attribute: player.AttackSpeedLevel,
                name: "공격속도 증가",
                icon: Resources.Load<Sprite>("Sprite/Icon/Attribute/i_att02")),
            (attribute: player.AttackRangeLevel,
                name: "공격범위 증가",
                icon: Resources.Load<Sprite>("Sprite/Icon/Attribute/i_att03")),
            (attribute: player.CriticalMultiplierLevel,
                name: "치명타 확률 증가",
                icon: Resources.Load<Sprite>("Sprite/Icon/Attribute/i_att04")),
            (attribute: player.AttackMultiplierLevel,
                name: "치명타 추가데미지 배율 증가",
                icon: Resources.Load<Sprite>("Sprite/Icon/Attribute/i_att05")),
        };

        foreach (var _pair in _attributes)
        {
            var _attributeGo = Instantiate(attackGoTemplate, accessor.UpgradableAttackAttributesPlaceholder);
            var _attributeAccessor = _attributeGo.GetComponent<UpgradableAttributeAccessor>();
            _attributeAccessor.UpgradeButton.onClick.AddListener(() =>
            {
                // 업그레이드 버튼을 눌러 비용을 지불하고 레벨을 1 증가시킵니다.

                int _requiredCost = 100; // temp: 비용을 임시적으로 100으로 설정합니다.
                if (GameManager.Instance.userDataManager.Magicstone < _requiredCost)
                    return; // 구매조건 불충족

                _pair.attribute.ApplyModifier(1); // 이 속성의 레벨을 1 증가시킵니다.
                UpdateAttribute(_pair, _attributeAccessor); // ui를 갱신합니다.
            });
            UpdateAttribute(_pair, _attributeAccessor);
        }
    }

    private void CreateDefenseAttributes()
    {
        // 1. 방어력 증가
        // 2. 방어력 배율 증가

        var _attributes = new[]
        {
            (attribute: player.DefenseLevel,
                name: "방어력 증가",
                icon: Resources.Load<Sprite>("Sprite/Icon/Attribute/i_bang01")),
            (attribute: player.DefenseMultiplierLevel,
                name: "방어력 배율 증가",
                icon: Resources.Load<Sprite>("Sprite/Icon/Attribute/i_bang02")),
        };

        foreach (var _pair in _attributes)
        {
            var _attributeGo = Instantiate(defenseGoTemplate, accessor.UpgradableDefenseAttributesPlaceholder);
            var _attributeAccessor = _attributeGo.GetComponent<UpgradableAttributeAccessor>();
            _attributeAccessor.UpgradeButton.onClick.AddListener(() =>
            {
                // 업그레이드 버튼을 눌러 비용을 지불하고 레벨을 1 증가시킵니다.

                int _requiredCost = 100; // temp: 비용을 임시적으로 100으로 설정합니다.
                if (GameManager.Instance.userDataManager.Magicstone < _requiredCost)
                    return; // 구매조건 불충족

                _pair.attribute.ApplyModifier(1); // 이 속성의 레벨을 1 증가시킵니다.
                UpdateAttribute(_pair, _attributeAccessor); // ui를 갱신합니다.
            });
            UpdateAttribute(_pair, _attributeAccessor);
        }
    }

    private void CreateHealthAttributes()
    {
        // 1. 현재체력 회복
        // 2. 최대체력 증가

        var _attributes = new[]
        {
            (attribute: player.Health,
                name: "현재체력 회복",
                icon: Resources.Load<Sprite>("Sprite/Icon/Attribute/i_che01")),
            (attribute: player.MaxHealthLevel,
                name: "최대체력 증가",
                icon: Resources.Load<Sprite>("Sprite/Icon/Attribute/i_che02")),
        };

        foreach (var _pair in _attributes)
        {
            var _attributeGo = Instantiate(healthGoTemplate, accessor.UpgradableHealthAttributesPlaceholder);
            var _attributeAccessor = _attributeGo.GetComponent<UpgradableAttributeAccessor>();
            _attributeAccessor.UpgradeButton.onClick.AddListener(() =>
            {
                // 업그레이드 버튼을 눌러 비용을 지불하고 레벨을 1 증가시킵니다.

                int _requiredCost = 100; // temp: 비용을 임시적으로 100으로 설정합니다.
                if (GameManager.Instance.userDataManager.Magicstone < _requiredCost)
                    return; // 구매조건 불충족

                _pair.attribute.ApplyModifier(1); // 이 속성의 레벨을 1 증가시킵니다.
                UpdateAttribute(_pair, _attributeAccessor); // ui를 갱신합니다.
            });
            UpdateAttribute(_pair, _attributeAccessor);
        }
    }

    private void UpdateAttribute(
        (Attribute attribute, string name, Sprite icon) _pair,
        UpgradableAttributeAccessor _accessor)
    {
        _accessor.TitleText.text = _pair.name;
        _accessor.Icon.sprite = _pair.icon;
        _accessor.AttributeValueText.text = _pair.attribute.CurrentValue.ToString();
        _accessor.CostText.text = "100"; // temp: 비용을 임시적으로 100으로 표시합니다.
    }
}