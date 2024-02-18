using System.Globalization;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Shop
{
    public abstract class AdsBase : MonoBehaviour
    {
        private readonly Color CantPurchaseColor = new Color(0.490566f, 0.490566f, 0.490566f, 1f);
        [SerializeField] private Button adsButton;
        private readonly string dailyPurchaseCheckColumn;

        public AdsBase(string dailyPurchaseCheckColumn)
        {
            this.dailyPurchaseCheckColumn = dailyPurchaseCheckColumn;
        }   

        private void Awake()
        {
            ButtonAddListener();
            InitPossiblePurchase();
        }

        private void ButtonAddListener()
        {
            adsButton.onClick.AddListener(() =>
            {
                Admob.Access.Instance.RewardAds.Show(Reward);
            });
        }

        private void ButtonSetting(Color color, bool active)
        {
            adsButton.image.color = color;
            adsButton.enabled = active;
        }

        protected virtual void Reward()
        {
            ButtonSetting(CantPurchaseColor, false);
            BackEnd.Shop.CheckPurchase.Today.UpdateDate(dailyPurchaseCheckColumn);
        }

        private void InitPossiblePurchase()
        {
            // 광고 상품은 모두 일일 구매 상품에 해당됨.
            if (BackEnd.Shop.CheckPurchase.Today.IsPossibleTodayPurchase(dailyPurchaseCheckColumn))
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