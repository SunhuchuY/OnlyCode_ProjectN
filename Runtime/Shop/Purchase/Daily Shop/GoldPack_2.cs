using BackEnd;
using System.Numerics;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Extension;

namespace Shop
{
    namespace Daily
    {
        public class GoldPack_2 : DailyInAppPurchaseBase
        {
            public GoldPack_2() : base("GoldPack2") { }

            public override void OnPurchaseComplete(Product product)
            {
                var validation = Backend.Receipt.IsValidateGooglePurchase(product.receipt, "골드팩_2", false);

                BackEnd.Shop.CheckPurchase.Today.UpdateDate(base.ColumnName);

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

            public override void OnPurchaseFailed(Product product, PurchaseFailureDescription failedDescription)
            {

            }

            // 클라이언트는 Biginteger, 서버는 String으로 저장
            // 클라이언트에서 연산처리 후 업데이트함.
            protected override void Purchase()
            {
                base.UpdatePurchaseEventHandler();

                GameManager.Instance.userDataManager.ModifierCurrencyValue(CurrencyType.Diamond, 11_500);
                GameManager.Instance.userDataManager.ModifierCurrencyValue(CurrencyType.Gold, 5_500);
                GameManager.Instance.userDataManager.Update();
            }
        }
    }
}

