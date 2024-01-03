using BackEnd;
using System.Numerics;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Extension;


namespace Shop
{
    namespace Daily
    {

        public class GoldPack_1 : InAppPurchaseBase
        {
            private const string tableName = "userData";

            public override void OnProductFetched(Product product)
            {

            }

            public override void OnPurchaseComplete(Product product)
            {
                var validation = Backend.Receipt.IsValidateGooglePurchase(product.receipt, "골드팩_1", false);

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
                // 클라이언트, 연산하세요
                BigInteger test = 1000;

                Param param = new Param
                {
                    { "Gold", test },
                    { "Diamond", test }
                };

                Backend.GameData.Update(tableName, new Where(), param, (callback) => 
                { 
                });
            }
        }
    }
}
