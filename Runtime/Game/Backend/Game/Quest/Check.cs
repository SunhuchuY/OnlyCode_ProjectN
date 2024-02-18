using System;
using UnityEngine;
using System.Collections.Generic;
using System.Globalization;

namespace BackEnd.Quest
{
    public class Check
    {
        public const string dateTimeForm = "yyyy-MM-ddTHH:mm:ss.fffZ";
        private const string tableName = "questData";

        /// <summary>
        /// 칼럼들의 쿨타임 여부를 가져옵니다.
        /// </summary>
        /// <param name="_select">선택 할 칼럼명들</param>
        /// <param name="_diff">비교 할 기간</param>
        /// <returns></returns>
        /// <exception cref="Exception">로그인 상태가 아닐 때</exception>
        public static Dictionary<string, DateTime> GetDateTIme(string[] _select)
        {
            Dictionary<string, DateTime> _dateTimes = new Dictionary<string, DateTime>();
            Where _where = new Where();

            var bro = Backend.GameData.GetMyData(tableName, _where, _select, _select.Length);
            string serverTime = bro.GetReturnValuetoJSON()["serverTime"].ToString();


            if (bro.IsSuccess())
            {
#if UNITY_EDITOR
                Debug.Log("체크 데이터를 성공적으로 불러왔습니다.");
#endif

                DateTime _current = DateTime.ParseExact(serverTime
                    , dateTimeForm, CultureInfo.InvariantCulture);
                DateTime _previousReward;

                if (bro.GetReturnValuetoJSON()["rows"].Count <= 0)
                {
#if UNITY_EDITOR
                    Debug.LogWarning("체크 데이터가 존재하지 않으므로, 데이터를 생성합니다.");
#endif
                    Insert();
                    return GetDateTIme(_select);
                }

                foreach (string _column in _select)
                {
                    if (!DateTime.TryParseExact(bro.FlattenRows()[0][_column].ToString(),
        dateTimeForm, CultureInfo.InvariantCulture, DateTimeStyles.None, out _previousReward))
                    {
                        _previousReward = DateTime.MinValue;
                    }
                    _dateTimes.Add(_column, _previousReward);
                }

                return _dateTimes;
            }
            else
            {
#if UNITY_EDITOR
                Debug.LogError("체크 데이터를 불러오지 못했습니다: " + bro);
#endif
                throw new Exception("Failed to load quest data.");
            }
        }

        public static Dictionary<string, bool> GetIsClearStory(string[] _column)
        {
            Dictionary<string, bool> _isClearStorys = new Dictionary<string, bool>();
            Where _where = new Where();
            var bro = Backend.GameData.GetMyData(tableName, _where, _column, _column.Length);

            if (bro.IsSuccess())
            {
#if UNITY_EDITOR
                Debug.Log("체크 데이터를 성공적으로 불러왔습니다.");
#endif

                if (bro.GetReturnValuetoJSON()["rows"].Count <= 0)
                {
#if UNITY_EDITOR
                    Debug.LogWarning("체크 데이터가 존재하지 않으므로, 데이터를 생성합니다.");
#endif
                    Insert();
                    return GetIsClearStory(_column);
                }

#if UNITY_EDITOR
                Debug.Log(bro.GetReturnValuetoJSON().ToJson());
#endif


                foreach (string _c in _column)
                {
                    bool _temp = (bool)bro.FlattenRows()[0][_c];
                    _isClearStorys.Add(_c, _temp);
                }

                return _isClearStorys;  
            }
            else
            {
#if UNITY_EDITOR
                Debug.LogError("체크 데이터를 불러오지 못했습니다: " + bro);
#endif
                throw new Exception("Failed to load quest data.");
            }
        }

        private static void Insert()
        {
            Where where = new Where();
            Param param = new Param();

            var bro = Backend.GameData.Insert(tableName, param);

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