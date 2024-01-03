using GoogleMobileAds.Api;
using UnityEngine;
using UnityEngine.UI;

namespace Shop
{
    namespace Daily
    {
        public class FreeMagicStone : AdsBase
        {
            [SerializeField] private Button freeMagicStonedButton;

            private void Awake()
            {
                ButtonAddListener();
            }
            protected override void ButtonAddListener()
            {
                freeMagicStonedButton.onClick.AddListener(() =>
                {
                    Admob.Access.Instance.RewardAds.Show(Reward);
                });
            }
            protected override void Reward()
            {

            }
        }
    }
}