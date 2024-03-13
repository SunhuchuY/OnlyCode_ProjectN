using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEditor;
using UnityEngine;
using UnityEngine.U2D;

public class UpgradeOwnSkillsPopupUI : MonoBehaviour
{
    public UpgradeOwnSkillsPopupUIAccessor accessor;
    private UpgradeOwnSkillUI template;
    
    [SerializeField] private SpriteAtlas skillIconatlas;
    [SerializeField] private SpriteAtlas uiatlas;

    public int selectSlotID;

    private ActiveSkillData selectSkillData;
    private List<int> previousSkillIDList;
    

    private void Awake()
    {
        template = Resources.Load<UpgradeOwnSkillUI>("Prefab/UI/UpgradeOwnSkill");
    }   

    private IEnumerator Start()
    {
        yield return null;
        yield return null;
        yield return null;
        
        InitSkillTreeUI();
        ButtonAddListener();
        SubscribeSkillTree();
    }

    private void InitSkillTreeUI()
    {
        previousSkillIDList = new List<int>();

        foreach (int skillID in GameManager.Instance.userDataManager.userData.SkillDict.Keys)
        {
            previousSkillIDList.Add(skillID);
            InstantiateSkillTreeUI(DataTable.ActiveSkillDataTable[skillID]);
        }
    }

    /// <summary> 강화 팝업 내에서 스킬 UI를 하나 생성합니다. </summary>
    private void InstantiateSkillTreeUI(ActiveSkillData skill)
    {
        Instantiate(template, accessor.SkillScollViewParent).skill = skill;
    }

    /// <summary> 새로 들어온 스킬들을 구독합니다. </summary>
    private void SubscribeSkillTree()
    {
        GameManager.Instance.userDataManager.userData.SkillDict.ObserveEveryValueChanged(x => x.Count)
            .Subscribe(_ =>
            {
                foreach (var skillID in GameManager.Instance.userDataManager.userData.SkillDict.Keys)
                {
                    if (!previousSkillIDList.Contains(skillID))
                    {
                        previousSkillIDList.Add(skillID);
                        InstantiateSkillTreeUI(DataTable.ActiveSkillDataTable[skillID]);
                    }
                }

            }).AddTo(this); 
    }

    private void ButtonAddListener()
    {
        accessor.QuitButton.onClick.AddListener(() =>
        {
            accessor.gameObject.SetActive(false);
        });

        accessor.skillUpgradePopup.SkillUseButton.onClick.AddListener(() =>
        {
            if (selectSkillData == null || selectSlotID < 0 
                || selectSlotID >= GameManager.Instance.userDataManager.userData.SkillsInUseList.Count)
            {
                GameManager.Instance.commonUI.ToastMessage("스킬이 선택되지 않았습니다.");
                return;
            }

            if (GameManager.Instance.userDataManager.userData.SkillsInUseList.Contains(selectSkillData.Id))
            {
                GameManager.Instance.commonUI.ToastMessage("이미 등록 되어 있는 스킬입니다.");
                return;
            }
            
            GameManager.Instance.userDataManager.userData.SkillsInUseList[selectSlotID] = selectSkillData.Id;
            accessor.gameObject.SetActive(false);
            GameManager.Instance.commonUI.ToastMessage("스킬이 정상적으로 등록되셨습니다!");
        });

        accessor.skillUpgradePopup.LevelUpButton.onClick.AddListener(() =>
        {
            if (selectSkillData == null)
            {
                GameManager.Instance.commonUI.ToastMessage("스킬을 선택하지 않으셨습니다.");
                return;
            }

            SavedSkillData savedData = GameManager.Instance.userDataManager.userData.SkillDict[selectSkillData.Id];
            int needOfGold = PlayerTapParser.Instance.levelOfSkill[savedData.Level].NeedOfGold;
            int needOfSkillFragments = PlayerTapParser.Instance.levelOfSkill[savedData.Level].NeedOfSkillFragments;

            if (savedData.Count < needOfSkillFragments)
            {
                GameManager.Instance.commonUI.ToastMessage("스킬 개수가 부족합니다.");
                return;
            }

            if (GameManager.Instance.userDataManager.userData.Gold < needOfGold)
            {
                GameManager.Instance.commonUI.ToastMessage("골드량이 부족합니다.");
                return;
            }

            GameManager.Instance.userDataManager.ModifierCurrencyValue(CurrencyType.Gold, -needOfGold);
            savedData.Count -= needOfSkillFragments;
            savedData.Level++;

            SelectUpgradeSkillUI(selectSkillData); // 변경사항을 새롭게 업데이트합니다.
        });
    }

    public void SelectUpgradeSkillUI(ActiveSkillData activeSkillData)
    {
        selectSkillData = activeSkillData;
        SavedSkillData savedData = GameManager.Instance.userDataManager.userData.SkillDict[selectSkillData.Id];

        accessor.skillUpgradePopup.TitleSkillImage.sprite = skillIconatlas.GetSprite(activeSkillData.Name);
        accessor.skillUpgradePopup.TitleNameText.text = activeSkillData.Name;
        accessor.skillUpgradePopup.TitleLevelText.text = $"Lv.{savedData.Level}";

        bool isMaxLevel = savedData.Level == PlayerTapParser.Instance.MAX_LEVEL_SKILL;
        accessor.skillUpgradePopup.CurrentCostText.text = $"Cost: {activeSkillData.Cost}";
        accessor.skillUpgradePopup.CurrentExplanationText.text = $"{InsertDescriptionFormat(activeSkillData.Description, activeSkillData.LevelPer, savedData.Level)}";
        accessor.skillUpgradePopup.NextCostText.text = $"Cost: {activeSkillData.Cost}";
        if (isMaxLevel)
        {
            accessor.skillUpgradePopup.CurrentLevelText.text = $"Lv.MAX";

            accessor.skillUpgradePopup.NextLevelText.text = string.Empty;
            accessor.skillUpgradePopup.NextExplanationText.text = string.Empty;
        }
        else
        {
            accessor.skillUpgradePopup.CurrentLevelText.text = $"Lv.{savedData.Level}";

            accessor.skillUpgradePopup.NextLevelText.text = $"Lv.{savedData.Level + 1}";
            accessor.skillUpgradePopup.NextExplanationText.text = $"{InsertDescriptionFormat(activeSkillData.Description, activeSkillData.LevelPer, savedData.Level + 1)}";
        }
        
        bool isSkillCount = savedData.Count >= PlayerTapParser.Instance.levelOfSkill[savedData.Level].NeedOfSkillFragments;
        accessor.skillUpgradePopup.NeedSkillImage.sprite = skillIconatlas.GetSprite(activeSkillData.Name);
        accessor.skillUpgradePopup.NeedSkillText.text = isSkillCount
            ? $"{savedData.Count} / {CurrencyHelper.ToCurrencyString(PlayerTapParser.Instance.levelOfSkill[savedData.Level].NeedOfSkillFragments)}"
            : $"<color=red>{savedData.Count}</color> / {CurrencyHelper.ToCurrencyString(PlayerTapParser.Instance.levelOfSkill[savedData.Level].NeedOfSkillFragments)}";

        bool isGold = GameManager.Instance.userDataManager.userData.Gold >= PlayerTapParser.Instance.levelOfSkill[savedData.Level].NeedOfGold;
        accessor.skillUpgradePopup.NeedGoldImage.sprite = uiatlas.GetSprite("i_gold");
        accessor.skillUpgradePopup.NeedGoldText.text = isGold
            ? $"{CurrencyHelper.ToCurrencyString(PlayerTapParser.Instance.levelOfSkill[savedData.Level].NeedOfGold)}"
            : $"<color=red>{CurrencyHelper.ToCurrencyString(PlayerTapParser.Instance.levelOfSkill[savedData.Level].NeedOfGold)}</color>";

        bool isLevelUp = isSkillCount && isGold && !isMaxLevel;
        accessor.skillUpgradePopup.LevelUpButton.image.color = isLevelUp ? Color.white : Color.gray;
        accessor.skillUpgradePopup.LevelUpText.color = isLevelUp ? Color.white : Color.red;
    }

    private string InsertDescriptionFormat(string original, string levelPer, int level)
    {
        float per;

        if (levelPer.EndsWith('+'))
        {
            per = float.Parse(levelPer.Substring(0, levelPer.Length - 1)) + level;
        }
        else
        {
            per = float.Parse(levelPer) * level;
        }

        return System.Text.RegularExpressions.Regex.Replace(original, @"\{(.*?)\}", m =>
        {
            if (float.TryParse(m.Groups[1].Value, out float result))
            {
                result *= per;
                return $"{result}";
            }
            return m.Value;
        });
    }
}