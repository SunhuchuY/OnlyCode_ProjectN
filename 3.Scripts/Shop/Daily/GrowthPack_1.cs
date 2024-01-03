using System.Collections;
using UnityEngine.Purchasing;
using UnityEngine;
using UnityEngine.Purchasing.Extension;
using BackEnd;
using System.Numerics;

namespace Shop
{
    namespace Daily
    {
        public class GrowthPack_1 : InAppPurchaseBase
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
                var validation = Backend.Receipt.IsValidateGooglePurchase(product.receipt, "성장팩_1", false);

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
                // 클라이언트, 연산하세요
                BigInteger test = 1000;

                Param param = new Param
                {
                    { "MagicStone", test },
                    { "Diamond", test }
                };

                Backend.GameData.Update(tableName, new Where(), param, (callback) =>
                {
                });
            }
        }
    }
}

