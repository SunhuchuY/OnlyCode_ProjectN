using BackEnd;
using UnityEngine;



namespace Authenticate
{

        public class Change
        {
            public static bool Email(string email)
            {
                var bro = Backend.BMember.UpdateCustomEmail(email);

                if (bro.IsSuccess())
                {
#if UNITY_EDITOR
                    Debug.Log("이메일 변경 성공");
#endif

                    return true;
                }
                else
                {
#if UNITY_EDITOR
                    Debug.Log("이메일 변경 실패");
#endif

                    return false;
                }
            }

            public static bool Nickname(string nickname)
            {
                var bro = Backend.BMember.CreateNickname(nickname);

                if (bro.IsSuccess())
                {
#if UNITY_EDITOR
                    Debug.Log("닉네임 변경 성공");
#endif

                    return true;
                }
                else
                {
#if UNITY_EDITOR
                    Debug.Log("닉네임 변경 실패");
#endif

                    return false;
                }
            }
        }
}

