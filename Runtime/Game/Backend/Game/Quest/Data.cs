using System;
using UnityEngine;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace BackEnd.Quest
{
    public class Data
    {
        public enum DataType
        {
            DailyKillCount,
            WeekKillCount,
            DailyGachaCount,
            WeekGachaCount,
            EpisodeCount,
        }

        public static readonly Dictionary<QuestPanelUI.QuestPanelType, Dictionary<DataType, string>> data = new Dictionary<QuestPanelUI.QuestPanelType, Dictionary<DataType, string>>
        {
            {
                QuestPanelUI.QuestPanelType.Week, new Dictionary<DataType, string>
                {
                    { DataType.WeekKillCount, "WeekKillCount" },
                    { DataType.WeekGachaCount, "WeekGachaCount" }
                }
            },
            {
                QuestPanelUI.QuestPanelType.Daily, new Dictionary<DataType, string>
                {
                    { DataType.DailyKillCount, "DailyKillCount" },
                    { DataType.DailyGachaCount, "DailyGachaCount" }
                }
            },
            {
                QuestPanelUI.QuestPanelType.Story, new Dictionary<DataType, string>
                {
                    { DataType.EpisodeCount, "EpisodeCount" },
                }
            }
        };



        public const string TABLENAME = "questData";
        private const string NotFoundException = "NotFoundException";

        public static void CoolTimeUpdate(string[] _select)
        {
            string _serverTime = Backend.Utils.GetServerTime().GetReturnValuetoJSON()["utcTime"].ToString();

            Param _param = new Param();
            Where _where = new Where();

            foreach (var _column in _select)
            {
                _param.Add(_column, _serverTime);
            }

            var bro = Backend.GameData.Update(TABLENAME, _where, _param);

            if (bro.IsSuccess())
            {
#if UNITY_EDITOR
                Debug.Log("쿨타임 업데이트 성공!");
#endif
            }
            else
            {
#if UNITY_EDITOR
                Debug.Log("쿨타임 업데이트 실패: " + bro);
#endif
                if (bro.GetErrorCode() == NotFoundException)
                {
                    Insert();
                    CoolTimeUpdate(_select);
                }
            }
        }

        public static void UpdateQuestData(Dictionary<DataType, int> dict)
        {
            Param param = new Param();
            Where where = new Where();

            // 대상에 맞는 문자열을 찾습니다.
            string[] select = data.SelectMany(x => x.Value.Select(y => y.Value)).ToArray();

            // 그 문자열을 param에 넣습니다.
            foreach (var d in dict)
            {
                string key = data.SelectMany(x => x.Value)
                                  .FirstOrDefault(y => y.Key.Equals(d.Key)).Value;
                if (!string.IsNullOrEmpty(key))
                {
                    param.Add(key, d.Value);
                }
            }

            var bro = Backend.GameData.Update(TABLENAME, where, param);

            if (!bro.IsSuccess())
            {
#if UNITY_EDITOR
                Debug.LogError("퀘스트 데이터를 업데이트 하지 못했습니다." + bro);
#endif
            }
        }

        public static Dictionary<QuestPanelUI.QuestPanelType, Dictionary<BackEnd.Quest.Data.DataType, int>> GetQuestsData()
        {
            // 서버에서 데이터를 불러옵니다.
            Where where = new Where();
            string[] select = data.SelectMany(x => x.Value.Select(y => y.Value)).ToArray();

            var bro = Backend.GameData.GetMyData(TABLENAME, where, select, select.Length);

            if (bro.IsSuccess())
            {
#if UNITY_EDITOR
                Debug.Log("퀘스트 데이터를 불러왔습니다!");
#endif

                if (bro.GetReturnValuetoJSON()["rows"].Count <= 0)
                {
#if UNITY_EDITOR
                    Debug.LogWarning("체크 데이터가 존재하지 않으므로, 데이터를 생성합니다.");
#endif
                    Insert();
                    return GetQuestsData();
                }
            }
            else
            {
#if UNITY_EDITOR
                Debug.LogError("서버에서 데이터를 불러오지 못했습니다." + bro);
#endif
            }

            Dictionary<QuestPanelUI.QuestPanelType, Dictionary<BackEnd.Quest.Data.DataType, int>> result = new Dictionary<QuestPanelUI.QuestPanelType, Dictionary<DataType, int>>();

            foreach (var questType in data.Keys)
            {
                var questData = new Dictionary<BackEnd.Quest.Data.DataType, int>();

                foreach (var d in data[questType])
                {
                    questData.Add(d.Key, (int)bro.FlattenRows()[0][d.Value]); // Replace 0 with actual value from 'bro'
                }

                result.Add(questType, questData);
            }

            return result;
        }

        private static void Insert()
        {
            Where where = new Where();
            Param param = new Param();
            
            var bro = Backend.GameData.Insert(TABLENAME, param);

            if (bro.IsSuccess())
            {
#if UNITY_EDITOR
                Debug.Log("데이터 생성이 성공적으로 완료되었습니다.");
#endif
            }
            else
            {
#if UNITY_EDITOR
                Debug.Log("데이터 생성이 되지 않았습니다: " + bro);
#endif
                throw new Exception("Failed to load quest data.");
            }
        }
    }
}