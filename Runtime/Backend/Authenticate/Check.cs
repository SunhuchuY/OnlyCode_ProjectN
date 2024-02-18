using BackEnd;
using UnityEngine;

namespace Authenticate
{
    public class Check
    {
        public static bool IsDuplicationNickname(string nickname)
        {
            var bro = Backend.BMember.CheckNicknameDuplication(nickname);

            if (bro.IsSuccess())
            {
#if UNITY_EDITOR
                    Debug.Log("닉네임 중복 없음");
#endif
                return false;
            }
            else
            {
#if UNITY_EDITOR
                    Debug.Log("닉네임 중복 있음 : " + bro);
#endif
                return true;
            }

        }

        /// <summary>
        /// 비밀번호 일치시 True
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public static bool IsConfirmPassword(string password)
        {
            var bro = Backend.BMember.ConfirmCustomPassword(password);

            if (bro.IsSuccess())
            {
#if UNITY_EDITOR
                    Debug.Log("비밀번호 일치");
#endif
                return true;
            }
            else
            {
#if UNITY_EDITOR
                    Debug.LogError("비밀번호 불일치" + bro);
#endif
                return false;
            }
        }
    }
}
