using BackEnd;
using System.Numerics;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Extension;

namespace Shop
{
    namespace Diamonds
    {
        public class Diamond_3200 : InAppPurchaseBase
        {
            private const string tableName = "userData";

            public override void OnProductFetched(Product product)
            {
            }

            public override void OnPurchaseFailed(Product product, PurchaseFailureDescription failedDescription)
            {
            }

            public override void OnPurchaseComplete(Product product)
            {
                var validation = Backend.Receipt.IsValidateGooglePurchase(product.receipt, "다이아몬드 3,200개", false);

                if (validation.IsSuccess())
                {
#if UNITY_EDITOR
                    Debug.Log("영수증 검증 성공!");
#endif

                    Purchase();
                }
                else
                {
#if UNITY_EDITOR
                    Debug.Log("영수증 검증 실패 ㅠㅠ");
#endif
                }
            }

            // 클라이언트는 Biginteger, 서버는 String으로 저장
            // 클라이언트에서 연산처리 후 업데이트함.
            protected override void Purchase()
            {
                GameManager.Instance.userDataManager.ModifierCurrencyValue(CurrencyType.Diamond, 3_200);
                GameManager.Instance.userDataManager.Update();
            }
        }
    }
}
