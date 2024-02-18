using System.Data.Common;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Extension;
using UnityEngine.UI;

namespace Shop
{
    [System.Serializable]
    public abstract class InAppPurchaseBase : MonoBehaviour
    {
        // 인앱 버튼 이벤트 추가가 일부만 존재해서, 그냥 모든 메서드 레퍼런스를 걸어 주는걸로 함
        public abstract void OnPurchaseComplete(Product product);
        public abstract void OnProductFetched(Product product);
        public abstract void OnPurchaseFailed(Product product, PurchaseFailureDescription failedDescription);

        protected abstract void Purchase();
    }

    [System.Serializable]
    public abstract class DailyInAppPurchaseBase : InAppPurchaseBase
    {
        private readonly Color CantPurchaseColor = new Color(0.490566f, 0.490566f, 0.490566f, 1f);
        [SerializeField] private Button _button;
        public readonly string ColumnName;

        private void Awake()
        {
            _button = GetComponentInChildren<Button>(); 
            UpdatePossiblePurchase();
        }

        public DailyInAppPurchaseBase(string ColumnName)
        {
            this.ColumnName = ColumnName;
        }

        public override void OnProductFetched(Product product)
        {
        }

        private void ButtonSetting(Color color, bool active)
        {
            _button.image.color = color;
            _button.enabled = active;
        }

        protected void UpdatePurchaseEventHandler()
        {
            BackEnd.Shop.CheckPurchase.Today.UpdateDate(ColumnName);
            ButtonSetting(CantPurchaseColor, false);
        }

        private void UpdatePossiblePurchase()
        {
            if (BackEnd.Shop.CheckPurchase.Today.IsPossibleTodayPurchase(ColumnName))
            {
                ButtonSetting(Color.white, true);
            }
            else
            {
                ButtonSetting(CantPurchaseColor, false);
            }
        }
    }
}
