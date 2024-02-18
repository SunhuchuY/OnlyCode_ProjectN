using BackEnd;
using UnityEngine;

namespace Authenticate
{
    public class Find
    {
        public static bool FindID(string email)
        {
            var bro = Backend.BMember.FindCustomID(email);

            if (bro.IsSuccess())
            {
#if UNITY_EDITOR
                    Debug.Log($"{email}로 아이디 전송 성공!");
#endif
                return true;
            }
            else
            {
#if UNITY_EDITOR
                    Debug.LogError($"{email}로 전송 실패");
#endif
                return false;
            }
        }

    }
}
