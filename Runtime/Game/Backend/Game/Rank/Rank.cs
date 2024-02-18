using LitJson;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BackEnd.Rank
{
    public class Rank : MonoBehaviour
    {
        public const string rankTableUUID = "42a13170-c3f8-11ee-b1e5-d7de251bde7e";
        public const string rankTableName = "rankTable";
        private const string columnName = "MaxDamage";
        private const int limit = 50;

        public static List<RankItem> GetRankList()
        {
            var bro = Backend.URank.User.GetRankList(rankTableUUID, limit);

            if (bro.IsSuccess())
            {
#if UNITY_EDITOR
                Debug.Log("랭크 테이블을 성공적으로 불러왔습니다.");
#endif
                JsonData rankListJson = bro.GetFlattenJSON()["rows"];
                List<RankItem> rankItemList = rankListJson.Cast<JsonData>()
                                          .Select(jsonItem => GetRankItem(jsonItem))
                                          .ToList();

                return rankItemList;
            }
            else
            {
#if UNITY_EDITOR
                Debug.LogError("랭크 테이블을 불러오지 못했습니다: " + bro);
#endif
                return null;
            }
        }

        public static RankItem GetMyRank()
        {
            var bro = Backend.URank.User.GetMyRank(rankTableUUID);

            if (bro.IsSuccess())
            {
#if UNITY_EDITOR
                Debug.Log("랭크 테이블을 성공적으로 불러왔습니다.");
#endif
                // json을 가져옵니다.
                JsonData rankJson = bro.FlattenRows()[0];
                return GetRankItem(rankJson);
            }
            else
            {
                if (bro.GetErrorCode() == "NotFoundException")
                {
                    AddValueMyRank(0);
                    var bro2 = Backend.URank.User.GetMyRank(rankTableUUID);
                    
                    if (bro2.IsSuccess())
                    {
                        JsonData rankJson = bro2.FlattenRows()[0];
                        return GetRankItem(rankJson);
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
#if UNITY_EDITOR
                    Debug.LogError("랭크 테이블을 불러오지 못했습니다: " + bro);
#endif
                    return null;
                }
            }

        }

        private static RankItem GetRankItem(JsonData rankJson)
        {
            return new
                (
                       rankJson.ContainsKey("nickname") ? rankJson["nickname"].ToString() : string.Empty,
                       // memo: 점수
                       // caution: 컬럼명이 score로 통일됩니다.
                       // caution: 랭킹 항목 컬럼명이 power인 경우에도 score
                       int.Parse(rankJson["score"].ToString()),
                       int.Parse(rankJson["rank"].ToString())
                    );
        }

        public static void AddValueMyRank(float additionValue)
        {
            string rowIndate = string.Empty;

            var bro = Backend.GameData.Get(rankTableName, new Where());
            double original = 0;

            if (bro.IsSuccess())
            {
                if (bro.FlattenRows().Count > 0)
                {
                    InitRankData(bro, out rowIndate);
                    original = double.Parse(bro.FlattenRows()[0][columnName].ToString());
                }
                else
                {
                    var bro2 = Backend.GameData.Insert(rankTableName, new Param() { { columnName, 0 } });

                    if (bro2.IsSuccess())
                    {
                        InitRankData(bro2, out rowIndate);
                        original = 0;
                    }
                    else
                    {
                        return;
                    }

                }
            }
            else
            {
                return;
            }

            if (rowIndate == string.Empty)
            {
                return;
            }

            Param param = new Param();
            param.Add(columnName,  original + (double)additionValue);

            Backend.URank.User.UpdateUserScore(rankTableUUID, rankTableName, rowIndate, param, callback =>
            {
            });
        }

        private static void InitRankData(BackendReturnObject bro, out string rowIndate)
        {
            rowIndate = bro.GetInDate();
        }
    }
}

