using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using BackEnd;

public class UpgradeStatPanelsUI : BaseToggleUI<UpgradeStatPanelsUI.TapType>
{
    [SerializeField] private Player player;
    [SerializeField] private UpgradeStatPanelsAccessor accessor;

    private Dictionary<TapType, GameObject> GOTempletes;

    protected override Dictionary<TapType, (Button, RectTransform)> Dict { get; set; }

    struct AttributeCost
    {
    }

    public enum TapType
    {
        Skill,
        Attack,
        Defense,
        Health
    }

    public enum AttackTap
    {
        Attack,
        AttackSpeed,
        AttackRange,
        CriticalMultiplier,
        AttackMultiplier
    }

    public enum DefenseTap
    {
        Defense,
        Counter
    }

    public enum HealthTap
    {
        HealthRecovery,
        MaxHealth
    }

    private void Awake()
    {
        Dictionary<TapType, RectTransform> placeHolders = new Dictionary<TapType, RectTransform>();
        placeHolders.Add(TapType.Skill, accessor.UpgradableSkillsPlaceholder);
        placeHolders.Add(TapType.Attack, accessor.UpgradableAttackAttributesPlaceholder);
        placeHolders.Add(TapType.Defense, accessor.UpgradableDefenseAttributesPlaceholder);
        placeHolders.Add(TapType.Health, accessor.UpgradableHealthAttributesPlaceholder);

        GOTempletes = GetBackupTemplates(placeHolders);

        Dict = new Dictionary<TapType, (Button, RectTransform)>();
        Dict.Add(TapType.Skill, (accessor.UpgradeSkillsMenuButton, accessor.UpgradableSkillsPanel));
        Dict.Add(TapType.Attack, (accessor.UpgradeAttackMenuButton, accessor.UpgradableAttackAttributesPanel));
        Dict.Add(TapType.Defense, (accessor.UpgradeDefenseMenuButton, accessor.UpgradableDefenseAttributesPanel));
        Dict.Add(TapType.Health, (accessor.UpgradeHealthMenuButton, accessor.UpgradableHealthAttributesPanel));

        InitToggleUI();

        DisableAllButtons();
        DisableAllPanels();
    }

    private IEnumerator Start()
    {
        // 다른 스크립트보다 나중에 실행시키기 위해 2프레임 기다립니다.
        yield return null;

        CreateSkills();
        CreateAttackAttributes();
        CreateDefenseAttributes();
        CreateHealthAttributes();
    }

    private void OnEnable()
    {
        OnAction(TapType.Skill);
    }

    private void OnDisable()
    {
        ButtonRemoveListener();
    }

    private void CreateSkills()
    {
        if (Backend.IsLogin)
        {
            for (int i = 0; i < UseOwnSkillInventory.USEOWNSKILL_MAX_COUNT; i++)
            {
                if (i >= GameManager.Instance.userDataManager.userData.SkillsInUseList.Count)
                {
                    GameManager.Instance.userDataManager.userData.SkillsInUseList.Add(-1);
                }

                var _skillGo = Instantiate(GOTempletes[TapType.Skill], accessor.UpgradableSkillsPlaceholder);
                _skillGo.GetComponent<InventoryCard>().slot = i;
            }
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
            (
                attribute: player.Stats["AttackLevel"] as Attribute,
                maxLevel: PlayerTapParser.Instance.attack
                    .Where(x => x.Value.Attack.Value == 0 || x.Key == PlayerTapParser.Instance.attack.Count)
                    .Select(x => x.Key)
                    .FirstOrDefault() - 1,
                valueStat: player.Stats["Attack"],
                name: "공격력 증가",
                icon: Resources.Load<Sprite>("Sprite/Icon/Attribute/i_att01"),
                types: AttackTap.Attack,
                cost: PlayerTapParser.Instance.attack[player.Stats["AttackLevel"].CurrentValueInt].Attack.PayMagicStone
            ),
            (
                attribute: player.Stats["AttackSpeedLevel"] as Attribute,
                maxLevel: PlayerTapParser.Instance.attack
                    .Where(x => x.Value.AttackSpeed.Value == 0 || x.Key == PlayerTapParser.Instance.attack.Count)
                    .Select(x => x.Key)
                    .FirstOrDefault() - 1,
                valueStat: player.Stats["AttackSpeed"],
                name: "공격속도 증가",
                icon: Resources.Load<Sprite>("Sprite/Icon/Attribute/i_att02"),
                types: AttackTap.AttackSpeed,
                cost: PlayerTapParser.Instance.attack[player.Stats["AttackSpeedLevel"].CurrentValueInt].AttackSpeed
                    .PayMagicStone
            ),
            (
                attribute: player.Stats["AttackRangeLevel"] as Attribute,
                maxLevel: PlayerTapParser.Instance.attack
                    .Where(x => x.Value.AttackDistance.Value == 0 || x.Key == PlayerTapParser.Instance.attack.Count)
                    .Select(x => x.Key)
                    .FirstOrDefault() - 1,
                valueStat: player.Stats["AttackRange"],
                name: "공격범위 증가",
                icon: Resources.Load<Sprite>("Sprite/Icon/Attribute/i_att03"),
                types: AttackTap.AttackRange,
                cost: PlayerTapParser.Instance.attack[player.Stats["AttackRangeLevel"].CurrentValueInt].AttackDistance
                    .PayMagicStone
            ),
            (
                attribute: player.Stats["CriticalChanceLevel"] as Attribute,
                maxLevel: PlayerTapParser.Instance.attack
                    .Where(x => x.Value.AttackCritical.Value == 0 || x.Key == PlayerTapParser.Instance.attack.Count)
                    .Select(x => x.Key)
                    .FirstOrDefault() - 1,
                valueStat: player.Stats["CriticalChance"],
                name: "치명타 확률 증가",
                icon: Resources.Load<Sprite>("Sprite/Icon/Attribute/i_att04"),
                types: AttackTap.CriticalMultiplier,
                cost: PlayerTapParser.Instance.attack[player.Stats["CriticalChanceLevel"].CurrentValueInt]
                    .AttackCritical.PayMagicStone
            ),
            (
                attribute: player.Stats["CriticalMultiplierLevel"] as Attribute,
                maxLevel: PlayerTapParser.Instance.attack
                    .Where(x => x.Value.AttackCriticalDamage.Value == 0 ||
                                x.Key == PlayerTapParser.Instance.attack.Count)
                    .Select(x => x.Key)
                    .FirstOrDefault() - 1,
                valueStat: player.Stats["CriticalMultiplier"],
                name: "치명타 추가데미지\n배율 증가",
                icon: Resources.Load<Sprite>("Sprite/Icon/Attribute/i_att05"),
                types: AttackTap.AttackMultiplier,
                cost: PlayerTapParser.Instance.attack[player.Stats["CriticalMultiplierLevel"].CurrentValueInt]
                    .AttackCriticalDamage.PayMagicStone
            ),
        };

        foreach (var _pair in _attributes)
        {
            var _attributeGo = Instantiate(GOTempletes[TapType.Attack], accessor.UpgradableAttackAttributesPlaceholder);
            var _attributeAccessor = _attributeGo.GetComponent<UpgradableAttributeAccessor>();

            _attributeAccessor.UpgradeButton.onClick.AddListener(() =>
            {
                int _currentLevel = _pair.attribute.CurrentValueInt;
                int _nextLevel = _currentLevel + 1;

                if (GameManager.Instance.userDataManager.userData.MagicStone < _pair.cost)
                    return; // 구매조건 불충족

                if (_nextLevel > _pair.maxLevel)
                    return; // 레밸 MAX

                GameManager.Instance.userDataManager.ModifierCurrencyValue(CurrencyType.MagicStone,
                    -1 * _pair.cost); // 비용을 지불합니다.
                _pair.attribute.ApplyModifier(new StatModifier() { Magnitude = 1 }); // 이 속성의 레벨을 1 증가시킵니다.
                UpdateUI(_pair, _attributeAccessor); // ui를 갱신합니다.
            });

            UpdateUI(_pair, _attributeAccessor); // ui를 갱신합니다.

            GameManager.Instance.userDataManager.userData
                .ObserveEveryValueChanged(data => data.MagicStone)
                .Subscribe(newValue =>
                    _attributeAccessor.CostText.color = newValue < _pair.cost ? Color.red : Color.white);

            _attributeAccessor.CostText.color = GameManager.Instance.userDataManager.userData.MagicStone < _pair.cost
                ? Color.red
                : Color.white;
        }
    }

    private void CreateDefenseAttributes()
    {
        // 1. 방어력 증가
        // 2. 반격 데미지 증가

        var _attributes = new[]
        {
            (
                attribute: player.Stats["DefenseLevel"] as Attribute,
                maxLevel: PlayerTapParser.Instance.defense
                    .Where(x => x.Value.Defense.Value == 0 || x.Key == PlayerTapParser.Instance.defense.Count)
                    .Select(x => x.Key)
                    .FirstOrDefault() - 1,
                valueStat: player.Stats["Defense"],
                name: "방어력 증가",
                icon: Resources.Load<Sprite>("Sprite/Icon/Attribute/i_bang01"),
                types: DefenseTap.Defense,
                cost: PlayerTapParser.Instance.defense[player.Stats["DefenseLevel"].CurrentValueInt].Counter
                    .PayMagicStone
            ),
            (
                attribute: player.Stats["CounterAttackLevel"] as Attribute,
                maxLevel: PlayerTapParser.Instance.defense
                    .Where(x => x.Value.Counter.Value == 0 || x.Key == PlayerTapParser.Instance.defense.Count)
                    .Select(x => x.Key)
                    .FirstOrDefault() - 1,
                valueStat: player.Stats["CounterAttack"],
                name: "반격 데미지 증가",
                icon: Resources.Load<Sprite>("Sprite/Icon/Attribute/i_bang02"),
                types: DefenseTap.Counter,
                cost: PlayerTapParser.Instance.defense[player.Stats["CounterAttackLevel"].CurrentValueInt].Counter
                    .PayMagicStone
            ),
        };

        foreach (var _pair in _attributes)
        {
            var _attributeGo =
                Instantiate(GOTempletes[TapType.Defense], accessor.UpgradableDefenseAttributesPlaceholder);
            var _attributeAccessor = _attributeGo.GetComponent<UpgradableAttributeAccessor>();
            _attributeAccessor.UpgradeButton.onClick.AddListener(() =>
            {
                int _currentLevel = _pair.attribute.CurrentValueInt;
                int _nextLevel = _currentLevel + 1;

                if (GameManager.Instance.userDataManager.userData.MagicStone < _pair.cost)
                    return; // 구매조건 불충족

                if (_nextLevel > _pair.maxLevel)
                    return; // 레밸 MAX

                GameManager.Instance.userDataManager.ModifierCurrencyValue(CurrencyType.MagicStone,
                    -1 * _pair.cost); // 비용을 지불합니다.
                _pair.attribute.ApplyModifier(new StatModifier() { Magnitude = 1 }); // 이 속성의 레벨을 1 증가시킵니다.
                UpdateUI(_pair, _attributeAccessor); // ui를 갱신합니다.
            });

            UpdateUI(_pair, _attributeAccessor); // ui를 갱신합니다.

            GameManager.Instance.userDataManager.userData
                .ObserveEveryValueChanged(data => data.MagicStone)
                .Subscribe(newValue =>
                    _attributeAccessor.CostText.color = newValue < _pair.cost ? Color.red : Color.white);

            _attributeAccessor.CostText.color = GameManager.Instance.userDataManager.userData.MagicStone < _pair.cost
                ? Color.red
                : Color.white;
        }
    }

    private void CreateHealthAttributes()
    {
        // 1. 현재체력 회복
        // 2. 최대체력 증가

        var _attributes = new[]
        {
            (
                attribute: player.Stats["RecoveryLevel"] as Attribute,
                maxLevel: PlayerTapParser.Instance.health
                    .Where(x => x.Value.Recovery.Value == 0 || x.Key == PlayerTapParser.Instance.health.Count)
                    .Select(x => x.Key)
                    .FirstOrDefault() - 1,
                valueStat: player.Stats["Recovery"],
                name: "현재체력 회복",
                icon: Resources.Load<Sprite>("Sprite/Icon/Attribute/i_che01"),
                types: HealthTap.HealthRecovery,
                cost: PlayerTapParser.Instance.health[player.Stats["RecoveryLevel"].CurrentValueInt].Recovery
                    .PayMagicStone
            ),
            (
                attribute: player.Stats["MaxHealthLevel"] as Attribute,
                maxLevel: PlayerTapParser.Instance.health
                    .Where(x => x.Value.Recovery.Value == 0 || x.Key == PlayerTapParser.Instance.health.Count)
                    .Select(x => x.Key)
                    .FirstOrDefault() - 1,
                valueStat: player.Stats["MaxHealth"],
                name: "최대체력 증가",
                icon: Resources.Load<Sprite>("Sprite/Icon/Attribute/i_che02"),
                types: HealthTap.MaxHealth,
                cost: PlayerTapParser.Instance.health[player.Stats["MaxHealthLevel"].CurrentValueInt].MaxHP
                    .PayMagicStone
            ),
        };

        foreach (var _pair in _attributes)
        {
            var _attributeGo = Instantiate(GOTempletes[TapType.Health], accessor.UpgradableHealthAttributesPlaceholder);
            var _attributeAccessor = _attributeGo.GetComponent<UpgradableAttributeAccessor>();
            _attributeAccessor.UpgradeButton.onClick.AddListener(() =>
            {
                int _currentLevel = _pair.attribute.CurrentValueInt;
                int _nextLevel = _currentLevel + 1;

                if (GameManager.Instance.userDataManager.userData.MagicStone < _pair.cost)
                    return; // 구매조건 불충족

                if (_nextLevel > _pair.maxLevel)
                    return; // 레밸 MAX

                GameManager.Instance.userDataManager.ModifierCurrencyValue(CurrencyType.MagicStone,
                    -1 * _pair.cost); // 비용을 지불합니다.
                _pair.attribute.ApplyModifier(new StatModifier() { Magnitude = 1 }); // 이 속성의 레벨을 1 증가시킵니다.

                UpdateUI(_pair, _attributeAccessor); // ui를 갱신합니다.
            });

            UpdateUI(_pair, _attributeAccessor); // ui를 갱신합니다.

            GameManager.Instance.userDataManager.userData
                .ObserveEveryValueChanged(data => data.MagicStone)
                .Subscribe(newValue =>
                    _attributeAccessor.CostText.color = newValue < _pair.cost ? Color.red : Color.white);

            _attributeAccessor.CostText.color = GameManager.Instance.userDataManager.userData.MagicStone < _pair.cost
                ? Color.red
                : Color.white;
        }
    }

    private void UpdateUI(
        (Attribute levelAttribute, int maxLevel, Stat valueStat, string name, Sprite icon, Enum type, BigInteger cost)
            _pair,
        UpgradableAttributeAccessor _accessor)
    {
        // memo: BigInteger는 값에 의한 복사이므로, 메모리 사용량을 줄이기 위해, ref를 씁니다.

        _accessor.AttributeValueText.text =
            CurrencyHelper.ToCurrencyString(new BigInteger(_pair.valueStat.CurrentValueInt));
        _accessor.TitleText.text = _pair.name;
        _accessor.Icon.sprite = _pair.icon;

        if (_pair.levelAttribute.CurrentValueInt == _pair.maxLevel)
        {
            _accessor.LevelText.text = $"Lv.MAX";
            _accessor.CostText.text = string.Empty;
        }
        else
        {
            _accessor.LevelText.text = $"Lv.{_pair.levelAttribute.CurrentValueInt.ToString()}";
            _accessor.CostText.text = CurrencyHelper.ToCurrencyString(_pair.cost);
        }
    }
}