using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Extension;
using UnityEngine.UI;

namespace Shop
{
    [System.Serializable]
    public abstract class InAppPurchaseBase : MonoBehaviour
    {
        // 인앱 버튼 이벤트 추가가 일부만 존재해서 그냥 모두 레퍼런스로 걸어 주는걸로 함
        public abstract void OnPurchaseComplete(Product product);
        public abstract void OnProductFetched(Product product);
        public abstract void OnPurchaseFailed(Product product, PurchaseFailureDescription failedDescription);

        protected abstract void Purchase();
    }

    [System.Serializable]
    public abstract class AdsBase : MonoBehaviour
    {
        protected abstract void ButtonAddListener();
        protected abstract void Reward();
    }
}
