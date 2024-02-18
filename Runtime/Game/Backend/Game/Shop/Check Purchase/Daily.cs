using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

namespace BackEnd.Shop.CheckPurchase
{
    public class Today
    {
        private const string dateTimeForm = "yyyy-MM-ddTHH:mm:ss.fffZ";
        private const string tableName = "DailyPurchase";


        public static bool IsPossibleTodayPurchase(string _columnName)
        {
            Where _where = new Where();
            string[] _select = { _columnName };

            var bro = Backend.GameData.GetMyData(tableName, _where, _select, _select.Length);
            string _serverTime = bro.GetReturnValuetoJSON()["serverTime"].ToString();

            if (bro.IsSuccess())
            {
#if UNITY_EDITOR
                Debug.Log("일일 상점 체크 데이터를 성공적으로 불러왔습니다.");
#endif

                DateTime _current = DateTime.ParseExact(_serverTime
                    , dateTimeForm, CultureInfo.InvariantCulture);
                DateTime _previousPurchase;

                if (bro.GetReturnValuetoJSON()["rows"].Count <= 0)
                {
#if UNITY_EDITOR
                    Debug.LogWarning("일일 상점 체크 데이터가 존재하지 않으므로, 데이터를 생성 후 다시 시도합니다.");
#endif
                    Insert(_columnName);
                    return IsPossibleTodayPurchase(_columnName);
                }

#if UNITY_EDITOR
                Debug.Log(bro.GetReturnValuetoJSON().ToJson());
#endif


                // DateTime으로 변형을 시도 한 후, 날짜를 비교합니다.
                if (DateTime.TryParseExact(bro.FlattenRows()[0][_columnName].ToString(),
    dateTimeForm, CultureInfo.InvariantCulture, DateTimeStyles.None, out _previousPurchase))
                {

                    // check: 날짜를 확인합니다.
                    int diffDate = (int)((_current.Date - _previousPurchase.Date).Days);

                    if (diffDate >= 1)
                    {
#if UNITY_EDITOR
                        Debug.Log($"{_columnName}은(는) 구매 가능 상태입니다.");
#endif
                        return true;
                    }
                    else
                    {
#if UNITY_EDITOR
                        Debug.Log($"{_columnName}은(는) 구매 가능 상태가 아닙니다.");
#endif
                        return false;
                    }
                }
                else
                {
#if UNITY_EDITOR
                    Debug.Log($"일일 상점 체크 데이터가 비어있으므로 {_columnName}은(는) 구매 가능 상태입니다.");
#endif
                    return true;

                }
            }
            else
            {
#if UNITY_EDITOR
                Debug.LogError("일일 상점 체크 데이터를 불러오지 못했습니다: " + bro);
#endif
                return false;
            }
        }

        public static void UpdateDate(string _columnName)
        {
            string _serverTime = Backend.Utils.GetServerTime().GetReturnValuetoJSON()["utcTime"].ToString();

            Where _where = new Where();
            Param _param = new Param() 
            {
                { _columnName, _serverTime}
            };  
                
            var bro = Backend.GameData.Update(tableName, _where, _param);

            if (bro.IsSuccess())
            {
#if UNITY_EDITOR
                Debug.Log("서버시간을 성공적으로 저장했습니다.");
#endif

            }
            else
            {
#if UNITY_EDITOR
                Debug.Log("서버시간을 저장하지 못했습니다: " + bro);
#endif
                if (bro.GetErrorCode() == "NotFoundException")
                {
                    Insert(_columnName);
                    UpdateDate(_columnName);
                }
                else
                {
                    throw new System.Exception("not found dailyPurchase data in server.");
                }
            }
        }

        private static void Insert(string _columnName)
        {
            Where _where = new Where();
            Param _param = new Param()
            {
                { _columnName, string.Empty }
            };

            var bro = Backend.GameData.Insert(tableName, _param);

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
            }
        }
    }
}