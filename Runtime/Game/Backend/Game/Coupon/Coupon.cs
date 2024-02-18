using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BackEnd
{
    public class Coupon : MonoBehaviour
    {
        /// <summary>
        /// 사용 가능 상태는 True, 이미 사용한 상태는 False
        /// </summary>
        /// <param name="code"></param>
        /// <param name="reward"></param>
        public static bool IsAvailablyUse(string code)
        {
            var bro = Backend.Coupon.UseCoupon(code);

            if (bro.IsSuccess())
            {
#if UNITY_EDITOR
                Debug.Log("쿠폰지급이 성공적으로 완료 되었습니다.");
#endif
                return true;
            }
            else
            {
#if UNITY_EDITOR
                Debug.Log("이미 지급 완료 된 쿠폰이거나 쿠폰지급이 완료되지 못했습니다: " + bro);
#endif
                return false;
            }
        }
    }
}
