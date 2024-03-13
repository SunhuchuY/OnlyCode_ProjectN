using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using BackEnd;
using Cysharp.Threading.Tasks.Triggers;
using UniRx.Triggers;
using UnityEngine.EventSystems;

public class UpgradeStatPanelsUI : BaseToggleUI<UpgradeStatPanelsUI.TapType>
{
    private const float CHANGE_SEQUENCE_UPGRADE_DURATION = 1.8f;
    private const float SEQUENCE_UPGRADE_DELAY = 0.1f;
    
    [SerializeField] private Player player;
    [SerializeField] private UpgradeStatPanelsAccessor accessor;

    private Dictionary<TapType, GameObject> GOTempletes;
    protected override Dictionary<TapType, (Button, RectTransform)> Dict { get; set; }
    private bool isPressed;

    public enum TapType
    {
        Skill,
        Attack,
        Defense,
        Health
    }

    public class UpgradableAttributeItem
    {
        public string Title;
        public Sprite Icon;
        public Stat TargetStat;
        public int MaxLevel;
        public Func<int> CurrentLevel;
        public Action<int> CurrentLevelSetter;
        public Func<BigInteger> CostMagicStone;
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
        yield return null;
        yield return null;

        CreateSkills();
        CreateAttributes();
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
        for (int i = 0; i < UserDataManager.USEOWNSKILL_MAX_COUNT; i++)
        {
            if (i >= GameManager.Instance.userDataManager.userData.SkillsInUseList.Count)
            {
                GameManager.Instance.userDataManager.userData.SkillsInUseList.Add(-1);
            }
            
            var _skillGo = Instantiate(GOTempletes[TapType.Skill], accessor.UpgradableSkillsPlaceholder);
            _skillGo.GetComponent<InventoryCard>().InitializeSlot(i);
        }
    }

    private void CreateAttributes()
    {
        var _attackItems = new List<UpgradableAttributeItem>
        {
            // 1. 공격력 증가
            // 2. 공격속도 증가
            // 3. 공격범위 증가
            // 4. 치명타 확률 증가
            // 5. 치명타 배율 증가
            new()
            {
                Title = "공격력 증가",
                Icon = Resources.Load<Sprite>("Sprite/Icon/Attribute/i_att01"),
                TargetStat = player.Stats["Attack"],
                MaxLevel = PlayerTapParser.Instance.attack
                    .Where(x => x.Value.Attack.PayMagicStone == 0)
                    .Select(x => x.Key)
                    .FirstOrDefault(),
                CurrentLevel = () => GameManager.Instance.userDataManager.userData.AttackLevel,
                CurrentLevelSetter = v => GameManager.Instance.userDataManager.userData.AttackLevel = v,
                CostMagicStone = () =>
                    PlayerTapParser.Instance
                    .attack[GameManager.Instance.userDataManager.userData.AttackLevel]
                    .Attack.PayMagicStone
            },
            new()
            {
                Title = "공격속도 증가",
                Icon = Resources.Load<Sprite>("Sprite/Icon/Attribute/i_att02"),
                TargetStat = player.Stats["AttackSpeed"],
                MaxLevel = PlayerTapParser.Instance.attack
                    .Where(x => x.Value.AttackSpeed.PayMagicStone == 0)
                    .Select(x => x.Key)
                    .FirstOrDefault(),
                CurrentLevel = () => GameManager.Instance.userDataManager.userData.AttackSpeedLevel,
                CurrentLevelSetter = v => GameManager.Instance.userDataManager.userData.AttackSpeedLevel = v,
                CostMagicStone = () => 
                    PlayerTapParser.Instance
                    .attack[GameManager.Instance.userDataManager.userData.AttackSpeedLevel]
                    .AttackSpeed.PayMagicStone
            },
            new()
            {
                Title = "공격범위 증가",
                Icon = Resources.Load<Sprite>("Sprite/Icon/Attribute/i_att03"),
                TargetStat = player.Stats["AttackRange"],
                MaxLevel = PlayerTapParser.Instance.attack
                    .Where(x => x.Value.AttackDistance.PayMagicStone == 0)
                    .Select(x => x.Key)
                    .FirstOrDefault(),
                CurrentLevel = () => GameManager.Instance.userDataManager.userData.AttackRangeLevel,
                CurrentLevelSetter = v => GameManager.Instance.userDataManager.userData.AttackRangeLevel = v,
                CostMagicStone = () => 
                    PlayerTapParser.Instance
                    .attack[GameManager.Instance.userDataManager.userData.AttackRangeLevel]
                    .AttackDistance.PayMagicStone
            },
            new()
            {
                Title = "치명타 확률 증가",
                Icon = Resources.Load<Sprite>("Sprite/Icon/Attribute/i_att04"),
                TargetStat = player.Stats["CriticalChance"],
                MaxLevel = PlayerTapParser.Instance.attack
                    .Where(x => x.Value.AttackCritical.PayMagicStone == 0)
                    .Select(x => x.Key)
                    .FirstOrDefault(),
                CurrentLevel = () => GameManager.Instance.userDataManager.userData.CriticalChanceLevel,
                CurrentLevelSetter = v => GameManager.Instance.userDataManager.userData.CriticalChanceLevel = v,
                CostMagicStone = () => 
                    PlayerTapParser.Instance
                    .attack[GameManager.Instance.userDataManager.userData.CriticalChanceLevel]
                    .AttackCritical.PayMagicStone
            },
            new()
            {
                Title = "치명타 추가데미지\n배율 증가",
                Icon = Resources.Load<Sprite>("Sprite/Icon/Attribute/i_att05"),
                TargetStat = player.Stats["CriticalMultiplier"],
                MaxLevel = PlayerTapParser.Instance.attack
                    .Where(x => x.Value.AttackCriticalDamage.PayMagicStone == 0)
                    .Select(x => x.Key)
                    .FirstOrDefault(),
                CurrentLevel = () => GameManager.Instance.userDataManager.userData.CriticalMultiplierLevel,
                CurrentLevelSetter = v => GameManager.Instance.userDataManager.userData.CriticalMultiplierLevel = v,
                CostMagicStone = () =>
                    PlayerTapParser.Instance
                    .attack[GameManager.Instance.userDataManager.userData.CriticalMultiplierLevel]
                    .AttackCriticalDamage.PayMagicStone
            },
        };

        var _defenseItems = new List<UpgradableAttributeItem>
        {
            // 1. 방어력 증가
            // 2. 반격 데미지 증가
            new()
            {
                Title = "방어력 증가",
                Icon = Resources.Load<Sprite>("Sprite/Icon/Attribute/i_bang01"),
                TargetStat = player.Stats["Defense"],
                MaxLevel = PlayerTapParser.Instance.defense
                    .Where(x => x.Value.Defense.PayMagicStone == 0)
                    .Select(x => x.Key)
                    .FirstOrDefault(),
                CurrentLevel = () => GameManager.Instance.userDataManager.userData.DefenseLevel,
                CurrentLevelSetter = v => GameManager.Instance.userDataManager.userData.DefenseLevel = v,
                CostMagicStone = () => 
                    PlayerTapParser.Instance
                    .defense[GameManager.Instance.userDataManager.userData.DefenseLevel].Counter
                    .PayMagicStone
            },
            new()
            {
                Title = "반격 데미지 증가",
                Icon = Resources.Load<Sprite>("Sprite/Icon/Attribute/i_bang02"),
                TargetStat = player.Stats["CounterAttack"],
                MaxLevel = PlayerTapParser.Instance.defense
                    .Where(x => x.Value.Counter.PayMagicStone == 0)
                    .Select(x => x.Key)
                    .FirstOrDefault(),
                CurrentLevel = () => GameManager.Instance.userDataManager.userData.CounterAttackLevel,
                CurrentLevelSetter = v => GameManager.Instance.userDataManager.userData.CounterAttackLevel = v,
                CostMagicStone = () => 
                    PlayerTapParser.Instance
                    .defense[GameManager.Instance.userDataManager.userData.CounterAttackLevel].Counter
                    .PayMagicStone
            },
        };

        var _healthItems = new List<UpgradableAttributeItem>
        {
            // 1. 현재체력 회복
            // 2. 최대체력 증가
            new()
            {
                Title = "현재체력 회복",
                Icon = Resources.Load<Sprite>("Sprite/Icon/Attribute/i_che01"),
                TargetStat = player.Stats["Recovery"],
                MaxLevel = PlayerTapParser.Instance.health
                    .Where(x => x.Value.Recovery.PayMagicStone == 0)
                    .Select(x => x.Key)
                    .FirstOrDefault(),
                CurrentLevel = () => GameManager.Instance.userDataManager.userData.RecoveryLevel,
                CurrentLevelSetter = v => GameManager.Instance.userDataManager.userData.RecoveryLevel = v,
                CostMagicStone = () => 
                    PlayerTapParser.Instance
                    .health[GameManager.Instance.userDataManager.userData.RecoveryLevel].Recovery
                    .PayMagicStone
            },
            new()
            {
                Title = "최대체력 증가",
                Icon = Resources.Load<Sprite>("Sprite/Icon/Attribute/i_che02"),
                TargetStat = player.Stats["MaxHp"],
                MaxLevel = PlayerTapParser.Instance.health
                    .Where(x => x.Value.MaxHP.PayMagicStone == 0)
                    .Select(x => x.Key)
                    .FirstOrDefault(),
                CurrentLevel = () => GameManager.Instance.userDataManager.userData.MaxHealthLevel,
                CurrentLevelSetter = v => GameManager.Instance.userDataManager.userData.MaxHealthLevel = v,
                CostMagicStone = () => 
                    PlayerTapParser.Instance
                    .health[GameManager.Instance.userDataManager.userData.MaxHealthLevel].MaxHP
                    .PayMagicStone
            }
        };

        CreateUpgradableAttributeItems(
            _attackItems, GOTempletes[TapType.Attack], accessor.UpgradableAttackAttributesPlaceholder);
        CreateUpgradableAttributeItems(
            _defenseItems, GOTempletes[TapType.Defense], accessor.UpgradableDefenseAttributesPlaceholder);
        CreateUpgradableAttributeItems(
            _healthItems, GOTempletes[TapType.Health], accessor.UpgradableHealthAttributesPlaceholder);
    }

    private void CreateUpgradableAttributeItems(
        List<UpgradableAttributeItem> _items,
        GameObject _templateGo,
        Transform _placeholder)
    {
        foreach (var _item in _items)
        {
            var _attributeGo = Instantiate(_templateGo, _placeholder);
            var _attributeAccessor = _attributeGo.GetComponent<UpgradableAttributeAccessor>();

            AddListenerEventTrigger(_item, _attributeAccessor);

            StartCoroutine(UpdateUI(_item, _attributeAccessor)); // ui를 갱신합니다.

            GameManager.Instance.userDataManager.userData
                .ObserveEveryValueChanged(data => data.MagicStone)
                .Subscribe(newValue =>
                    _attributeAccessor.CostText.color = newValue < _item.CostMagicStone()
                    ? Color.red 
                    : Color.white);

            _attributeAccessor.CostText.color =
                GameManager.Instance.userDataManager.userData.MagicStone < _item.CostMagicStone()
                    ? Color.red
                    : Color.white;
        }
    }

    private void AddListenerEventTrigger(UpgradableAttributeItem _item, UpgradableAttributeAccessor _attributeAccessor)
    {
        _attributeAccessor.gameObject.GetOrAddComponent<ObservablePointerDownTrigger>()
            .OnPointerDownAsObservable()
            .Subscribe(_ =>
            {
                isPressed = true;

                TryUpgrade(_item, _attributeAccessor);
                StartCoroutine(SequenceUpgrade(_item, _attributeAccessor));
            })
            .AddTo(gameObject);
        
        _attributeAccessor.gameObject.GetOrAddComponent<ObservablePointerUpTrigger>()
            .OnPointerUpAsObservable()
            .Subscribe(_ =>
            {
                isPressed = false;
            })
            .AddTo(gameObject);
    }

    private void TryUpgrade(UpgradableAttributeItem _item, UpgradableAttributeAccessor _attributeAccessor)
    {
        // 구매조건 불충족
        if (GameManager.Instance.userDataManager.userData.MagicStone < _item.CostMagicStone())
        {
            GameManager.Instance.commonUI.ToastMessage("마석이 부족합니다.");
            return;
        }

        // 레밸 MAX
        if (_item.CurrentLevel() >= _item.MaxLevel)
        {
            GameManager.Instance.commonUI.ToastMessage("최대 래벨 입니다.");
            return;
        }

        GameManager.Instance.userDataManager.ModifierCurrencyValue(CurrencyType.MagicStone,
            -1 * _item.CostMagicStone()); // 비용을 지불합니다.
        _item.CurrentLevelSetter.Invoke(_item.CurrentLevel() + 1); // 이 속성의 레벨을 1 증가시킵니다.
        StartCoroutine(UpdateUI(_item, _attributeAccessor)); // ui를 갱신합니다.
    }

    private IEnumerator UpdateUI(UpgradableAttributeItem _item, UpgradableAttributeAccessor _accessor)
    {
        // 레밸 업그레이드 -> 스탯이 증가 -> UI 업데이트를 순차적으로 진행시키기 위해서 1프레임을 대기합니다.
        yield return null;

        _accessor.AttributeValueText.text =
            CurrencyHelper.ToCurrencyString(new BigInteger(_item.TargetStat.CurrentValueInt));
        _accessor.TitleText.text = _item.Title;
        _accessor.Icon.sprite = _item.Icon;

        if (_item.CurrentLevel() >= _item.MaxLevel)
        {
            _accessor.LevelText.text = $"Lv.MAX";
            _accessor.CostText.text = string.Empty;
            _accessor.RequirementRT.gameObject.SetActive(false);
        }
        else
        {
            _accessor.LevelText.text = $"Lv.{_item.CurrentLevel()}";
            _accessor.CostText.text = CurrencyHelper.ToCurrencyString(_item.CostMagicStone());
        }
    }

    private IEnumerator SequenceUpgrade(UpgradableAttributeItem _item, UpgradableAttributeAccessor _accessor)
    {
        float pointerUpTime = 0;

        while (isPressed)
        {
            if (pointerUpTime > CHANGE_SEQUENCE_UPGRADE_DURATION)
            {
                TryUpgrade(_item, _accessor);
                yield return new WaitForSeconds(SEQUENCE_UPGRADE_DELAY);   
            }
            else
            {
                pointerUpTime += Time.deltaTime;
                yield return null;
            }
        }
    }
}