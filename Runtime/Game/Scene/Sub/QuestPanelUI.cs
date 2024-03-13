using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.U2D;
using UniRx;
using System;
using BackEnd;

public class QuestPanelUI : BaseToggleUI<QuestPanelUI.QuestPanelType>
{
    [SerializeField] private SpriteAtlas uiAtlas;
    public QuestPanelAccessor accessor;
    protected override Dictionary<QuestPanelType, (Button, RectTransform)> Dict { get; set; }
    private Dictionary<QuestPanelType, RectTransform> placeHolders;
    private GameObject goTemplate;

    public enum QuestPanelType
    {
        Daily = 0,
        Week = 1,
        Story = 2
    }

    private void Start()
    {
        if (!Backend.IsLogin)
        {
#if UNITY_EDITOR
            Debug.LogWarning("로그인하지 않았으므로, 퀘스트 초기화가 진행되지 않습니다.");
#endif
            return;
        }

        accessor.ExitButton.onClick.AddListener(() => accessor.gameObject.SetActive(false));
        InitPlaceHolders();
        InitDict();
        InitToggleUI();
        goTemplate = Resources.Load<GameObject>("Prefab/UI/QuestUI");
        DisableAllButtons();
        DisableAllPanels();
        OnAction(QuestPanelUI.QuestPanelType.Daily);
        InitializeQuests();

        SubscribeStoryQuestToBigwave(GameManager.Instance.questData.questData.EpisodeCount.Value);
    }

    private void InitPlaceHolders()
    {
        placeHolders = new Dictionary<QuestPanelType, RectTransform>()
        {
            { QuestPanelType.Daily, accessor.DailyPlaceholder },
            { QuestPanelType.Week, accessor.WeekPlaceholder },
            { QuestPanelType.Story, accessor.StoryPlaceholder }
        };
    }
    private void InitDict()
    {
        Dict = new Dictionary<QuestPanelType, (Button, RectTransform)>()
        {
            { QuestPanelType.Daily, (accessor.DailyButton, accessor.DailyPanel) },
            { QuestPanelType.Week, (accessor.WeekButton, accessor.WeekPanel) },
            { QuestPanelType.Story, (accessor.StoryButton, accessor.StoryPanel) },
        };
    }

    private void InitializeQuests()
    {
        foreach (QuestPanelType type in Enum.GetValues(typeof(QuestPanelType)))
        {
            CreateQuestsForType(type);
        }
    }

    private void CreateQuestsForType(QuestPanelType type)
    {
        foreach (var quest in QuestParser.Instance.Quests[(int)type])
        {
            GameObject go = Instantiate(goTemplate, placeHolders[type]);
            QuestUIAccessor questAccessor = go.GetComponent<QuestUIAccessor>();
            ChangeState(questAccessor, quest, type);
            ButtonAddListener(questAccessor, quest, type);
            InitQuestUI(questAccessor, quest);

            if (type != QuestPanelType.Story && quest.condition.type != Quest.Condition.Type.Join)
            {
                SubscribeCountQuest(questAccessor, quest, type);
            }
            
            if (type == QuestPanelType.Story)
            {
                questAccessor.ConditionSlider.value = (float)GameManager.Instance.userDataManager.userData.BigWave / (float)(quest.condition.value + 1);
                questAccessor.QuestCondition.CostText.text = $"Wave <color=red>{GameManager.Instance.userDataManager.userData.Wave}</color>/{quest.condition.value}";
            }
        }
    }

    private void ChangeState(QuestUIAccessor accessor, Quest quest, QuestPanelType type)
    {
        // 스토리 퀘스트 상태 변경 로직
        if (type == QuestPanelType.Story)
        {
            HandleStoryQuestState(accessor, quest);
        }
        else
        {
            // 일반 퀘스트 상태 변경 로직 (Join, Kill, Gacha)
            HandleGeneralQuestState(accessor, quest, type);
        }
    }

    private void HandleStoryQuestState(QuestUIAccessor accessor, Quest quest)
    {
        int episodeCount = GameManager.Instance.questData.questData.EpisodeCount.Value;

        if (quest.condition.value <= episodeCount)
        {
            accessor.ChangeState(QuestUIAccessor.State.Complete);
        }
        // 조건문 실행 횟수가 1개보다 작거나 같습니다.
        else if (quest.condition.value == episodeCount + 1 &&
                 GameManager.Instance.userDataManager.userData.BigWave - episodeCount > 1)
        {
            accessor.ChangeState(QuestUIAccessor.State.Acquire);
        }
        else
        {
            accessor.ChangeState(QuestUIAccessor.State.Condition);
        }
    }

    private void HandleGeneralQuestState(QuestUIAccessor accessor, Quest quest, QuestPanelType type)
    {
        if (quest.condition.type == Quest.Condition.Type.Join)
        {
            accessor.ChangeState(QuestUIAccessor.State.Complete);
            return;
        }

        bool isQuestUsed = GameManager.Instance.questData.questData.IsUse[quest.ColumnStr];
        ReactiveProperty<int> count = GetCountByQuestType(quest.condition.type, type);

        if (isQuestUsed)
        {
            accessor.ChangeState(QuestUIAccessor.State.Complete);
        }
        else if (count.Value >= quest.condition.value)
        {
            accessor.ChangeState(QuestUIAccessor.State.Acquire);
        }
        else
        {
            accessor.ChangeState(QuestUIAccessor.State.Condition);
        }
    }

    private void ButtonAddListener(QuestUIAccessor accessor, Quest quest, QuestPanelType type)
    {
        // Daily와 Week 퀘스트에 대한 리스너 설정
        if (type == QuestPanelType.Daily || type == QuestPanelType.Week)
        {
            AddGeneralQuestListener(accessor, quest);
        }
        // Story 퀘스트에 대한 리스너 설정
        else if (type == QuestPanelType.Story)
        {
            AddStoryQuestListener(accessor, quest);
        }
    }

    private void AddGeneralQuestListener(QuestUIAccessor accessor, Quest quest)
    {
        accessor.QuestAcquire.Button.onClick.AddListener(() =>
        {
            accessor.ChangeState(QuestUIAccessor.State.Complete);
            GiveReward(quest.reward);
            UpdateQuestCompletionInBackend(quest);
        });
    }

    private void AddStoryQuestListener(QuestUIAccessor accessor, Quest quest)
    {
        accessor.QuestAcquire.Button.onClick.AddListener(() =>
        {
            GiveReward(quest.reward);
            GameManager.Instance.questData.questData.EpisodeCount.Value++;
            LoadStoryScene(quest.condition.value); // 씬이 변경되면서 서버에 동기화됩니다.
        });

        accessor.QquestComplete.Button.onClick.AddListener(() =>
        {
            LoadStoryScene(quest.condition.value);
        });
    }

    private void UpdateQuestCompletionInBackend(Quest quest)
    {
        Param param = new Param() { { quest.ColumnStr, true } };
        Backend.GameData.Update(QuestData.TABLE_NAME, new Where(), param);
    }

    private void LoadStoryScene(int storyId)
    {
        GameManager.Instance.storyManager.Initialize(storyId);
        SceneLoader.LoadSceneAsync(SceneLoader.SceneType.Story);
    }

    private void SubscribeCountQuest(QuestUIAccessor accessor, Quest quest, QuestPanelType type)
    {
        ReactiveProperty<int> count = GetCountByQuestType(quest.condition.type, type);

        count.Subscribe(newValue =>
        {
            if (accessor.state == QuestUIAccessor.State.Condition && newValue >= quest.condition.value)
            {
                accessor.ChangeState(QuestUIAccessor.State.Acquire);
            }
            else
            {
                accessor.ConditionSlider.value = (float)newValue / (float)quest.condition.value;
                accessor.QuestCondition.CostText.text = $"<color=red>{CurrencyHelper.ToCurrencyString(newValue)}</color>/{CurrencyHelper.ToCurrencyString(quest.condition.value)}"; 
            }
        });
    }

    private void SubscribeStoryQuestToBigwave(int curEpisodeCount)
    {
        int maxEpisodeCount = QuestParser.Instance.Quests[(int)QuestPanelType.Story].Count;

        GameManager.Instance.userDataManager.userData.ObserveEveryValueChanged(x => x.BigWave)
            .Where(newValue => curEpisodeCount < maxEpisodeCount)
            .Subscribe(newValue =>
            {
                // curEpisodeCount = 2, Bigwave = 3 -> 4
                RectTransform accessorParent = placeHolders[QuestPanelType.Story];

                if (newValue - curEpisodeCount > 1)
                {
                    if (curEpisodeCount == 0)
                    {
                        accessorParent.GetChild(curEpisodeCount).GetComponent<QuestUIAccessor>()
                            .ChangeState(QuestUIAccessor.State.Acquire);
                    }
                    else
                    {
                        if (accessorParent.GetChild(curEpisodeCount).GetComponent<QuestUIAccessor>().state ==
                            QuestUIAccessor.State.Complete)
                        {
                            accessorParent.GetChild(curEpisodeCount + 1).GetComponent<QuestUIAccessor>()
                                .ChangeState(QuestUIAccessor.State.Acquire);
                        }
                    }
                }
            });
    }

    private ReactiveProperty<int> GetCountByQuestType(Quest.Condition.Type conditionType, QuestPanelType panelType)
    {
        switch (conditionType)
        {
            case Quest.Condition.Type.Kill:
                return panelType == QuestPanelType.Daily
                    ? GameManager.Instance.questData.questData.DailyKillCount
                    : GameManager.Instance.questData.questData.WeekKillCount;
            case Quest.Condition.Type.Gacha:
                return panelType == QuestPanelType.Daily
                    ? GameManager.Instance.questData.questData.DailyGachaCount
                    : GameManager.Instance.questData.questData.WeekGachaCount;
            default:
                return null;
        }
    }

    private void InitQuestUI(QuestUIAccessor accessor, Quest quest)
    {
        Sprite rewardIcon = null;

        switch (quest.reward.type)
        {
            case Quest.Reward.Type.Diamond:
                rewardIcon = uiAtlas.GetSprite("i_diamond");
                break;

            case Quest.Reward.Type.Gold:
                rewardIcon = uiAtlas.GetSprite("i_gold");
                break;
        }

        string originalText = quest.ConditionStr;
        int spaceIndex = originalText.IndexOf(' ');
        if (spaceIndex != -1 && spaceIndex + 1 < originalText.Length)
        {
            originalText = originalText.Substring(0, spaceIndex + 1) + "\n" + originalText.Substring(spaceIndex + 1);
        }

        accessor.TitleText.text = originalText;
        accessor.QuestCondition.Icon.sprite = rewardIcon;
        accessor.QuestAcquire.Icon.sprite = rewardIcon;

        accessor.RewardText.text = $"{CurrencyHelper.ToCurrencyString(quest.reward.value)}";
        accessor.QuestAcquire.CostText.text = $"{CurrencyHelper.ToCurrencyString(quest.reward.value)}";

        accessor.Initialize(rewardIcon, quest.reward.value);
    }

    private void GiveReward(Quest.Reward reward)
    {
        GameManager.Instance.userDataManager.ModifierCurrencyValue(reward.GetCurrencyType(), reward.value);
        GameManager.Instance.userDataManager.Update();
    }
}