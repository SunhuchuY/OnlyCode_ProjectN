using UnityEngine;
using BackEnd;

namespace Authenticate
{
    public class WithDrawAccount
    {
        public static bool WithDraw()
        {
            var bro = Backend.BMember.WithdrawAccount(24 * 7);

            if (bro.IsSuccess())
            {
#if UNITY_EDITOR
                Debug.Log("탈퇴가 정상적으로 처리되었습니다.");
#endif
                return true;
            }
            else
            {
#if UNITY_EDITOR
                Debug.Log("탈퇴 진행 에러: " + bro);
#endif
                return false;
            }
        }
    }
}

public class Qusset_UI 
{
    

}