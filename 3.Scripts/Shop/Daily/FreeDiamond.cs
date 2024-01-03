using GoogleMobileAds.Api;
using UnityEngine;
using UnityEngine.UI;

namespace Shop
{
    namespace Daily
    {
        public class FreeDiamond : AdsBase 
        {
            [SerializeField] private Button freeDiamondButton;
            private void Awake()
            {
                ButtonAddListener();
            }
            protected override void ButtonAddListener()
            {
                freeDiamondButton.onClick.AddListener(() =>
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