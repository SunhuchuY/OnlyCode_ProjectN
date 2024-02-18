using BackEnd;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Authenticate
{
    public class Token : MonoBehaviour
    {
        public static bool IsAccessTokenAlive()
        {
            var bro = Backend.BMember.IsAccessTokenAlive();
            if (bro.IsSuccess())
            {
#if UNITY_EDITOR
                Debug.Log("액세스 토큰이 살아있습니다");
#endif
                return true;
            }
            else
            {
#if UNITY_EDITOR
                Debug.Log("액세스 토큰이 남아있지 않습니다: " + bro);
#endif
                return false;
            }
        }
    }
}

