using BackEnd;
using Cysharp.Threading.Tasks.Triggers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using UniRx;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class QuestSavedData
{
    public DateTime DailyJoin;
    public DateTime WeekJoin;

    public ReactiveProperty<int> DailyKillCount;
    public ReactiveProperty<int> WeekKillCount;
    public ReactiveProperty<int> DailyGachaCount;
    public ReactiveProperty<int> WeekGachaCount;
    public ReactiveProperty<int> EpisodeCount;

    public Dictionary<string, bool> IsUse;

    public QuestSavedData DeepCopy()
    {
        QuestSavedData copy = new QuestSavedData();

        // DateTime 필드는 값 타입이므로 직접 할당
        copy.DailyJoin = this.DailyJoin;
        copy.WeekJoin = this.WeekJoin;

        // ReactiveProperty<T>는 참조 타입이지만, 새 인스턴스를 생성하여 값을 할당
        copy.DailyKillCount = new ReactiveProperty<int>(this.DailyKillCount.Value);
        copy.WeekKillCount = new ReactiveProperty<int>(this.WeekKillCount.Value);
        copy.DailyGachaCount = new ReactiveProperty<int>(this.DailyGachaCount.Value);
        copy.WeekGachaCount = new ReactiveProperty<int>(this.WeekGachaCount.Value);
        copy.EpisodeCount = new ReactiveProperty<int>(this.EpisodeCount.Value);

        // Dictionary는 참조 타입이므로 내용을 새로운 인스턴스에 복사
        copy.IsUse = new Dictionary<string, bool>(this.IsUse);

        return copy;
    }
}

public class QuestData
{
    public const string DATETIME_FORMAT = "yyyy:mm:dd";
    public const string TABLE_NAME = "questData";

    public QuestSavedData questData { get; private set; }
    private QuestSavedData savedData;

    public QuestData()
    {
        if (Backend.IsLogin)
        {
            questData = new();

            string[] select = CreateSelect();
            BackendReturnObject bro = Backend.GameData.Get(TABLE_NAME, new Where(), select, select.Length);

            if (bro.IsSuccess())
            {
                // 모든 퀘스트 데이터는 NULL을 허용하지 않습니다.
                if (bro.GetReturnValuetoJSON()["rows"].Count <= 0)
                {
                    Backend.GameData.Insert(TABLE_NAME, new Param());
                    
                    // 다시 데이터를 불러옵니다.
                    bro = Backend.GameData.Get(TABLE_NAME, new Where(), select, select.Length);
                }

                var json = bro.FlattenRows()[0];

                questData.DailyJoin = DateTime.Parse(json[nameof(questData.DailyJoin)].ToString());
                questData.WeekJoin = DateTime.Parse(json[nameof(questData.WeekJoin)].ToString());

                questData.DailyKillCount = new(int.Parse(json[nameof(questData.DailyKillCount)].ToString()));
                questData.DailyGachaCount = new(int.Parse(json[nameof(questData.DailyGachaCount)].ToString()));
                questData.WeekKillCount = new(int.Parse(json[nameof(questData.WeekKillCount)].ToString()));
                questData.WeekGachaCount = new(int.Parse(json[nameof(questData.WeekGachaCount)].ToString()));
                questData.EpisodeCount = new(int.Parse(json[nameof(questData.EpisodeCount)].ToString()));

                questData.IsUse = new();
                QuestParser.Instance.Quests
                           .Where(q => q.Key != (int)QuestPanelUI.QuestPanelType.Story)
                           .SelectMany(q => q.Value)
                           .Where(x => x.condition.type != Quest.Condition.Type.Join)
                           .Select(x => x.ColumnStr)
                           .ToList()

                           .ForEach(s => questData.IsUse.Add(s, bool.Parse(json[s].ToString())));

                savedData = questData.DeepCopy();
                CheckReset();

                Param changedParam = GetChangedParam();
                if (changedParam.Count != 0)
                {
                    Backend.GameData.Update(TABLE_NAME, new Where(), changedParam);
                }
            }
            else
            {
                return;
            }
        }
        else
        {
            return;
        }
    }

    private void CheckReset()
    {
        DateTime currentTime = DateTime.UtcNow;

        ResetDailyDataIfNeeded(currentTime);
        ResetWeeklyDataIfNeeded(currentTime);

        GameManager.Instance.userDataManager.Update();
    }

    private void ResetDailyDataIfNeeded(DateTime currentTime)
    {
        int dailyDiff = (currentTime.Date - questData.DailyJoin.Date).Days;
        if (dailyDiff >= 1)
        {
            questData.DailyJoin = currentTime;
            questData.DailyKillCount.Value = 0;
            questData.DailyGachaCount.Value = 0;    

            ResetQuestFlags(QuestPanelUI.QuestPanelType.Daily);
            GiveJoinReward(QuestPanelUI.QuestPanelType.Daily);
        }
    }

    private void ResetWeeklyDataIfNeeded(DateTime currentTime)
    {
        int weekDiff = (currentTime.Date - questData.WeekJoin.Date).Days; 
        if (weekDiff >= 7)
        {   
            questData.WeekJoin = currentTime;
            questData.WeekKillCount.Value = 0;
            questData.WeekGachaCount.Value = 0;

            ResetQuestFlags(QuestPanelUI.QuestPanelType.Week);
            GiveJoinReward(QuestPanelUI.QuestPanelType.Week);
        }
    }

    private void ResetQuestFlags(QuestPanelUI.QuestPanelType panelType)
    {
        QuestParser.Instance.Quests[(int)panelType]
            .Where(x => x.condition.type != Quest.Condition.Type.Join)
            .Select(x => x.ColumnStr)
            .ToList()
            .ForEach(x => questData.IsUse[x] = false);
    }

    private void GiveJoinReward(QuestPanelUI.QuestPanelType panelType)
    {
        var joinQuest = QuestParser.Instance.Quests[(int)panelType]
                .FirstOrDefault(x => x.condition.type == Quest.Condition.Type.Join);


        GameManager.Instance.userDataManager.ModifierCurrencyValue(joinQuest.reward.GetCurrencyType(), joinQuest.reward.value);
        GameManager.Instance.rewardPopupUI.OnPopup(joinQuest);
    }

    private string[] CreateSelect()
    {
        List<string> select = new();

        Type type = typeof(QuestSavedData);
        var fields = type.GetFields(BindingFlags.Public | BindingFlags.Instance);

        foreach (var f in fields)
        {
            if (f.Name.Equals(nameof(QuestSavedData.IsUse)))
            {
                QuestParser.Instance.Quests
                    .Where(q => q.Key != (int)QuestPanelUI.QuestPanelType.Story)
                    .SelectMany(q => q.Value)

                    .Where(x => x.condition.type != Quest.Condition.Type.Join)
                    .Select(x => x.ColumnStr)
                    .ToList()

                    .ForEach(x => select.Add(x));
            }
            else
            {
                select.Add(f.Name);
            }
        }

        return select.ToArray();
    }

    public Param GetChangedParam()
    {
        Param param = new Param();

        Type type = typeof(QuestSavedData);
        var fields = type.GetFields(BindingFlags.Public | BindingFlags.Instance);

        foreach (var field in fields)
        {
            if (field.Name.Equals(nameof(QuestSavedData.IsUse)))
            {
                // 'IsUse' 딕셔너리에 대한 처리
                var originalIsUse = (Dictionary<string, bool>)field.GetValue(savedData);
                var savedIsUse = (Dictionary<string, bool>)field.GetValue(questData);

                foreach (var key in savedIsUse.Keys)
                {
                    bool originalValue;
                    if (!originalIsUse.TryGetValue(key, out originalValue) || originalValue != savedIsUse[key])
                    {
                        param.Add(key, savedIsUse[key]);
                    }
                }
            }
            else
            {
                var originalValue = field.GetValue(savedData);
                var savedValue = field.GetValue(questData);

                if (!object.Equals(originalValue, savedValue))
                {
                    if (field.FieldType == typeof(DateTime))
                    {
                        param.Add(field.Name, savedValue);
                    }
                    else if (field.FieldType == typeof(ReactiveProperty<int>))
                    {
                        ReactiveProperty<int> count = (ReactiveProperty<int>)savedValue;
                        param.Add(field.Name, count.Value);
                    }
                }
            }
        }

        // TODO: GC 소비가 심하므로 추후에 바꿔야합니다.
        savedData = questData.DeepCopy();
        return param;
    }
}
