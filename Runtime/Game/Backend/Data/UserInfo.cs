using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;

namespace BackEnd.Data
{
    public class UserInfo
    {
        public static UserInformation GetMyInfo()
        {
            var bro = Backend.BMember.GetUserInfo();
            JsonData userInfoJson = bro.GetReturnValuetoJSON()["row"];

            Debug.Log(userInfoJson.ToJson());

            if (bro.IsSuccess())
            {
#if UNITY_EDITOR
                Debug.Log("내 정보를 성공적으로 로드했습니다.");
#endif
            }
            else
            {
#if UNITY_EDITOR
                Debug.Log("내 정보를 로드하지 못했습니다.");
#endif
            }

            return new UserInformation
                    (userInfoJson["gamerId"].ToString()
                    , userInfoJson["nickname"] == null ? "NULL" : userInfoJson["nickname"].ToString()
                    , userInfoJson["emailForFindPassword"] == null ? "NULL" : userInfoJson["emailForFindPassword"].ToString()
                    , userInfoJson["inDate"].ToString()
                    , userInfoJson["countryCode"] == null ? "NULL" : userInfoJson["countryCode"].ToString());
        }
    }

    public struct UserInformation
    {
        public readonly string gamerId;
        public readonly string nickname;
        public readonly string emailForFindPassword;
        public readonly string inDate;
        public readonly string countryCode;

        public UserInformation
            (string gamerId, string nickname, string emailForFindPassword,
            string inDate, string countryCode)
        {
            this.gamerId = gamerId;
            this.nickname = nickname;
            this.inDate = inDate;
            this.emailForFindPassword = emailForFindPassword;
            this.countryCode = countryCode;
        }
    }

}