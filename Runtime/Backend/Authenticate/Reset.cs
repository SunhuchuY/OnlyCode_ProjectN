using BackEnd;
using UnityEngine;


namespace Authenticate
{
    public class Reset
    {
        public static bool Password(string id, string email)
        {
            var bro = Backend.BMember.ResetPassword(id, email);

            if (bro.IsSuccess())
            {
#if UNITY_EDITOR
                Debug.Log("비밀번호 리셋 이메일 전송" + bro);

#endif

                return true;
            }
            else
            {
#if UNITY_EDITOR

                Debug.LogError("��й�ȣ �ʱ�ȭ �̸��� ���ۿ���" + bro);
#endif

                return false;
            }
        }

    }
}