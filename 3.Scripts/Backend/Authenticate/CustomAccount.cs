using BackEnd;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace Authenticate
{

    namespace Custom
    {
        public class Account
        {
            public static bool SignUp(string id, string password, string nickname, string email)
            {
                // 회원가입, 닉네임, 이메일
                var bro = Backend.BMember.CustomSignUp(id, password);
                Authenticate.Change.Nickname(nickname);
                Authenticate.Change.Email(email);

                if (bro.IsSuccess())
                {
#if UNITY_EDITOR
                    Debug.Log("회원가입 성공!!");
                    Debug.Log(id);
                    Debug.Log(password);
                    Debug.Log(nickname);
                    Debug.Log(email);
#endif

                    return true;
                }
                else
                {
#if UNITY_EDITOR
                    Debug.Log("회원가입 실패");
#endif

                    return false;
                }
            }

            public static bool Login(string id, string password)
            {
                var bro = Backend.BMember.CustomLogin(id, password);

                if (bro.IsSuccess())
                {
#if UNITY_EDITOR
                    Debug.Log("로그인 성공!!");
#endif

                    return true;
                }
                else
                {
#if UNITY_EDITOR
                    Debug.Log("로그인 실패");
#endif

                    return false;
                }

            }

            public static bool LogOut()
            {
                var bro = Backend.BMember.Logout();

                if (bro.IsSuccess())
                {
#if UNITY_EDITOR
                    Debug.Log("로그아웃 성공!!");
#endif

                    return true;
                }
                else
                {
#if UNITY_EDITOR
                    Debug.Log("로그아웃 실패: " + bro);
#endif

                    return false;
                }
            }
        }
    }
}
